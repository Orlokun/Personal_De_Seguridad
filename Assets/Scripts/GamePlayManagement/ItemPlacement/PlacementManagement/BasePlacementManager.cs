using System;
using DataUnits.ItemScriptableObjects;
using GameDirection;
using GamePlayManagement.BitDescriptions;
using GamePlayManagement.ItemManagement;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GamePlayManagement.ItemPlacement.PlacementManagement
{
    public abstract class BasePlacementManager : MonoBehaviour, IBasePlacementManager
    {
        protected GameObject CurrentPlacedObject;
        protected GameObject LastInstantiatedGameObject;

        protected IItemObject CurrentItemData;
        
        [SerializeField] protected LayerMask targetLayerMask;
        [SerializeField] protected LayerMask blockLayerMasks;
        [SerializeField] protected GameObject roofLayerObject;
        
        protected bool IsPlaceSuccess = false;
        protected bool IsAttemptingPlacement;
        protected bool IsInsideAllowedZone;

        public delegate void PlacedItem(IItemObject itemObject);
        public event PlacedItem OnItemPlaced;

        
        
        protected Vector3 MousePosition;
        protected Camera MainCamera;
        
        //The scaling factor of the object
        [SerializeField] private float scaleFactor = 1.5f;

        protected virtual void Awake()
        {
            MainCamera = Camera.main;
        }
        protected void Update()
        {
            if (CurrentPlacedObject == null)
            {
                return;
            }
            if (IsAttemptingPlacement)
            {
                ProcessPlacementMovement();
            }
            ProcessPlacementActiveStatus();
        }
        
        private void ProcessPlacementMovement()
        {
            if (MouseTouchesExpectedLayer())
            {
                //Debug.Log($"{CurrentPlacedObject.name}: Object is inside Placement zone");
                IsInsideAllowedZone = true;
                if (!CurrentPlacedObject.activeInHierarchy)
                {
                    CurrentPlacedObject.SetActive(IsInsideAllowedZone);
                }
                SetCurrentMousePosition();
                RotateObjectPreview();
            }
            else
            {
                if (CurrentPlacedObject != null && IsInsideAllowedZone && CurrentPlacedObject.activeInHierarchy)
                {
                    IsInsideAllowedZone = false;
                    CurrentPlacedObject.SetActive(IsInsideAllowedZone);
                }
            }
        }
        private void ProcessPlacementActiveStatus()
        {
            if(!IsAttemptingPlacement)
            {
                ResetSelectedObject();
            }
            if (Input.GetMouseButton(0) && CurrentPlacedObject != null && IsInsideAllowedZone && IsAttemptingPlacement)
            {
                AttemptItemPlacement();
            }
            else if(Input.GetMouseButton(0) && IsAttemptingPlacement && !IsInsideAllowedZone)
            {
                ResetSelectedObject();
            }
        }
        
        private void AttemptItemPlacement()
        {
            CreateObjectInPlace();
            ResetSelectedObject();
        }

        protected virtual void SetCurrentMousePosition()
        {
            MousePosition = Input.mousePosition;
        }

        protected virtual void RotateObjectPreview()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                CurrentPlacedObject.transform.Rotate(0f, 45f, 0f, Space.World);
            }
        }
        protected abstract IBasePlacementPosition GetPlacementPoint(Vector3 mouseScreenPosition);
        protected void ConfirmCamera()
        {
            if (MainCamera == null)
            {
                MainCamera = Camera.main;
            }
        }

        public void AttachNewObject(IItemObject itemData, GameObject newObject)
        {
            AttachObjectProcess(itemData, newObject);
            IsAttemptingPlacement = true;
        }
        
        protected virtual void AttachObjectProcess(IItemObject itemData, GameObject newObject)
        {
            if (CurrentPlacedObject)
            {
                CurrentPlacedObject.SetActive(false);
            }
            CurrentPlacedObject = newObject;
            CurrentItemData = itemData;
            Debug.Log($"[AttachNewObject] New 'Current Placed Object = {CurrentPlacedObject.name}");
            ActivatePlacementStatus();
            GameDirector.Instance.GetUIController.DeactivateObject(CanvasBitId.GamePlayCanvas, GameplayPanelsBitStates.ITEM_DETAILED_SIDEBAR);
        }


        protected virtual void CreateObjectInPlace()
        {
            try
            {
                var obj = Instantiate(CurrentPlacedObject);
                if (GameDirector.Instance.GetCurrentHighLvlGameState != HighLevelGameStates.InCutScene)
                {
                    SceneManager.MoveGameObjectToScene(obj, SceneManager.GetSceneByName("Level_One"));          //TODO: Fix Hardcode
                }
                LastInstantiatedGameObject = obj;
                SetNewObjectPosition(obj);
                IsAttemptingPlacement = false;
                var objectData = (IBaseItemObject) obj.GetComponent<BaseItemGameObject>();
                objectData.SetInPlacementStatus(false);
                objectData.InitializeItem(CurrentItemData);
                OnItemPlaced?.Invoke(CurrentItemData);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        protected virtual void SetNewObjectPosition(GameObject gObject)
        {
            gObject.transform.position = CurrentPlacedObject.transform.position;
            gObject.transform.localEulerAngles = CurrentPlacedObject.transform.localEulerAngles;
            gObject.transform.localScale *= scaleFactor;
        }
        private bool MouseTouchesExpectedLayer()
        {
            var newPoint = Input.mousePosition;
            if (Camera.main != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(newPoint);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo, 500, targetLayerMask) && !Physics.Raycast(ray, 1000, blockLayerMasks))
                {
                    //Debug.Log("[ObjectSelectedInsideZone] Mouse is inside map");
                    return true;
                }
                return false;
                if(Physics.Raycast(ray, out hitInfo, 500))
                {
                    //Debug.Log($"[ObjectSelectedInsideZone] Hit something else. {hitInfo.collider.name}");
                }
            }
            return false;
        }

        #region Utils
        public void ToggleRoofObject(bool isActive)
        {
            if (roofLayerObject == null)
            {
                Debug.LogError("[ToggleRoofObject] Roof object must not be null");
                return;
            }
            roofLayerObject.SetActive(isActive);
        }

        public bool IsPlacingObject => IsAttemptingPlacement;

        private void ResetSelectedObject()
        {
            IsAttemptingPlacement = false;
            CurrentPlacedObject.SetActive(false);
        }
        private void ActivatePlacementStatus()
        {
            CurrentPlacedObject.SetActive(true);
            IsAttemptingPlacement = true;
        }
        #endregion
    }
}