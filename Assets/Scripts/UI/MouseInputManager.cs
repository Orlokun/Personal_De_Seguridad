using UnityEngine;
namespace UI
{
    public class MouseInputManager : MonoBehaviour
    {
        [SerializeField] private Texture2D interactiveObjectMouseTexture;
        [SerializeField] private Texture2D defaultMouseTexture;
        [SerializeField] private LayerMask itemObjectsLayer;

        private IInteractiveClickableObject _mHoveredInteractiveObject; 

        private bool _isTouchingInteractiveObject;
        private Camera _mainCamera;
        
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Update()
        {
            ConfirmMainCamera();
            ManageMouseCursor();
            ManageMouseClick();
        }

        private void ConfirmMainCamera()
        {
            if (_mainCamera != null)
            {
                return;
            }
            
            if (Camera.main == null)
            {
                Debug.LogWarning("[MouseInputManager.ConfirmMainCamera] A Camera must be available in scene for mouse to work");
                return;
            }
            _mainCamera = Camera.main;
        }
        private void ManageMouseCursor()
        {
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            ProcessItemInteractiveObjects(ray);
            ProcessCursor();
        }
        
        private void ProcessItemInteractiveObjects(Ray ray)
        {
            if (Physics.Raycast(ray, out var hitInfo, 100, itemObjectsLayer))
            {
                var interactiveObject = hitInfo.collider.gameObject;
                _isTouchingInteractiveObject = true;
                if (!interactiveObject.TryGetComponent<IInteractiveClickableObject>(out _mHoveredInteractiveObject))
                {
                    Debug.LogError("[MouseInputManager.ProcessItemInteractiveObjects] Mouse interactive object component not found");
                    return;
                }
            }
            else
            {            
                _mHoveredInteractiveObject = null;
            }
        }

        private void ProcessCursor()
        {
            if (_isTouchingInteractiveObject)
            {
                Cursor.SetCursor(interactiveObjectMouseTexture, Vector2.zero, CursorMode.Auto);
            }
            else
            {
                Cursor.SetCursor(defaultMouseTexture, Vector2.zero, CursorMode.Auto);
            }
            _isTouchingInteractiveObject = false;
        }

        private void ManageMouseClick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                ProcessInteractiveItemClicked();
            }
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