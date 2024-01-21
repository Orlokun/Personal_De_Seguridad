using System;
using GameDirection.GeneralLevelManager;
using GameDirection.GeneralLevelManager.ShopPositions;
using GamePlayManagement.Players_NPC.Animations;
using GamePlayManagement.Players_NPC.Animations.Interfaces;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management;
using UnityEngine;
using UnityEngine.AI;

namespace GamePlayManagement.Players_NPC
{
    public interface IBaseCharacterInScene
    {
        public Guid CharacterId { get; }
    }
    
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(NavMeshObstacle))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(BaseAnimatedAgent))]
    public abstract class BaseCharacterInScene : MonoBehaviour, IBaseCharacterInScene
    {
        protected const string Idle = "Idle";
        protected const string Walk = "Walk";
        public Guid CharacterId => MCharacterId;
        protected Guid MCharacterId;
        
        protected BaseCharacterMovementStatus _mCharacterMovementStatus = 0;
        
        protected IBaseAnimatedAgent BaseAnimator;
        protected NavMeshAgent NavMeshAgent;
        protected Vector3 MInitialPosition;
        protected IShopPositionsManager PositionsManager;
        protected NavMeshObstacle ObstacleComponent;

        protected float MRotationSpeed = 12;
        [SerializeField] protected Transform headTransform;     

        #region Events
        private delegate void ReachDestination();
        private event ReachDestination WalkingDestinationReached;

        #endregion
        protected virtual void Awake()
        {
            CheckId();
            BaseAnimator = GetComponent<BaseAnimatedAgent>();
            NavMeshAgent = GetComponent<NavMeshAgent>();
            BaseAnimator.Initialize(GetComponent<Animator>());
            ObstacleComponent = GetComponent<NavMeshObstacle>();
            ObstacleComponent.enabled = false;
            WalkingDestinationReached += ReachWalkingDestination;
        }

        private void CheckId()
        {
            if (MCharacterId == Guid.Empty)
            {
                MCharacterId = Guid.NewGuid();
            }
        }
        protected virtual void Start()
        {
            PositionsManager = FindObjectOfType<ShopPositionsManager>();
        }
        protected virtual void RotateTowardsYOnly(Transform rotatingObject, Transform facingTowards)
        {
            Vector3 targetDirection = facingTowards.position - rotatingObject.position;
            targetDirection.y = 0; // Ignore height difference
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            // Smoothly rotate towards the target
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, MRotationSpeed * Time.deltaTime);
        }
        protected virtual void RotateTowards(Transform rotatingObject, Transform facingTowards)
        {
            Vector3 targetDirection = facingTowards.position - rotatingObject.position;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            // Smoothly rotate towards the target
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, MRotationSpeed * Time.deltaTime);
        }

        protected virtual void ReachWalkingDestination()
        {
            
        }
        
        protected void StartWalking()
        {
            SetCharacterMovementStatus(BaseCharacterMovementStatus.Walking);
            BaseAnimator.ChangeAnimationState(Walk);
        }
        protected virtual void SetCharacterMovementStatus(BaseCharacterMovementStatus newMovementStatus)
        {
            _mCharacterMovementStatus = 0;
            _mCharacterMovementStatus |= newMovementStatus;
            //set
            switch (newMovementStatus)
            {
                case  BaseCharacterMovementStatus.Walking:
                    ObstacleComponent.enabled = false;
                    NavMeshAgent.enabled = true;
                    NavMeshAgent.isStopped = false;
                    BaseAnimator.ChangeAnimationState(Walk);
                    break;
                case  BaseCharacterMovementStatus.Idle:
                    NavMeshAgent.isStopped = true;
                    NavMeshAgent.enabled = false;
                    ObstacleComponent.enabled = true;
                    BaseAnimator.ChangeAnimationState(Idle);
                    break;
            }
        }

        protected virtual void ProcessInViewTargets()
        {
            //Must be implemented by inheritor if used     
        }
        protected void EvaluateWalkingDestination()
        {
            if (NavMeshAgent.destination.Equals(default(Vector3)))
            {
                Debug.LogWarning("Destination to walk to must be already set");
                return;
            }

            if (NavMeshAgent.remainingDistance < 1f && !NavMeshAgent.isStopped)
            {
                NavMeshAgent.isStopped = true;
                OnWalkingDestinationReached();
            }
        }
        protected virtual void OnWalkingDestinationReached()
        {
            WalkingDestinationReached?.Invoke();
        }
    }
}