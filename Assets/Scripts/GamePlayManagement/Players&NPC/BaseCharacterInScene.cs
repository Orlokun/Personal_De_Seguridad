using System;
using DataUnits.ItemScriptableObjects;
using GameDirection.GeneralLevelManager.ShopPositions;
using GamePlayManagement.ItemManagement.Guards;
using GamePlayManagement.LevelManagement.LevelObjectsManagement;
using GamePlayManagement.Players_NPC.Animations;
using GamePlayManagement.Players_NPC.Animations.Interfaces;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.AttitudeStates;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.GuardStates;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.MovementStates;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

namespace GamePlayManagement.Players_NPC
{
    /// <summary>
    /// THIS CLASS MUST BE TURNED INTO A PROPER STATE MACHINE
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(BaseAnimatedAgent))]
    public abstract class BaseCharacterInScene : MonoBehaviour, IBaseCharacterInScene
    {
        //Many of these should eventually be managed through code: speeds, etc.
        #region Constants
        protected const float BaseWalkSpeed = 3.5f;
        protected const float BaseRunSpeed = 15f;
        protected const int BaseAwakeTime = 5000;
        
        public const string SearchAround = "SearchAround";
        #endregion

        #region Protected Data, components and references
        
        protected IStoreEntrancePosition MEntranceData;
        public IStoreEntrancePosition EntranceData => MEntranceData;
        
        protected Guid MCharacterId;
        protected IItemTypeStats MyStats;
        protected BaseCharacterMovementStatus MCharacterMovementStatus = BaseCharacterMovementStatus.Idle;
        protected IBaseAnimatedAgent MBaseAnimator;
        public IBaseAnimatedAgent BaseAnimator => MBaseAnimator;
        protected NavMeshAgent MyNavMeshAgent;
        protected Vector3 MInitialPosition;
        public Vector3 InitialPosition => MInitialPosition;
        public IShopPositionsManager PositionsManager;
        #endregion

        #region StateMachine
        protected StateMachine<IMovementState> _mMovementStateMachine;
        protected StateMachine<IAttitudeState> _mAttitudeStateMachine;
        #endregion

        #region ProceduralAnimConstraints 
        [SerializeField] protected MultiAimConstraint mHeadAimConstraint;
        [SerializeField] protected TwoBoneIKConstraint mGrabObjectConstraint;
        [SerializeField] protected TwoBoneIKConstraint mInspectObjectConstraint;
        [SerializeField] protected Transform rightHand;
        
        public TwoBoneIKConstraint InspectObjectConstraint => mInspectObjectConstraint;
        public MultiAimConstraint HeadAimConstraint => mHeadAimConstraint;
        public TwoBoneIKConstraint GrabObjectConstraint => mGrabObjectConstraint;
        public Transform TempObjectTargetOfInterest => MTempTargetOfInterest;
        public Tuple<Transform, IStoreProductObjectData> TempStoreProductOfInterest => MTempStoreProductOfInterest;
        #endregion

        
        #region Product of Interest
        protected Tuple<Transform, IStoreProductObjectData> MTempStoreProductOfInterest;

        public virtual void ClearProductOfInterest()
        {
            MTempStoreProductOfInterest = null;
        }
        
        public void SetTempProductOfInterest(Tuple<Transform, IStoreProductObjectData> productOfInterest)
        {
            MTempStoreProductOfInterest = productOfInterest;
            MTempTargetOfInterest = MTempStoreProductOfInterest.Item1;
        }
        
        #endregion
        
        #region Object of Interest
        protected Transform MTempTargetOfInterest;
        #endregion
        
        public Guid CharacterId => MCharacterId;
        public NavMeshAgent GetNavMeshAgent => MyNavMeshAgent;
        public void ToggleNavMesh(bool isActive)
        {
            if (MyNavMeshAgent.enabled && isActive)
            {
                //Just make sure Nav Agent is not Stopped
                MyNavMeshAgent.isStopped = false;
                return;
            }
            if(MyNavMeshAgent.enabled && !isActive)
            {
                MyNavMeshAgent.isStopped = true;
                MyNavMeshAgent.enabled = false;
                return;
            }

            if (MyNavMeshAgent.enabled == false)
            {
                MyNavMeshAgent.enabled = isActive;
                if (MyNavMeshAgent.enabled == false)
                {
                    return;
                }
                MyNavMeshAgent.isStopped = !isActive;
            }
        }

        public void ChangeMovementState<T>() where T : IMovementState
        {
            _mMovementStateMachine.ChangeState<T>();
        }
        public void ChangeAttitudeState<T>() where T : IAttitudeState
        {
            _mAttitudeStateMachine.ChangeState<T>();
        }
        //protected NavMeshObstacle ObstacleComponent;

        protected float MRotationSpeed = 12;
        [SerializeField] protected Transform headTransform;     

        #region Events
        public delegate void ReachDestination();
        public event ReachDestination WalkingDestinationReached;

        #endregion
        protected virtual void Awake()
        {
            CheckId();
            InitiateStateMachines();
            PositionsManager = FindFirstObjectByType<ShopPositionsManager>();    
            MBaseAnimator = GetComponent<BaseAnimatedAgent>();
            MyNavMeshAgent = GetComponent<NavMeshAgent>();
            BaseAnimator.Initialize(GetComponent<Animator>());
            EnsureNavMeshAgentOnNavMesh();
            //ObstacleComponent = GetComponent<NavMeshObstacle>();
            //ObstacleComponent.enabled = false;
        }

        private void CheckId()
        {
            if (MCharacterId == Guid.Empty)
            {
                MCharacterId = Guid.NewGuid();
            }
        }
        public void EnsureNavMeshAgentOnNavMesh()
        {
            if (!MyNavMeshAgent.isOnNavMesh)
            {
                Vector3 nearestNavMeshPosition;
                if (FindNearestNavMeshPosition(transform.position, out nearestNavMeshPosition))
                {
                    MyNavMeshAgent.Warp(nearestNavMeshPosition);
                }
                else
                {
                    Debug.LogWarning("No valid NavMesh position found nearby.");
                }
            }
        }

        private bool FindNearestNavMeshPosition(Vector3 position, out Vector3 nearestNavMeshPosition, float maxDistance = 1.0f)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(position, out hit, maxDistance, NavMesh.AllAreas))
            {
                nearestNavMeshPosition = hit.position;
                return true;
            }

            nearestNavMeshPosition = Vector3.zero;
            return false;
        }
        
        private void InitiateStateMachines()
        {
            //Movement State Machine
            _mMovementStateMachine = new StateMachine<IMovementState>();
            _mMovementStateMachine.AddState(new IdleMovementState(this));
            _mMovementStateMachine.AddState(new WalkingState(this));
            _mMovementStateMachine.AddState(new RunningState(this));
            
            //Attitude State Machine
            _mAttitudeStateMachine = new StateMachine<IAttitudeState>();
            _mAttitudeStateMachine.AddState(new IdleAttitudeState(this));
            _mAttitudeStateMachine.AddState(new AccessingBuildingState(this));
            _mAttitudeStateMachine.AddState(new TalkingState(this));
            _mAttitudeStateMachine.AddState(new ShoppingState(this));
            _mAttitudeStateMachine.AddState(new StealingState(this));
            _mAttitudeStateMachine.AddState(new EvaluatingProductState(this));
            _mAttitudeStateMachine.AddState(new ScreamingState(this));
            _mAttitudeStateMachine.AddState(new FightingState(this));
            _mAttitudeStateMachine.AddState(new PayingState(this));
            _mAttitudeStateMachine.AddState(new ScaredCrouchState(this));
            _mAttitudeStateMachine.AddState(new ScaredRunningState(this));
            _mAttitudeStateMachine.AddState(new LeavingBuildingState(this));
            
            //Guards Base AttitudeStates
            _mAttitudeStateMachine.AddState(new GuardIdleState(this));
            
            
        }
        
        protected virtual void Start()
        {
        }
        public void RotateTowardsYOnly(Transform facingTowards)
        {
            Vector3 targetDirection = facingTowards.position - transform.position;
            targetDirection.y = 0; // Ignore height difference
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            // Smoothly rotate towards the target
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, MRotationSpeed * Time.deltaTime);
        }

        protected virtual void ReachWalkingDestination()
        {
            
        }

        protected void NavAgentUpdateStatusStats()
        {
            MyNavMeshAgent.speed = GetStatusSpeed();
        }
        protected virtual float GetStatusSpeed()
        {
            return 1;
        }



        public virtual void ChangeCharacterAttitudeState(BaseCharacterAttitudeStatus characterAttitudeStatus)
        {
            
        }
        public virtual void SetCharacterAttitudeStatus(GuardSpecialAttitudeStatus guardAttitudeStatus)
        {
            
        }


        protected virtual void ProcessInViewTargets()
        {
            //Must be implemented by inheritor if used     
        }
        public void SetMovementDestination(Vector3 targetPosition)
        {
            ToggleNavMesh(true);
            MyNavMeshAgent.SetDestination(targetPosition);
        }
        public void EvaluateWalkingDestination()
        {
            if (MyNavMeshAgent.destination.Equals(default(Vector3)))
            {
                Debug.LogWarning("Destination to walk to must be already set");
                return;
            }
            if (!MyNavMeshAgent.enabled)
            {
                return;
            }

            if (MyNavMeshAgent.isStopped) { return; }
            if (MyNavMeshAgent.remainingDistance > 1f) return;
            
            MyNavMeshAgent.isStopped = true;
            OnWalkingDestinationReached();
        }
        protected virtual void OnWalkingDestinationReached()
        {
            WalkingDestinationReached?.Invoke();
        }

        public void DestroyCharacter()
        {
            Destroy(gameObject);
        }
    }
}