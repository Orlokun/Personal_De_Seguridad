using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cinemachine;
using DataUnits.GameCatalogues;
using DataUnits.ItemScriptableObjects;
using ExternalAssets._3DFOV.Scripts;
using GameDirection.GeneralLevelManager.ShopPositions;
using GamePlayManagement.ItemPlacement.PlacementManagers;
using GamePlayManagement.Players_NPC;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.CustomerInterfaces;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace GamePlayManagement.ItemManagement.Guards
{
    public class BaseGuardGameObject : BaseCharacterInScene, IInteractiveClickableObject, IBaseGuardGameObject, IInitializeWithArg1<IItemObject>
    {
        #region Members
        private IItemObject _myGuardData;
        public IItemObject GuardBaseData => _myGuardData;

        #region PlacementManagement
        public void SetInPlacementStatus(bool inPlacement)
        {
            _mInPlacement = inPlacement;
        }
        private bool _mInPlacement;
        #endregion

        #region WeaponManagementMembers
        [SerializeField] private ParticleSystem myParticleSystem;
        public Transform GunParentTransform => mGunPositionTransform;
        [SerializeField] private Transform mGunPositionTransform;
        private IWeaponStats CurrentWeaponStats => (IWeaponStats)_currentWeaponItem?.ItemStats;
        private IItemObject _currentWeaponItem;
        private GameObject _currentWeaponObject;
        #endregion
        
        /// <summary>
        /// Not Used yet
        /// </summary>
        #region CameraPerspective
        private CinemachineVirtualCamera _myVc;
        public CinemachineVirtualCamera VirtualCamera => _myVc;
        #endregion
        
        #region FOV
        public bool HasFieldOfView => _fieldOfViewModule != null;
        public IFieldOfView3D FieldOfView3D => _fieldOfViewModule.Fov3D;


        private IFieldOfViewItemModule _fieldOfViewModule;
        [SerializeField] private DrawFoVLines myDrawFieldOfView;
        [SerializeField] private FieldOfView3D my3dFieldOfView;
        #endregion

        #region CustomerTrackingData
        private Dictionary<Guid, IBaseCustomer> _mTrackedCustomers;
        private IBaseCustomer _currentCustomerTarget;
        #endregion

        #region LevelInspectionData
        private IGuardRouteSystemModule _mInspectionSystemModule;

        #endregion

        #region StateManagement
        private IGuardStatusModule _mGuardStatusModule;
        #endregion

        #region GuardStats
        public IGuardStats Stats => (IGuardStats)MyStats;
        #endregion

        #endregion

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
            _mInspectionSystemModule.GetCurrentPosition;

        protected override float GetStatusSpeed(BaseCharacterMovementStatus currentStatus)
        {
            base.GetStatusSpeed(currentStatus);
            var guardSpeed = (float)Stats.Speed / 10;
            switch (currentStatus)
            {
                case BaseCharacterMovementStatus.Walking:
                    // ReSharper disable once PossibleLossOfFraction
                    return BaseWalkSpeed * guardSpeed;
                case BaseCharacterMovementStatus.Running:
                    // ReSharper disable once PossibleLossOfFraction
                    return BaseRunSpeed * guardSpeed;
                default:
                    return 1;
            }
        }
        #endregion

        private bool _mIsInitialized;
        public bool IsInitialized => _mIsInitialized;

        private void SetupInspectionModule()
        {
            _mInspectionSystemModule = Factory.CreateGuardSystemModule();
            var getInspectionPositions = PositionsManager.GetClosestPosition(transform.position);
            _mInspectionSystemModule.Initialize(getInspectionPositions);
        }
        public void StartBehaviorTree()
        {
            EvaluateStatsForInspection();
        }
        
        public void Initialize(IItemObject itemObjectData)
        {
            if (IsInitialized)
            {
                return;
            }
            try
            {
                _mGuardStatusModule = Factory.CreateGuardStatusModule(this);
                _myGuardData = itemObjectData;
                MyStats = ItemsDataController.Instance.GetStatsForGuard(_myGuardData.ItemSupplier, _myGuardData.BitId);
                PositionsManager = FindObjectOfType<ShopPositionsManager>();
                PrepareFieldOfView();
                SetupInspectionModule();
                _mIsInitialized = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        private async void EvaluateStatsForInspection()
        {
            var waitResultTime = BaseAwakeTime / Stats.Proactive;
            await Task.Delay(waitResultTime);
            
            var willWalkAround = RandomChanceDice(Stats.Proactive);
            if (willWalkAround)
            {
                Debug.Log($"[EvaluateStatsForInspecting] Guard {gameObject.name} will start inspecting areas");
                StartInspecting();
                return;
            }
            Debug.Log($"[EvaluateStatsForInspecting] Guard {gameObject.name} is lazy, don't feel like working");
            SetCharacterMovementStatus(BaseCharacterMovementStatus.Idle);
            SetCharacterAttitudeStatus(GuardSpecialAttitudeStatus.Idle);
        }
        protected override void SetCharacterAttitudeStatus(GuardSpecialAttitudeStatus newGuardStatus)
        {
            _mGuardStatusModule.SetGuardAttitudeStatus(newGuardStatus);
        }

        private void StartInspecting()
        {
            SetCharacterMovementStatus(BaseCharacterMovementStatus.Walking);
            _mGuardStatusModule.SetGuardAttitudeStatus(GuardSpecialAttitudeStatus.Inspecting);
        }
        private bool RandomChanceDice(int valueUsed)
        {
            Random.InitState(DateTime.Now.Millisecond);
            var randomNumber = Random.Range(1, 10);
            return randomNumber <= valueUsed;
        }


        public void PrepareFieldOfView()
        {
            if (_fieldOfViewModule != null)
            {
                return;
            }
            _fieldOfViewModule = Factory.CreateFieldOfViewItemModule(myDrawFieldOfView, my3dFieldOfView); 
            _fieldOfViewModule.Fov3D.SetupCharacterFoV(Stats.FoVRadius);
        }

        #region TargetTrackingProcess
        private void UpdateTargetsInViewData()
        {
            var seenObjects = _fieldOfViewModule.Fov3D.SeenTargetObjects;
            foreach (var seenObject in seenObjects)
            {
                var isCustomer = seenObject.TryGetComponent<IBaseCustomer>(out var customerStatus);
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
                var customersSeen = seenObjects.Where(x=> x.TryGetComponent<IBaseCustomer>(out _));
                if (customersSeen.All(x => x.GetComponent<IBaseCustomer>().CustomerId != trackedCustomer.Key))
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
            ManageMovementStatus();
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
        
        #region AttitudeStateManagement
        protected override void ReachWalkingDestination()
        {
            switch (_mGuardStatusModule.CurrentAttitude)
            {
                case GuardSpecialAttitudeStatus.Inspecting:
                    ReachInspectedZone();
                    break;
                case GuardSpecialAttitudeStatus.Chasing:
                    AttemptDetention();
                    break;
                case GuardSpecialAttitudeStatus.Following:
                    EvaluateStartShopping();
                    break;
                case GuardSpecialAttitudeStatus.Fighting:
                    break;
            }
        }
        private async void ReachInspectedZone()
        {
            var nextPosition = PositionsManager.GetNextPosition(CurrentInspectionPosition.Id);
            SetCharacterAttitudeStatus(GuardSpecialAttitudeStatus.Idle);
            SetCharacterMovementStatus(BaseCharacterMovementStatus.Idle);
            await Task.Delay(500);
            BaseAnimator.ChangeAnimationState(SearchAround);
            await Task.Delay(4000);
            _mInspectionSystemModule.SetNewCurrentPosition(nextPosition);
            SetCharacterAttitudeStatus(GuardSpecialAttitudeStatus.Inspecting);
            SetCharacterMovementStatus(BaseCharacterMovementStatus.Walking);
        }
        
        private void AttemptDetention ()
        {
    
        }
        private void EvaluateStartShopping()
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

        public void SendClickObject()
        {
            if(WeaponPlacementManager.Instance.IsPlacingObject)
            {
                return;
            }
            if (_mInPlacement)
            {
                return;
            }
            Debug.Log($"[CameraItemPrefab.SendClickObject] Clicked object named{gameObject.name}");
            _fieldOfViewModule.ToggleInGameFoV(!my3dFieldOfView.IsDrawActive);
        }
        #endregion


    }
}