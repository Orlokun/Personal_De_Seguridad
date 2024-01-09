using DataUnits.ItemScriptableObjects;
using ExternalAssets._3DFOV.Scripts;
using GameDirection;
using GamePlayManagement.BitDescriptions;
using Players_NPC.NPC_Management.Customer_Management;
using Players_NPC.NPC_Management.Customer_Management.CustomerInterfaces;
using UI;
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

        protected override void MoveObjectPreview()
        {
            IBasePlacementPosition guardPosition;
            base.MoveObjectPreview();
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
        protected void ConfirmCamera()
        {
            if (MainCamera == null)
            {
                MainCamera = Camera.main;
            }
        }
        
        protected override void AttachObjectProcess(IItemObject itemData, GameObject newObject)
        {
            base.AttachObjectProcess(itemData, newObject);
            var itemObject = (IGuardItemObject)CurrentPlacedObject.GetComponent<GuardItemObject>();
            var fov = itemObject.FieldOfView3D;
            fov.ToggleInGameFoV(true);
            GameDirector.Instance.GetUIController.DeactivateObject(CanvasBitId.GamePlayCanvas, GameplayPanelsBitStates.ITEM_DETAILED_SIDEBAR);
        }

    }
}