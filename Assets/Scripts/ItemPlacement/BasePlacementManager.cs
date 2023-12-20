using UnityEngine;
using UnityEngine.SceneManagement;

namespace ItemPlacement
{
    public interface IBasePlacementManager
    {
        public void AttachNewObject(GameObject newObject);
        public void ToggleRoofObject(bool isActive);
        public void ReleaseDraggedItem();
    }
    public abstract class BasePlacementManager : MonoBehaviour, IBasePlacementManager
    {
        protected GameObject CurrentPlacedObject;
        protected GameObject LastInstantiatedGameObject;
        
        [SerializeField] protected LayerMask targetLayerMask;
        [SerializeField] protected LayerMask blockLayerMasks;
        [SerializeField] protected float deltaY;
        [SerializeField] protected GameObject roofLayerObject;
        
        protected bool IsPlaceSuccess = false;
        protected int TouchID;
        protected bool IsDragging = false;
        protected bool IsMouseReleased = false;
        protected bool IsInsideAllowedZone = false;

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

            if (IsDraggingObject())
            {
                Debug.Log("[Update] User clicked or is inside zone");
                IsDragging = true;
            }
            else
            {
                IsDragging = false;
            }
            
            if (IsDragging)
            {
                MoveObjectPreview();
            }
            if (IsDragging && IsMouseReleased)
            {
                CheckIfDragReleaseIsAllowed();
            }

            if (MouseTouchesExpectedLayerAndCurrentObjectIsActive())
            {
                IsInsideAllowedZone = true;
                if (!CurrentPlacedObject.activeInHierarchy)
                {
                    CurrentPlacedObject.SetActive(IsInsideAllowedZone);
                }
                MoveObjectPreview();
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
            
            if (Input.GetMouseButton(0) && CurrentPlacedObject != null && IsInsideAllowedZone)
            {
                CreateObjectInPlace();
                ResetSelectedObject();
            }
        }

        protected void MoveObjectPreview()
        {
            Vector3 point;
            Vector3 mousePosition;

#if !UNITY_EDITOR&&(UNITY_ANDROID||UNITY_IOS)
        Touch touch = Input.GetTouch (touchID);
        screenPosition = new Vector3 (touch.position.x, touch.position.y, 0);
#else
            mousePosition = Input.mousePosition;
#endif
            point = GetPlacementPoint(mousePosition);
            CurrentPlacedObject.transform.position = new Vector3(point.x, point.y, point.z);
        }

        protected void RotateObjectPreview()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                CurrentPlacedObject.transform.Rotate(Vector3.up, 90);
            }
        }

        protected virtual Vector3 GetPlacementPoint(Vector3 mouseScreenPosition)
        {
            return default;
        }

        protected bool IsDraggingObject()
        {
#if !UNITY_EDITOR&&(UNITY_ANDROID||UNITY_IOS)
        if (Input.touches.Length > 0) {
            if (!isTouchInput) {
                isTouchInput = true;
                touchID = Input.touches[0].fingerId;
                return true;
            } else if (Input.GetTouch (touchID).phase == TouchPhase.Ended) {
                isTouchInput = false;
                return false;
            } else {
                return true;
            }
        }
        return false;
#else
            var mouseInput = Input.GetMouseButton(0);
            return mouseInput;
#endif
        }
        public void AttachNewObject(GameObject newObject)
        {
            if (CurrentPlacedObject)
            {
                CurrentPlacedObject.SetActive(false);
            }

            CurrentPlacedObject = newObject;
            deltaY = CurrentPlacedObject.transform.localScale.y;
            Debug.Log($"[AttachNewObject] New 'Current Placed Object = {CurrentPlacedObject.name}");
        }

        public void ToggleRoofObject(bool isActive)
        {
            if (roofLayerObject == null)
            {
                Debug.LogError("[ToggleRoofObject] Roof object must not be null");
                return;
            }
            roofLayerObject.SetActive(isActive);
        }

        public void ReleaseDraggedItem()
        {
            IsMouseReleased = true;
        }
        
        protected void ResetSelectedObject()
        {
            IsDragging = false;
            CurrentPlacedObject.SetActive(false);
            CurrentPlacedObject = null;
        }
        protected virtual void CreateObjectInPlace()
        {
            GameObject obj = Instantiate(CurrentPlacedObject);
            SceneManager.MoveGameObjectToScene(obj, SceneManager.GetSceneByName("Level_One"));      
            LastInstantiatedGameObject = obj;
            obj.transform.position = CurrentPlacedObject.transform.position;
            obj.transform.localEulerAngles = CurrentPlacedObject.transform.localEulerAngles;
            obj.transform.localScale *= scaleFactor;
        }
        
        protected bool MouseTouchesExpectedLayerAndCurrentObjectIsActive()
        {
            if (!CurrentPlacedObject)
            {
                Debug.Log("[ObjectSelectedInsideZone] No item selected");
                return false;
            }
            var newPoint = Input.mousePosition;
            if (Camera.main != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(newPoint);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo, 1000, targetLayerMask) && !Physics.Raycast(ray, 1000, blockLayerMasks))
                {
                    Debug.Log("[ObjectSelectedInsideZone] Mouse is inside map");
                    return true;
                }
                else if(Physics.Raycast(ray, out hitInfo, 1000))
                {
                    Debug.Log($"[ObjectSelectedInsideZone] Hit something else. {hitInfo.collider.name}");
                }
            }

            return false;
        }
        
        protected void CheckIfDragReleaseIsAllowed()
        {
            Debug.Log("[CheckIfPlaceSuccess] Checking if place meets conditions");
            if (IsPlaceSuccess)
            {
                CreateObjectInPlace();
            }
            ResetSelectedObject();
        }
    }
}