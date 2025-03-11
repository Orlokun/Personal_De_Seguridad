using DataUnits.ItemScriptableObjects;
using GameDirection;
using GamePlayManagement.ItemManagement.Guards;
using UnityEngine;
using Utils;

namespace GamePlayManagement.ItemPlacement.PlacementManagement
{
    public class FloorPlacementManager : BasePlacementManager
    {
        private static FloorPlacementManager _instance;
        public static IBasePlacementManager Instance
        {
            get { return _instance; }
        }
        
        //The length of the z axis of the object from the camera
        private float zDistance = 50f;
        private float lastMouseXPos;
        protected override void Awake()
        {
            _instance = this;
            base.Awake();
            
            OnItemPlaced += GameDirector.Instance.GetActiveGameProfile.GetRequestsModuleManager()
                .CheckItemPlacementChallenges;
            OnItemPlaced += GameDirector.Instance.GetActiveGameProfile.GetComplianceManager
                .CheckItemPlacementCompliance;
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
            ConfirmNoRoofBlocking();
            var ray = MainCamera.ScreenPointToRay(mouseScreenPosition);
            RaycastHit hitInfo;
            IBasePlacementPosition newPoint;
            if (Physics.Raycast(ray, out hitInfo, 500, targetLayerMask))
            {
                Debug.Log($"[FloorPlacementManager.GetPlacementPoint] HIT FLOOR: {hitInfo.collider.gameObject.name}");
                var hPoint = hitInfo.point;
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

        protected override void SetCurrentMousePosition()
        {
            IBasePlacementPosition guardPosition;
            base.SetCurrentMousePosition();
            guardPosition = GetPlacementPoint(MousePosition);
            CurrentPlacedObject.transform.position = new Vector3(guardPosition.ItemPosition.x, guardPosition.ItemPosition.y, guardPosition.ItemPosition.z);
            if (!CurrentPlacedObject.activeInHierarchy)
            {
                CurrentPlacedObject.SetActive(true);
            }
        }
        protected void ConfirmNoRoofBlocking()
        {
            if (roofLayerObject.activeInHierarchy && IsAttemptingPlacement)
            {
                roofLayerObject.SetActive(false);
            }
        }
        
        protected override void AttachObjectProcess(IItemObject itemData, GameObject newObject)
        {
            base.AttachObjectProcess(itemData, newObject);
            var itemObject = (IBaseGuardGameObject)CurrentPlacedObject.GetComponent<BaseGuardGameObject>();
            itemObject.Initialize(itemData);
            var fov = itemObject.FieldOfView3D;
            fov.ToggleInGameFoV(true);
        }
    }
}