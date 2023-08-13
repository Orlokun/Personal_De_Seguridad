using UnityEngine;

namespace ItemPlacement
{
    public class FloorPlacementManager : BasePlacementManager
    {
        private static FloorPlacementManager _instance;
        public static FloorPlacementManager Instance
        {
            get { return _instance; }
        }
        
        //The length of the z axis of the object from the camera
        [SerializeField] private float zDistance = 50f;

        protected override void Awake()
        {
            _instance = this;
            base.Awake();
        }

        new void Update()   
        {
            base.Update();
        }
        
        protected override Vector3 GetPlacementPoint(Vector3 mouseScreenPosition)
        {
            Ray ray = MainCamera.ScreenPointToRay(mouseScreenPosition);
            RaycastHit hitInfo;
            Vector3 newPoint;
            if (Physics.Raycast(ray, out hitInfo, 1000, targetLayerMask))
            {
                var hPoint = hitInfo.point + new Vector3(0, deltaY, 0);
                newPoint = hPoint;
                IsPlaceSuccess = true;
            }
            else
            {
                newPoint = ray.GetPoint(zDistance);
                IsPlaceSuccess = false;
            }
            return newPoint;
        }
    }
}