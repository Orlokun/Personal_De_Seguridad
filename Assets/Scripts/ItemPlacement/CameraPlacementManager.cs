using System.Collections.Generic;
using UnityEngine;

namespace ItemPlacement
{
    public class CameraPlacementManager : BasePlacementManager
    {
        private static CameraPlacementManager _instance;
        public static CameraPlacementManager Instance {
            get { return _instance; }
        }

        [SerializeField] private Transform cameraObjectsParent;
        private List<Vector3> m_cameraPositions = new List<Vector3>();
        [SerializeField] private float zDistance;
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
                m_cameraPositions.Add(cameraObject.position);
            }
        }
        
        protected new void Update()
        {
            base.Update();
        }
        
        protected override Vector3 GetPlacementPoint(Vector3 mouseScreenPosition)
        {
            Ray ray = MainCamera.ScreenPointToRay(mouseScreenPosition);
            RaycastHit hitInfo;
            
            var newPoint = new Vector3();
            
            if (Physics.Raycast(ray, out hitInfo, 1000, targetLayerMask))
            {
                newPoint = hitInfo.point;
                var closestCameraPoint = GetClosestCameraPoint(newPoint);
                var newPointDistanceToClosestCamera = Vector3.Distance(newPoint, closestCameraPoint);
                if (newPointDistanceToClosestCamera <= 5)
                {
                    newPoint = closestCameraPoint;
                }
            }
            else
            {
                newPoint = ray.GetPoint(zDistance);
            }
            Debug.Log($"Current Position of selected item: {newPoint}");
            return newPoint;
        }

        private Vector3 GetClosestCameraPoint(Vector3 point)
        {
            var closestCameraDistance = 100f;
            var closestCameraPosition = point;
            for (var i = 0;i<m_cameraPositions.Count;i++)
            {
                if (Vector3.Distance(point, m_cameraPositions[i]) > 5)
                {
                    continue;
                }
                var distance = Vector3.Distance(point, m_cameraPositions[i]);
                if (distance<closestCameraDistance)
                {
                    closestCameraDistance = distance;
                    closestCameraPosition = m_cameraPositions[i];
                }
            }
            return closestCameraPosition;
        }
    }
}