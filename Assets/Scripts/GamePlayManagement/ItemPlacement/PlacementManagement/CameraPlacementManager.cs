using System;
using System.Collections.Generic;
using DataUnits.ItemScriptableObjects;
using ExternalAssets._3DFOV.Scripts;
using GamePlayManagement.ItemManagement;
using UnityEngine;
using Utils;

namespace GamePlayManagement.ItemPlacement.PlacementManagement
{
    public class CameraPlacementManager : BasePlacementManager, ICameraPlacementManager
    {
        private static CameraPlacementManager _mInstance;
        public static IBasePlacementManager MInstance {
            get { return _mInstance; }
        }
        
        #region Camera Positions In Scene
        [SerializeField]private Transform _mCurrentCameraItemsPositionsParent;
        private Dictionary<Guid, ICameraPlacementPosition> _mCameraPositions = new Dictionary<Guid, ICameraPlacementPosition>();
        [SerializeField] private float zDistance;

        private ICameraPlacementPosition _currentPlacementPosition;
        #endregion
        
        protected override void Awake()
        {
            if(_mInstance != null && _mInstance != this)
            {
                Destroy(this);
                return;
            }
            _mInstance = this;
            base.Awake();
            UpdatePositionsForItemCameras(_mCurrentCameraItemsPositionsParent);
        }

        public void UpdatePositionsForItemCameras(Transform cameraObjectsParent)
        {
            _mCameraPositions.Clear();
            _mCurrentCameraItemsPositionsParent = cameraObjectsParent;
            CollectInitialCameraPositions();
        }
        private void CollectInitialCameraPositions()
        {
            if (_mCameraPositions.Count != 0)
            {
                Debug.LogWarning("Camera Positions are already set");
                return;
            }
            for (var i= 0; i<_mInstance._mCurrentCameraItemsPositionsParent.childCount;i++)
            {
                var cameraObject = _mCurrentCameraItemsPositionsParent.GetChild(i);
                var cameraPositionData = Factory.CreateCameraPlacementPosition(Guid.NewGuid(), cameraObject.position, cameraObject.gameObject.name);
                _mCameraPositions.Add(cameraPositionData.Id, cameraPositionData);
            }
        }
        
        protected new void Update()
        {
            base.Update();
        }

        protected override void SetCurrentMousePosition()
        {
            ICameraPlacementPosition cameraPosition;
            base.SetCurrentMousePosition();
            cameraPosition = (ICameraPlacementPosition)GetPlacementPoint(MousePosition);
            if(cameraPosition == null)
            {
                return;
            }
            var currentCameraRotationManager = (IItemCameraRotation)CurrentPlacedObject.GetComponent<ItemCameraRotation>();
            currentCameraRotationManager.SetNewPosition(cameraPosition.ItemPosition);
            CurrentPlacedObject.transform.position = new Vector3(cameraPosition.ItemPosition.x, cameraPosition.ItemPosition.y, cameraPosition.ItemPosition.z);
            if (!CurrentPlacedObject.activeInHierarchy)
            {
                CurrentPlacedObject.SetActive(true);
            }
        }


        
        protected override IBasePlacementPosition GetPlacementPoint(Vector3 mouseScreenPosition)
        {
            ConfirmCamera();
            var ray = MainCamera.ScreenPointToRay(mouseScreenPosition);
            RaycastHit hitInfo;
            
            Vector3 hitPoint;
            
            if (Physics.Raycast(ray, out hitInfo, 500, targetLayerMask))
            {
                hitPoint = hitInfo.point;
                var closestCameraPoint = GetClosestCameraPoint(hitPoint);
                _currentPlacementPosition = closestCameraPoint;
                return closestCameraPoint;
            }
            hitPoint = ray.GetPoint(zDistance);
            return null;
        }

        protected override void CreateObjectInPlace()
        {
            base.CreateObjectInPlace();
            var fov = LastInstantiatedGameObject.GetComponent<FieldOfView3D>();
            fov.ToggleInGameFoV(false);
        }

        protected override void AttachObjectProcess(IItemObject itemData, GameObject newObject)
        {
            base.AttachObjectProcess(itemData, newObject);
            var itemObject = (ICameraItemBaseObject)CurrentPlacedObject.GetComponent<CameraItemBaseObject>();
            itemObject.Initialize(itemData);
            var fov = (IFieldOfView3D)CurrentPlacedObject.GetComponent<FieldOfView3D>();
            fov.ToggleInGameFoV(true);
        }

        private ICameraPlacementPosition GetClosestCameraPoint(Vector3 point)
        {
            var closestCameraDistance = 100f;
            ICameraPlacementPosition closestCameraPosition;
            foreach (var cameraPosition in _mCameraPositions)
            {
                var positionData = cameraPosition.Value;
                if (positionData.IsOccupied)
                {
                    continue;
                }
                if (Vector3.Distance(point, positionData.ItemPosition) > 5)
                {
                    continue;
                }
                var distance = Vector3.Distance(point, positionData.ItemPosition);

                if (distance<closestCameraDistance)
                {
                    closestCameraDistance = distance;
                    closestCameraPosition = positionData;
                    return closestCameraPosition;
                }
            }
            return null;
        }
    }

    public interface ICameraPlacementManager
    {
        public void UpdatePositionsForItemCameras(Transform cameraObjectsParent);
    }
}