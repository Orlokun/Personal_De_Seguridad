using System;
using GameDirection.GeneralLevelManager;
using Players_NPC.Animations;
using Players_NPC.Animations.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace Players_NPC
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(BaseAnimatedAgent))]
    public abstract class BaseCharacterInScene : MonoBehaviour
    {
        protected const string IDLE = "Idle";
        protected const string WALK = "Walk";

        
        protected Guid MCustomerId;
        protected IBaseAnimatedAgent BaseAnimator;
        protected int CurrentProductSearchIndex = 0;
        protected NavMeshAgent NavMeshAgent;
        protected Vector3 MInitialPosition;
        protected IShopPositionsManager _positionsManager;


        protected float MRotationSpeed = 12;
        [SerializeField] protected Transform headTransform;     

        protected virtual void Awake()
        {
            MCustomerId = Guid.NewGuid();
            MInitialPosition = transform.position;
            BaseAnimator = GetComponent<BaseAnimatedAgent>();
            NavMeshAgent = GetComponent<NavMeshAgent>();
            BaseAnimator.Initialize(GetComponent<Animator>());
        }

        protected virtual void Start()
        {
            ShopPositionsManager positionManagerComponent;
            var foundPositionManager = TryGetComponent<ShopPositionsManager>(out positionManagerComponent);
            Debug.Log($"[BaseCharacterInScene.Start] Found Position Manager: {foundPositionManager}");
            _positionsManager = positionManagerComponent;
            _positionsManager = FindObjectOfType<ShopPositionsManager>();
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
    }
}