using UnityEngine;

namespace ItemPlacement
{
    public abstract class BasePlacementManager : MonoBehaviour
    {
        protected GameObject CurrentPlacedObject;
        protected GameObject _lastInstantiatedGameObject;
        
        [SerializeField] protected LayerMask targetLayerMask;
        [SerializeField] protected LayerMask blockLayerMasks;
        [SerializeField] protected float deltaY;

        protected bool IsPlaceSuccess = false;
        protected int TouchID;
        protected bool IsDragging = false;
        protected bool IsMouseReleased = false;
        protected bool IsInsideAllowedZone = false;

        protected UnityEngine.Camera MainCamera;
        
        //The scaling factor of the object
        [SerializeField] private float scaleFactor = 1.2f;

        protected virtual void Awake()
        {
            MainCamera = UnityEngine.Camera.main;
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
            Vector3 screenPosition;

#if !UNITY_EDITOR&&(UNITY_ANDROID||UNITY_IOS)
        Touch touch = Input.GetTouch (touchID);
        screenPosition = new Vector3 (touch.position.x, touch.position.y, 0);
#else
            screenPosition = Input.mousePosition;
#endif
            
            point = GetPlacementPoint(screenPosition);
            CurrentPlacedObject.transform.position = new Vector3(point.x, point.y, point.z);
            CurrentPlacedObject.transform.localEulerAngles = new Vector3(0, 60, 0);
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
        protected void CreateObjectInPlace()
        {
            GameObject obj = Instantiate(CurrentPlacedObject);
            _lastInstantiatedGameObject = obj;
            obj.transform.position = CurrentPlacedObject.transform.position;
            obj.transform.localEulerAngles = CurrentPlacedObject.transform.localEulerAngles;
            obj.transform.localScale *= scaleFactor;
            //Change the Layer of this object to Drag for subsequent drag detection
            //obj.layer = LayerMask.NameToLayer("Drag");
        }
        
        protected bool MouseTouchesExpectedLayerAndCurrentObjectIsActive()
        {
            if (!CurrentPlacedObject)
            {
                Debug.Log("[ObjectSelectedInsideZone] No item selected");
                return false;
            }

            Debug.Log("[ObjectSelectedInsideZone] Item Selected Correctly");
            var newPoint = Input.mousePosition;
            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(newPoint);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 1000, targetLayerMask) && !Physics.Raycast(ray, 1000, blockLayerMasks))
            {
                Debug.Log("[ObjectSelectedInsideZone] Mouse is inside map");
                return true;
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