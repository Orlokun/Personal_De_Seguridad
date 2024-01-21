using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using DataUnits.ItemScriptableObjects;
using ExternalAssets._3DFOV.Scripts;
using GamePlayManagement.ItemPlacement.PlacementManagers;
using GamePlayManagement.Players_NPC;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.CustomerInterfaces;
using UnityEngine;
using Utils;

namespace GamePlayManagement.ItemManagement.Guards
{
    public class BaseGuardGameObject : BaseCharacterInScene, IInteractiveClickableObject, IBaseGuardGameObject
    {
        #region Members
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


        #endregion

        #region StateManagement
        private IGuardStatusModule _mGuardStatusModule;
        private IGuardStats _myStats;
        public IGuardStats Stats => _myStats;
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
        #endregion
        
        protected override void Awake()
        {
            base.Awake();
            _mInPlacement = false;
            PrepareFieldOfView();
        }

        protected override void Start()
        {
            base.Start();
            StartInspecting();
        }

        private void StartInspecting()
        {
            _mGuardStatusModule.SetGuardAttitudeStatus(GuardSpecialAttitudeStatus.Inspecting);
            EvaluateStatsForInspection();
        }

        private void EvaluateStatsForInspection()
        {
            
        }

        private void PrepareFieldOfView()
        {
            _fieldOfViewModule = Factory.CreateFieldOfViewItemModule(myDrawFieldOfView, my3dFieldOfView); 
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
                var customersSeen = seenObjects.Where(x=> x.TryGetComponent<IBaseCustomer>(out var customerData));
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
        private void ReachInspectedZone()
        {
    
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
            Debug.Log($"[CameraItemPrefab.SendClickObject] Clicked object named{gameObject.name}");
            if (_mInPlacement)
            {
                return;
            }
            _fieldOfViewModule.ToggleInGameFoV(!my3dFieldOfView.IsDrawActive);
        }
        #endregion
    }
}