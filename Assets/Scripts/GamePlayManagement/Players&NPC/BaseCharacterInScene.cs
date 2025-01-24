using System;
using DataUnits.ItemScriptableObjects;
using GameDirection.GeneralLevelManager.ShopPositions;
using GamePlayManagement.ItemManagement.Guards;
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
        public NavMeshAgent GetNavMeshAgent { get; }
        public void ToggleNavMesh(bool isActive);
    }
    
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(BaseAnimatedAgent))]
    public abstract class BaseCharacterInScene : MonoBehaviour, IBaseCharacterInScene
    {
        protected const float BaseWalkSpeed = 3.5f;
        protected const float BaseRunSpeed = 15f;
        protected const int BaseAwakeTime = 5000;
        
        protected const string Idle = "Idle";
        protected const string Walk = "Walk";
        protected const string Run = "Run";        
        protected const string SearchAround = "SearchAround";

        public Guid CharacterId => MCharacterId;
        public NavMeshAgent GetNavMeshAgent => MyNavMeshAgent;
        public void ToggleNavMesh(bool isActive)
        {
            //ObstacleComponent.enabled = !isActive;
            if (MyNavMeshAgent.enabled)
            {
                MyNavMeshAgent.isStopped = !isActive;
                MyNavMeshAgent.enabled = isActive;
            }
            else
            {
                MyNavMeshAgent.enabled = isActive;
                MyNavMeshAgent.isStopped = !isActive;
            }
        }

        protected Guid MCharacterId;
        
        protected IItemTypeStats MyStats;
        protected BaseCharacterMovementStatus MCharacterMovementStatus = 0;
        
        protected IBaseAnimatedAgent BaseAnimator;
        protected NavMeshAgent MyNavMeshAgent;
        protected Vector3 MInitialPosition;
        protected IShopPositionsManager PositionsManager;
        //protected NavMeshObstacle ObstacleComponent;

        protected float MRotationSpeed = 12;
        [SerializeField] protected Transform headTransform;     

        #region Events
        private delegate void ReachDestination();
        private event ReachDestination WalkingDestinationReached;

        #endregion
        protected virtual void Awake()
        {
            CheckId();
            PositionsManager = FindFirstObjectByType<ShopPositionsManager>();    
            BaseAnimator = GetComponent<BaseAnimatedAgent>();
            MyNavMeshAgent = GetComponent<NavMeshAgent>();
            BaseAnimator.Initialize(GetComponent<Animator>());
            //ObstacleComponent = GetComponent<NavMeshObstacle>();
            //ObstacleComponent.enabled = false;
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
        }
        protected virtual void RotateTowardsYOnly(Transform rotatingObject, Transform facingTowards)
        {
            Vector3 targetDirection = facingTowards.position - rotatingObject.position;
            targetDirection.y = 0; // Ignore height difference
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
        protected void NavAgentUpdateStatusStats(BaseCharacterMovementStatus currentStatus)
        {
            MyNavMeshAgent.speed = GetStatusSpeed(currentStatus);
        }
        protected virtual float GetStatusSpeed(BaseCharacterMovementStatus currentStatus)
        {
            return 1;
        }

        protected void SetMovementDestination(Vector3 targetPosition)
        {
            MyNavMeshAgent.enabled = true;
            MyNavMeshAgent.isStopped = false;
            MyNavMeshAgent.SetDestination(targetPosition);
        }

        protected virtual void SetCharacterAttitudeStatus(BaseCustomerAttitudeStatus customerAttitudeStatus)
        {
            
        }
        protected virtual void SetCharacterAttitudeStatus(GuardSpecialAttitudeStatus guardAttitudeStatus)
        {
            
        }
        protected void SetCharacterMovementStatus(BaseCharacterMovementStatus newMovementStatus)
        {
            MCharacterMovementStatus = 0;
            MCharacterMovementStatus |= newMovementStatus;
            //set
            switch (newMovementStatus)
            {
                case  BaseCharacterMovementStatus.Walking:
                    NavAgentUpdateStatusStats(BaseCharacterMovementStatus.Walking);
                    BaseAnimator.ChangeAnimationState(Walk);
                    break;
                case  BaseCharacterMovementStatus.Idle:
                    //ToggleNavMesh(false);
                    BaseAnimator.ChangeAnimationState(Idle);
                    break;
                case  BaseCharacterMovementStatus.Running:
                    NavAgentUpdateStatusStats(BaseCharacterMovementStatus.Running);
                    //ToggleNavMesh(true);
                    BaseAnimator.ChangeAnimationState(Run);
                    break;
            }
        }

        protected virtual void ProcessInViewTargets()
        {
            //Must be implemented by inheritor if used     
        }
        protected void EvaluateWalkingDestination()
        {
            if (MyNavMeshAgent.destination.Equals(default(Vector3)))
            {
                Debug.LogWarning("Destination to walk to must be already set");
                return;
            }
            ConfirmAttitudeStatusData();
            if (MyNavMeshAgent.remainingDistance < 1f && !MyNavMeshAgent.isStopped)
            {
                MyNavMeshAgent.isStopped = true;
                OnWalkingDestinationReached();
            }
        }

        protected virtual void ConfirmAttitudeStatusData()
        {
            
        }

        protected virtual void OnWalkingDestinationReached()
        {
            WalkingDestinationReached?.Invoke();
        }
    }
}