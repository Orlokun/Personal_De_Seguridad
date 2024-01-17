using System;
using System.Collections.Generic;
using DataUnits.ItemScriptableObjects;
using ExternalAssets._3DFOV.Scripts;
using UnityEngine;
using Utils;

namespace GamePlayManagement.ItemPlacement
{
    public class CameraPlacementManager : BasePlacementManager
    {
        private static CameraPlacementManager _instance;
        public static IBasePlacementManager Instance {
            get { return _instance; }
        }
        
        #region Camera Positions In Scene
        [SerializeField] private Transform cameraObjectsParent;
        private Dictionary<Guid, ICameraPlacementPosition> m_cameraPositions = new Dictionary<Guid, ICameraPlacementPosition>();
        [SerializeField] private float zDistance;

        private ICameraPlacementPosition _currentPlacementPosition;
        #endregion
        
        protected override void Awake()
        {
            _instance = this;
            base.Awake();
            CollectInitialCameraPositions();
        }

        private void CollectInitialCameraPositions()
        {
            if (m_cameraPositions.Count != 0)
            {
                Debug.LogWarning("Camera Positions are already set");
                return;
            }

            for (var i= 0; i<_instance.cameraObjectsParent.childCount;i++)
            {
                var cameraObject = cameraObjectsParent.GetChild(i);
                var cameraPositionData = Factory.CreateCameraPlacementPosition(Guid.NewGuid(), cameraObject.position, cameraObject.gameObject.name);
                m_cameraPositions.Add(cameraPositionData.Id, cameraPositionData);
            }
        }
        
        protected new void Update()
        {
            base.Update();
        }

        protected override void MoveObjectPreview()
        {
            ICameraPlacementPosition cameraPosition;
            base.MoveObjectPreview();
            cameraPosition = (ICameraPlacementPosition)GetPlacementPoint(MousePosition);
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
            var fov = (IFieldOfView3D)CurrentPlacedObject.GetComponent<FieldOfView3D>();
            fov.ToggleInGameFoV(true);
        }

        private ICameraPlacementPosition GetClosestCameraPoint(Vector3 point)
        {
            var closestCameraDistance = 100f;
            ICameraPlacementPosition closestCameraPosition;
            foreach (var cameraPosition in m_cameraPositions)
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
}