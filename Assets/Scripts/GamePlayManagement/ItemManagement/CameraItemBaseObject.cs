using System;
using DataUnits.GameCatalogues;
using DataUnits.ItemScriptableObjects;
using ExternalAssets._3DFOV.Scripts;
using GameDirection.GeneralLevelManager.ShopPositions;
using GamePlayManagement.ItemPlacement;
using GamePlayManagement.ItemPlacement.FoV;
using Unity.Cinemachine;
using UnityEngine;
using Utils;

namespace GamePlayManagement.ItemManagement
{
    public class CameraItemBaseObject : BaseItemGameObject, ICameraItemBaseObject
    {
        private CinemachineCamera myVc;
        public CinemachineCamera VirtualCamera => myVc;

        
        private IFieldOfViewItemModule _fieldOfViewModule;
        [SerializeField]private DrawFoVLines _myDrawFieldOfView;
        [SerializeField]private FieldOfView3D _my3dFieldOfView;
        protected IShopPositionsManager PositionsManager;

        #region CameraStats
        protected IItemTypeStats MyStats;
        private ICameraBaseData BaseData => (ICameraBaseData) MyStats;
        private IItemObject _myCameraData;
        public IItemObject CameraBaseData => _myCameraData;

        #endregion

        #region InteractiveClickMembers

        private bool _mIsClicked;


        #endregion
        public bool HasFieldOfView => _fieldOfViewModule != null;
        public IFieldOfView3D FieldOfView3D => _fieldOfViewModule.Fov3D;
    
        private void Awake()
        {
            InPlacement = false;
        }

        public bool IsInitialized => _mIsInitialized;
        private bool _mIsInitialized;

        public void Initialize(IItemObject itemObjectData)
        {
            if (IsInitialized)
            {
                return;
            }
            try
            {
                _myCameraData = itemObjectData;
                MyStats = ItemsDataController.Instance.GetStatsForCamera(_myCameraData.ItemSupplier, _myCameraData.BitId);
                PositionsManager = FindObjectOfType<ShopPositionsManager>();
                PrepareFieldOfView();
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
            _myDrawFieldOfView = GetComponent<DrawFoVLines>();
            _my3dFieldOfView = GetComponent<FieldOfView3D>();
            _fieldOfViewModule = Factory.CreateFieldOfViewItemModule(_myDrawFieldOfView, _my3dFieldOfView); 
            _fieldOfViewModule.Fov3D.SetupCharacterFoV(BaseData.FoVRadius);
        }
        #region Interactive Object Interface
        public override void ReceiveClickEvent()
        {
            Debug.Log($"[CameraItemPrefab.ReceiveFirstClickEvent] Clicked camera named{gameObject.name}");
            if (InPlacement)
            {
                return;
            }
            _fieldOfViewModule.ToggleInGameFoV(!_my3dFieldOfView.IsDrawActive);
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
            }
            _fieldOfViewModule.ToggleInGameFoV(!_my3dFieldOfView.IsDrawActive);
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
            
        }
        
        public override void InitializeItem(IItemObject itemData)
        {
            base.InitializeItem(itemData);
            Initialize(itemData);
        }
        
        #endregion
    }        
}