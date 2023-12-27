using UnityEngine;
using Utils;

namespace ItemPlacement
{
    public class FloorPlacementManager : BasePlacementManager
    {
        private static FloorPlacementManager _instance;
        public static IBasePlacementManager Instance
        {
            get { return _instance; }
        }
        
        //The length of the z axis of the object from the camera
        [SerializeField] private float zDistance = 50f;
        private float lastMouseXPos;
        protected override void Awake()
        {
            _instance = this;
            base.Awake();
        }

        new void Update()   
        {
            base.Update();
            if (!Input.GetKeyDown(KeyCode.R) || CurrentPlacedObject == null)
            {
                return;
            }
            CurrentPlacedObject.transform.Rotate(0,45,0);
        }

        protected override IBasePlacementPosition GetPlacementPoint(Vector3 mouseScreenPosition)
        {
            ConfirmCamera();
            var ray = MainCamera.ScreenPointToRay(mouseScreenPosition);
            RaycastHit hitInfo;
            IBasePlacementPosition newPoint;
            if (Physics.Raycast(ray, out hitInfo, 500, targetLayerMask))
            {
                Debug.Log($"[FloorPlacementManager.GetPlacementPoint] HIT FLOOR: {hitInfo.collider.gameObject.name}");
                var hPoint = hitInfo.point + new Vector3(0, deltaY, 0);
                var placementPosition = Factory.CreateFloorPlacementPosition(hPoint);
                newPoint = placementPosition;
                IsPlaceSuccess = true;
            }
            else
            {
                Debug.Log($"[FloorPlacementManager.GetPlacementPoint] DIDN'T HIT FLOOR.");
                var placementPosition = Factory.CreateFloorPlacementPosition(ray.GetPoint(zDistance));
                newPoint = placementPosition;
                IsPlaceSuccess = false;
            }
            return newPoint;
        }

        protected void ConfirmCamera()
        {
            if (MainCamera == null)
            {
                MainCamera = Camera.main;
            }
        }
    }
}