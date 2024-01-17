using System;
using GameDirection.GeneralLevelManager;
using GamePlayManagement.Players_NPC.Animations;
using GamePlayManagement.Players_NPC.Animations.Interfaces;
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
        
        protected IBaseAnimatedAgent BaseAnimator;
        protected NavMeshAgent NavMeshAgent;
        protected Vector3 MInitialPosition;
        protected IShopPositionsManager PositionsManager;
        protected NavMeshObstacle ObstacleComponent;

        protected float MRotationSpeed = 12;
        [SerializeField] protected Transform headTransform;     

        protected virtual void Awake()
        {
            CheckId();
            BaseAnimator = GetComponent<BaseAnimatedAgent>();
            NavMeshAgent = GetComponent<NavMeshAgent>();
            BaseAnimator.Initialize(GetComponent<Animator>());
            ObstacleComponent = GetComponent<NavMeshObstacle>();
            ObstacleComponent.enabled = false;
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

        protected virtual void ProcessInViewTargets()
        {
            //Must be implemented by inheritor if used     
        }
    }
}