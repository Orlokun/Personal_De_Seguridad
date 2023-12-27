using UnityEngine;
namespace UI
{
    public class MouseInputManager : MonoBehaviour
    {
        [SerializeField] private Texture2D interactiveObjectMouseTexture;
        [SerializeField] private Texture2D defaultMouseTexture;
        [SerializeField] private LayerMask officeObjectLayer;
        [SerializeField] private LayerMask itemObjectsLayer;

        private IOfficeInteractiveObject _mHoveredOfficeObject; 
        private IInteractiveClickableObject _mHoveredInteractiveObject; 
        
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Update()
        {
            if (Camera.main == null)
            {
                return;
            }
            ManageMouseCursor();
            ManageMouseClick();
        }

        private void ManageMouseCursor()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            ProcessOfficeInteractiveObjects(ray);
            ProcessItemInteractiveObjects(ray);
        }

        private void ProcessOfficeInteractiveObjects(Ray ray)
        {
            if (Physics.Raycast(ray, out var hitInfo, 100, officeObjectLayer))
            {
                var interactiveObject = hitInfo.collider.gameObject;
                if (!interactiveObject.TryGetComponent<IOfficeInteractiveObject>(out _mHoveredOfficeObject))
                {
                    Debug.LogError("[MouseInputManager.ManageMouseCursor] Mouse interactive object component not found");
                    return;
                }
                Cursor.SetCursor(interactiveObjectMouseTexture, Vector2.zero, CursorMode.Auto);   
            }
            else
            {            
                Cursor.SetCursor(defaultMouseTexture, Vector2.zero, CursorMode.Auto);
                _mHoveredOfficeObject = null;
            }
        }
        private void ProcessItemInteractiveObjects(Ray ray)
        {
            if (Physics.Raycast(ray, out var hitInfo, 100, itemObjectsLayer))
            {
                var interactiveObject = hitInfo.collider.gameObject;
                if (!interactiveObject.TryGetComponent<IInteractiveClickableObject>(out _mHoveredInteractiveObject))
                {
                    Debug.LogError("[MouseInputManager.ProcessItemInteractiveObjects] Mouse interactive object component not found");
                    return;
                }
                Cursor.SetCursor(interactiveObjectMouseTexture, Vector2.zero, CursorMode.Auto);   
            }
            else
            {            
                Cursor.SetCursor(defaultMouseTexture, Vector2.zero, CursorMode.Auto);
                _mHoveredInteractiveObject = null;
            }
        }
        
        private void ManageMouseClick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                ProcessOfficeClickedObject();
                ProcessInteractiveItemClicked();
            }
        }

        private void ProcessOfficeClickedObject()
        {
            if (_mHoveredOfficeObject == null)
            {
                return;
            }
            _mHoveredOfficeObject.SendClickObject();
        }
        private void ProcessInteractiveItemClicked()
        {
            if (_mHoveredInteractiveObject == null)
            {
                return;
            }
            _mHoveredInteractiveObject.SendClickObject();
        }
    }
}