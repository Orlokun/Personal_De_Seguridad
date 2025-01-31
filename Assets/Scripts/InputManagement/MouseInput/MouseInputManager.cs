using UnityEngine;

namespace InputManagement.MouseInput
{
    public class MouseInputManager : MonoBehaviour
    {
        private const int RightClick = 1;
        private const int LeftClick = 0;
        
        #region MouseSpriteManagement
        [SerializeField] private Texture2D interactiveObjectMouseTexture;
        [SerializeField] private Texture2D defaultMouseTexture;        
        #endregion

        private Camera _mainCamera;
        
        private bool _isMouseStill;
        private bool _isSnippetActive;
        private const float SnipetWaitTime = 1.5f;
        private float _currentStillTime = 0f;

        private Vector3 _mLastMousePosition = new Vector3();

        #region ManageInteractionData
        [SerializeField] private LayerMask interactiveObjectsLayer;
        
        private IInteractiveClickableObject _mHoveredInteractiveObject; 
        private bool _isHoveringObject;
        
        private IInteractiveClickableObject _currentlyClickedObject;
        #endregion
        
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Update()
        {
            ConfirmMainCamera();
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            ManageMouseCursor(ray);
            ManageSnippet();
            ManageMouseClick();
            _mLastMousePosition = Input.mousePosition;
        }

        private void ActivateSnippet(string snippetTxt)
        {
            Debug.Log("Snippet Available");
            if (_isSnippetActive)
            {
                _isSnippetActive = true;
                return;
            }
        }
        private void EraseSnippet()
        {
            if (!_isSnippetActive)
            {
                return;
            }
            _isSnippetActive = false;
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
        #region HoverObject
        private void ManageMouseCursor(Ray ray)
        {
            ProcessObjectHovering(ray);
            ProcessCursor();
        }
        private void ProcessObjectHovering(Ray ray)
        {
            if (Physics.Raycast(ray, out var hitInfo, 100, interactiveObjectsLayer))
            {
                var interactiveObject = hitInfo.collider.gameObject;
                Debug.Log($"[ProcessItemInteractiveObjects] Object hovered: {interactiveObject.name}");
                _isHoveringObject = true;
                if (!interactiveObject.TryGetComponent<IInteractiveClickableObject>(out _mHoveredInteractiveObject))
                {
                    Debug.LogError("[MouseInputManager.ProcessItemInteractiveObjects] Mouse interactive object component not found");
                    return;
                }
            }
            else
            {
                _mHoveredInteractiveObject = null;
                _isHoveringObject = false;
            }
        }
        private void ProcessCursor()
        {
            if (_isHoveringObject)
            {
                Cursor.SetCursor(interactiveObjectMouseTexture, Vector2.zero, CursorMode.Auto);
            }
            else
            {
                Cursor.SetCursor(defaultMouseTexture, Vector2.zero, CursorMode.Auto);
            }
        }
        #endregion
        
        #region Snippet
        private void ManageSnippet()
        {
            if (_mHoveredInteractiveObject == null)
            {
                EraseSnippet();
                return;
            }

            if (!_mHoveredInteractiveObject.HasSnippet)
            {
                EraseSnippet();
                return;
            }
            var snippetText = _mHoveredInteractiveObject.GetSnippetText;
            _isMouseStill = Input.mousePosition == _mLastMousePosition;
            if (_isMouseStill)
            {
                _currentStillTime += Time.deltaTime;
                if (_currentStillTime >= SnipetWaitTime)
                {
                    ActivateSnippet(snippetText);
                }
                return;
            }
            _currentStillTime = 0;
            EraseSnippet();
        }
        
        #endregion
        
        #region ClickObject
        /// <summary>
        /// Possible cases:
        /// 1. Nothing clicked - clicks nothing
        /// 2. Nothing clicked - clicks something
        /// 3. Something clicked - clicks environment
        /// 4. Something clicked - clicks viable object
        /// Questions: How do I know if its a valid click? 
        /// </summary>
        private void ManageMouseClick()
        {
            if (Input.GetMouseButtonDown(LeftClick))
            {
                ManageLeftClick();
            }
        }

        /// <summary>
        /// Possible cases:
        /// 1. Nothing clicked - clicks nothing
        /// 2. Nothing clicked - clicks something
        /// 3. Something clicked - clicks environment
        /// 4. Something clicked - clicks viable object
        /// Questions: How do I know if its a valid click? 
        /// </summary>
        private void ManageLeftClick()
        {
            if (_currentlyClickedObject == null)
            {
                if (_mHoveredInteractiveObject == null)
                {
                    // 1. Nothing clicked - clicks nothing
                    return;
                }
                // 2. Nothing clicked - clicks something
                ProcessClickOnObject();
            }
            else
            {
                //TODO: Create different interfaces for interactive which may not need this.
                var hitInfo = GetHitInfo();
                _currentlyClickedObject.ReceiveActionClickedEvent(hitInfo); 
                ClearCurrentlyClickedObject();
            }
        }

        private RaycastHit GetHitInfo()
        {
            var newPoint = Input.mousePosition;
            ConfirmMainCamera();
            var ray = _mainCamera.ScreenPointToRay(newPoint);
            Physics.Raycast(ray, out var hitInfo, 100);
            return hitInfo;
        }

        private void ClearCurrentlyClickedObject()
        {
            _currentlyClickedObject = null;
            _mHoveredInteractiveObject = null;
        }
        
        private void ProcessClickOnObject()
        {
            _mHoveredInteractiveObject.ReceiveClickEvent();
            _currentlyClickedObject = _mHoveredInteractiveObject;
        }
        #endregion

    }
}