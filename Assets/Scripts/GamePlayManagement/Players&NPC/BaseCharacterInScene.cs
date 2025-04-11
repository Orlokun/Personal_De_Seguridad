using System;
using DataUnits.ItemScriptableObjects;
using GameDirection.GeneralLevelManager.ShopPositions;
using GamePlayManagement.LevelManagement.LevelObjectsManagement;
using GamePlayManagement.Players_NPC.Animations;
using GamePlayManagement.Players_NPC.Animations.Interfaces;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.AttitudeStates;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.AttitudeStates.BaseCharacter;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.MovementStates;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

namespace GamePlayManagement.Players_NPC
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(BaseAnimatedAgent))]
    public abstract class BaseCharacterInScene : MonoBehaviour, IBaseCharacterInScene
    {
        //Many of these should eventually be managed through code: speeds, etc.
        #region Constants
        protected const float BaseWalkSpeedMin = 3f;
        protected const float BaseWalkSpeedMax = 4.5f;
        
        protected const float BaseRunSpeedMin = 6.5f;
        protected const float BaseRunSpeedMax= 8.5f;
        protected const int BaseAwakeTime = 8000;
        
        #endregion

        #region Protected Data, components and references
        protected IStoreEntrancePosition MEntranceData;                                     //TODO: Turn into list for eventual many entrances
        protected Guid MCharacterId;
        protected IItemTypeStats MyStats;
        protected IBaseAnimatedAgent MBaseAnimator;
        protected NavMeshAgent MyNavMeshAgent;
        protected Vector3 MInitialPosition;
        #endregion

        public Vector3 InitialPosition => MInitialPosition;

        public IShopPositionsManager GetPositionsManager => MPositionsManager;
        protected IShopPositionsManager MPositionsManager;
        public IBaseAnimatedAgent BaseAnimator => MBaseAnimator;
        public IStoreEntrancePosition EntranceData => MEntranceData;

        protected int MCharacterSpeedLevel;
        public int GetCharacterSpeedLevel => MCharacterSpeedLevel;

        
        public IMovementState CurrentMovementState => _mMovementStateMachine.CurrentState;
        public IAttitudeState CurrentAttitudeState => _mAttitudeStateMachine.CurrentState;
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

        public virtual void ChangeMovementState<T>() where T : IMovementState
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
            InitiateBaseStateMachines();
            MPositionsManager = FindFirstObjectByType<ShopPositionsManager>();    
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
        
        protected virtual void InitiateBaseStateMachines()
        {
            //Movement State Machine
            _mMovementStateMachine = new StateMachine<IMovementState>();
            _mMovementStateMachine.AddState(new IdleMovementState(this));
            _mMovementStateMachine.AddState(new WalkingState(this));
            _mMovementStateMachine.AddState(new RunningState(this));
            
            //Attitude State Machine
            _mAttitudeStateMachine = new StateMachine<IAttitudeState>();
            _mAttitudeStateMachine.AddState(new IdleAttitudeState(this));
            _mAttitudeStateMachine.AddState(new WalkingTowardsPositionState(this));
            _mAttitudeStateMachine.AddState(new TalkingState(this));
            
            _mAttitudeStateMachine.AddState(new ScreamingState(this));
            _mAttitudeStateMachine.AddState(new FightingState(this));
            _mAttitudeStateMachine.AddState(new ScaredCrouchState(this));
            _mAttitudeStateMachine.AddState(new ScaredRunningState(this));
            _mAttitudeStateMachine.AddState(new DeathShotState(this));
            _mAttitudeStateMachine.AddState(new ShootTargetState(this));
        }
        
        protected virtual void Start()
        {
        }
        public void RotateTowardsYOnly(Vector3 facingTowards)
        {
            Vector3 targetDirection = facingTowards - transform.position;
            targetDirection.y = 0; // Ignore height difference
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            // Smoothly rotate towards the target
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, MRotationSpeed * Time.deltaTime);
        }
        
        public void SetRotateTowardsYOnly(Vector3 facingTowards)
        {
            Vector3 targetDirection = facingTowards - transform.position;
            targetDirection.y = 0; // Ignore height difference
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            // Smoothly rotate towards the target
            transform.rotation = targetRotation;
        }

        protected virtual void ReachWalkingDestination()
        {
            
        }

        protected void UpdateCharacterSpeed(int speedLevel)
        {
            switch (_mMovementStateMachine.CurrentState)
            {
                case IdleMovementState:
                    MyNavMeshAgent.speed = 0;
                    break;
                case WalkingState:
                    MyNavMeshAgent.speed =  CalculateWalkSpeed(speedLevel);
                    break;
                case RunningState:
                    MyNavMeshAgent.speed =  CalculateRunSpeed(speedLevel);
                    break;
                default:
                    return;
            }
            return;
        }
        
        //TODO: Eventually change to range depending on character, not rigid ranges.
        private float CalculateWalkSpeed(int speedLevel)
        {
            var speedRange = (BaseWalkSpeedMax - BaseWalkSpeedMin)/10;
            return BaseWalkSpeedMin + (speedRange * speedLevel);
        }
        
        private float CalculateRunSpeed(int speedLevel)
        {
            var speedRange = (BaseRunSpeedMax - BaseRunSpeedMin)/10;
            return BaseRunSpeedMin + (speedRange * speedLevel);
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
            ChangeMovementState<IdleMovementState>();
        }

        public virtual void OnWalkingDestinationReached()
        {
            WalkingDestinationReached?.Invoke();
        }

        public void DestroyCharacter()
        {
            Destroy(gameObject);
        }

        public void UpdateNavMeshSpeed(int speedLevel)
        {
            UpdateCharacterSpeed(speedLevel);
        }
    }
}