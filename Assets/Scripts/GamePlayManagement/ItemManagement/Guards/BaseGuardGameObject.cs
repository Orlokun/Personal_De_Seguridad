using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataUnits.ItemScriptableObjects;
using ExternalAssets._3DFOV.Scripts;
using GameDirection.GeneralLevelManager.ShopPositions;
using GamePlayManagement.ItemPlacement;
using GamePlayManagement.ItemPlacement.PlacementManagement;
using GamePlayManagement.Players_NPC;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.AttitudeStates;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.AttitudeStates.GuardStates;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.GuardStates;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.MovementStates;
using InputManagement.MouseInput;
using Unity.Cinemachine;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace GamePlayManagement.ItemManagement.Guards
{
    public class BaseGuardGameObject : BaseCharacterInScene, IInteractiveClickableObject, IBaseGuardGameObject, IInitializeWithArg1<IItemObject>
    {

        private Coroutine _mCurrentRoutine;
        private IItemObject _myGuardData;
        public IItemObject GuardBaseData => _myGuardData;

        public void SetInPlacementStatus(bool inPlacement)
        {
            _mInPlacement = inPlacement;
        }
        private bool _mInPlacement;

        #region ClicakbleObjectMembers
        private bool _mIsClicked;
        public Vector3 CurrentManualInspectionPosition { get; private set; }

        #endregion

        #region WeaponManagementMembers
        [SerializeField] private ParticleSystem myParticleSystem;
        public Transform GunParentTransform => mGunPositionTransform;
        [SerializeField] private Transform mGunPositionTransform;
        private IWeaponBaseData CurrentIWeaponBaseData => (IWeaponBaseData)_currentWeaponItem?.ItemStats;
        private IItemObject _currentWeaponItem;
        private GameObject _currentWeaponObject;
        
        
        #endregion
        /// <summary>
        /// Not Used yet
        /// </summary>
        #region CameraPerspective
        private CinemachineCamera _myVc;
        public CinemachineCamera VirtualCamera => _myVc;
        #endregion
        
        #region FOV
        public bool HasFieldOfView => _fieldOfViewModule != null;
        public IFieldOfView3D FieldOfView3D => _fieldOfViewModule.Fov3D;


        private IFieldOfViewItemModule _fieldOfViewModule;
        [SerializeField] private DrawFoVLines myDrawFieldOfView;
        [SerializeField] private FieldOfView3D my3dFieldOfView;
        #endregion

        #region CustomerTrackingData
        private Dictionary<Guid, BaseCustomer> _mTrackedCustomers;
        private BaseCustomer _currentCustomerTarget;
        #endregion

        private IGuardRouteModule _mInspectionModule;
        public IGuardRouteModule GetInspectionModule => _mInspectionModule; 
        
        public IGuardBaseData BaseData => (IGuardBaseData)MyStats;

        
        #region WeaponManagementApi
        public bool HasWeapon => _currentWeaponItem != null;
        public void ApplyWeapon(GameObject itemObject, IItemObject appliedWeapon)
        {
            if (appliedWeapon == null || itemObject == null)
            {
                return;
            }

            if (_currentWeaponItem != null)
            {
                DestroyWeapon();
            }
            _currentWeaponItem = appliedWeapon;
            _currentWeaponObject = itemObject;
            myParticleSystem.Play();
        }
        public void DestroyWeapon()
        {
            _currentWeaponItem = null;
            Destroy(_currentWeaponObject);
            _currentWeaponObject = null;
        }

        public IShopInspectionPosition CurrentInspectionPosition =>
            _mInspectionModule.GetCurrentPosition;


        #endregion

        #region Initialization
        private bool _mIsInitialized;
        public bool IsInitialized => _mIsInitialized;
        protected override void Awake()
        {
            base.Awake();
            InitiateGuardStateMachines();
        }

        private void InitiateGuardStateMachines()
        {
            _mAttitudeStateMachine.AddState(new GuardIdleState(this));
            _mAttitudeStateMachine.AddState(new GuardAutoInspectionState(this));
        }

        public void Initialize(IItemObject itemObjectData)
        {
            if (IsInitialized)
            {
                return;
            }
            try
            {
                _myGuardData = itemObjectData;
                MyStats = itemObjectData.ItemStats;
                MPositionsManager = FindFirstObjectByType<ShopPositionsManager>();
                PrepareFieldOfView();
                SetupInspectionModule();
                var guardStats = (IGuardBaseData)MyStats;
                MCharacterSpeedLevel = guardStats.Speed;
                UpdateCharacterSpeed(MCharacterSpeedLevel);
                _mIsInitialized = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void PrepareFieldOfView()
        {
            if (_fieldOfViewModule != null)
            {
                return;
            }
            _fieldOfViewModule = Factory.CreateFieldOfViewItemModule(myDrawFieldOfView, my3dFieldOfView); 
            _fieldOfViewModule.Fov3D.SetupCharacterFoV(BaseData.FoVRadius);
        }
        
        private void SetupInspectionModule()
        {
            _mInspectionModule = Factory.CreateGuardSystemModule();
            var getInspectionPositions = PositionsManager.GetClosestPosition(transform.position);
            _mInspectionModule.Initialize(getInspectionPositions);
        }
        #endregion
        
        public void StartBehaviorTree()
        {
            EvaluateStatsForInspection();
        }

        public void WalkingDestinationReached()
        {
            throw new NotImplementedException();
        }

        public IShopPositionsManager PositionsManager => MPositionsManager;


        private async void EvaluateStatsForInspection()
        {
            ChangeMovementState<IdleMovementState>();
            ChangeAttitudeState<IdleAttitudeState>();
            
            //TODO: Make formula for gradient progress
            var waitResultTime = BaseAwakeTime / BaseData.Proactive;
            await Task.Delay(waitResultTime);

            var willWalkAround = RandomChanceDice(BaseData.Proactive);
            if (willWalkAround)
            {
                Debug.Log($"[EvaluateStatsForInspecting] Guard {gameObject.name} will start inspecting areas");
                StartInspecting();
                return;
            }
            Debug.Log($"[EvaluateStatsForInspecting] Guard {gameObject.name} is lazy, don't feel like working");
            _mCurrentRoutine = StartCoroutine(WaitLazyness());
        }

        private IEnumerator WaitLazyness()
        {
            Debug.Log("[BaseGuardGameObject.WaitLazyness] Guard is lazy. Waiting to evaluate again.");
            var waitResultTime = BaseAwakeTime / BaseData.Proactive;
            yield return new WaitForSeconds(waitResultTime);
            if(_mMovementStateMachine.CurrentState is IdleMovementState && _mAttitudeStateMachine.CurrentState is IdleAttitudeState)
            {
                EvaluateStatsForInspection();
            }
        }

        private void StartInspecting()
        {
            _mMovementStateMachine.ChangeState<WalkingState>();
            _mAttitudeStateMachine.ChangeState<GuardAutoInspectionState>();
        }

        #region TargetTrackingProcess
        private void UpdateTargetsInViewData()
        {
            var seenObjects = _fieldOfViewModule.Fov3D.SeenTargetObjects;
            foreach (var seenObject in seenObjects)
            {
                var isCustomer = seenObject.TryGetComponent<BaseCustomer>(out var customerStatus);
                if (!isCustomer)
                {
                    continue;
                }
                if(!_mTrackedCustomers.ContainsKey(customerStatus.CustomerId))
                {
                    _mTrackedCustomers.Add(customerStatus.CustomerId, customerStatus);
                }
            }

            var removedCustomers = new List<Guid>();
            foreach (var trackedCustomer in _mTrackedCustomers)
            {
                var customersSeen = seenObjects.Where(x=> x.TryGetComponent<BaseCustomer>(out _));
                if (customersSeen.All(x => x.GetComponent<BaseCustomer>().CustomerId != trackedCustomer.Key))
                {
                    continue;
                }
                removedCustomers.Add(trackedCustomer.Key);
            }

            foreach (var removedCustomer in removedCustomers)
            {
                if (!_mTrackedCustomers.ContainsKey(removedCustomer))
                {
                    Debug.LogError("Tracked Customer must be tracked before being removed of tracking list");
                    continue;
                }
                _mTrackedCustomers.Remove(removedCustomer);
            }
            removedCustomers.Clear();
        }
        
        protected override void ProcessInViewTargets()
        {
            if (!_fieldOfViewModule.Fov3D.HasTargetsInRange /*|| !_mGuardStatusModule.IsGuardInspecting*/)
            {
                return;
            }
            UpdateTargetsInViewData();
            ProcessCustomersSeen();
        }
        private void ProcessCustomersSeen()
        {
            foreach (var mTrackedCustomer in _mTrackedCustomers)
            {
                if (!mTrackedCustomer.Value.IsCustomerStealing)
                {
                    continue;
                }
                StartClientBustedProcess();
                break;
            }
        }

        /// <summary>
        /// TODO: Steps for client busting:
        /// 1. Surprise Time (Dependent of intelligence and agility)
        /// 2. Check Weapons & Items Available
        /// 3. MakeDecision
        /// </summary>
        private void StartClientBustedProcess()
        {
            
        }
        #endregion
        private void Update()
        {
            _mAttitudeStateMachine.Update();
            _mMovementStateMachine.Update();
            
            /*
            if(_mGuardStatusModule.CurrentAttitude != GuardSpecialAttitudeStatus.ManualInspecting)
            {
                Debug.Log($"[BaseGuardGameObject.Update] Guard Named {gameObject.name} simple movement");
                ManageMovementStatus();
            }
            else
            {
                Debug.Log($"[Update: Guard Named {gameObject.name} manual movement]");
                ManageManualMovementStatus();
            }
            Debug.Log($"[BaseGuardGameObject] is in nav mesh: {MyNavMeshAgent.isOnNavMesh}");
            */
        }
        private void ManageMovementStatus()
        {
            switch (MCharacterMovementStatus)
            {
                case BaseCharacterMovementStatus.Idle:
                    break;
                case BaseCharacterMovementStatus.Walking:
                    EvaluateWalkingDestination();
                    break;
                case BaseCharacterMovementStatus.Running:
                    EvaluateWalkingDestination();
                    break;
            }
        }

        private void ManageManualMovementStatus()
        {
            EvaluateManualInspectionDestination();
        }

        #region AttitudeStateManagement
        protected override void ReachWalkingDestination()
        {
            /*
            switch (_mGuardStatusModule.CurrentAttitude)
            {
                case GuardSpecialAttitudeStatus.ManualInspecting:
                    ReachManuallyInspectedZone();
                    break;
                case GuardSpecialAttitudeStatus.Inspecting:
                    ReachInspectedZone();
                    break;
                case GuardSpecialAttitudeStatus.Chasing:
                    AttemptDetention();
                    break;
                case GuardSpecialAttitudeStatus.Following:
                    break;
                case GuardSpecialAttitudeStatus.Fighting:
                    break;
            }*/
        }

        public void SetGuardDestination(Vector3 targetPosition)
        {
            SetMovementDestination(targetPosition);
        }

        public void IdleInspection()
        {
            MyNavMeshAgent.isStopped = true;
            MyNavMeshAgent.ResetPath();
        }

        protected void EvaluateManualInspectionDestination()
        {
            if (MyNavMeshAgent.destination.Equals(default(Vector3)))
            {
                Debug.LogWarning("Destination to walk to must be different than default");
                return;
            }

            /*if (Math.Abs(MyNavMeshAgent.destination.x - CurrentManualInspectionPosition.x) > .01f || Math.Abs(MyNavMeshAgent.destination.z - CurrentManualInspectionPosition.z) > .01f)
            {
                Debug.LogWarning("[EvaluateManualInspectionDestination] Destination must be the Current Manual Target");
                MyNavMeshAgent.SetDestination(CurrentManualInspectionPosition);
            }*/

            if (MyNavMeshAgent.remainingDistance < .2f && !MyNavMeshAgent.isStopped)
            {
                MyNavMeshAgent.isStopped = true;
                OnWalkingDestinationReached();
            }
        }

        private void AttemptDetention ()
        {
    
        }
        #endregion

        #region ClickInteractiveManagementApi
        public string GetSnippetText { get; }
        public bool HasSnippet { get; }
        public void DisplaySnippet()
        {
            throw new NotImplementedException();
        }

        public void ReceiveActionClickedEvent(RaycastHit hitInfo)
        {
            if (!_mIsClicked)
            {
                return;
            }
            Debug.Log($"[BaseGuardObject.ReceiveActionClickedEvent] Clicked object named{hitInfo.collider.name}");
            if (hitInfo.collider.gameObject.layer == 3)
            {
                HitPointDebugData(hitInfo);
                var inspectionPosition = new Vector3(hitInfo.point.x, transform.position.y, hitInfo.point.z);
                CurrentManualInspectionPosition = inspectionPosition;
                ChangeMovementState<WalkingState>();
            }
            _fieldOfViewModule.ToggleInGameFoV(!my3dFieldOfView.IsDrawActive);
            _mIsClicked = false;
        }

        private void HitPointDebugData(RaycastHit hitPoint)
        {
            Debug.Log($"[HitPointDebugData] Hit point position = {hitPoint.point}");
            var position = transform.position;
            Debug.Log($"[HitPointDebugData] Guard Position = {position}");
            var distance = Vector3.Distance(hitPoint.point, position);
            Debug.Log($"[HitPointDebugData] Distance between guard and point: {distance}");
        }
        public void ReceiveDeselectObjectEvent()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 1. Turn on visual feedback
        /// 2. Prepare for selecting destination
        /// </summary>
        public void ReceiveClickEvent()
        {
            if(WeaponPlacementManager.Instance.IsPlacingObject)
            {
                return;
            }
            if (_mInPlacement)
            {
                return;
            }
            _mIsClicked = true;
            Debug.Log($"[BaseGuardObject.ReceiveFirstClickEvent] Clicked object named{gameObject.name}");
            _fieldOfViewModule.ToggleInGameFoV(!my3dFieldOfView.IsDrawActive);
            
        }
        #endregion

        #region Private Utils
        private bool RandomChanceDice(int valueUsed)
        {
            Random.InitState(DateTime.Now.Millisecond);
            var randomNumber = Random.Range(1, 10);
            return randomNumber <= valueUsed;
        }
        #endregion
    }
}