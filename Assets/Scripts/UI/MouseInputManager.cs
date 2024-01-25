using UnityEngine;

namespace UI
{
    public class MouseInputManager : MonoBehaviour
    {
        private const int RightClick = 0;
        private const int LeftClick = 1;
        
        #region MouseSpriteManagement
        [SerializeField] private Texture2D interactiveObjectMouseTexture;
        [SerializeField] private Texture2D defaultMouseTexture;        
        #endregion

        [SerializeField] private LayerMask interactiveObjectsLayer;
        private IInteractiveClickableObject _mHoveredInteractiveObject; 
        private bool _isTouchingInteractiveObject;

        private IInteractiveClickableObject _currentlyClickedObject;
        
        private Camera _mainCamera;
        
        private bool isMouseStill;
        private bool isSnippetActive;
        private const float snipetWaitTime = 1.5f;
        private float currentStillTime = 0f;

        private Vector3 _mLastMousePosition = new Vector3();
        
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Update()
        {
            ConfirmMainCamera();
            ManageMouseCursor();
            ManageSnippet();
            ManageMouseClick();
            _mLastMousePosition = Input.mousePosition;
        }

        private void ActivateSnippet(string snippetTxt)
        {
            Debug.Log("Snippet Available");
            if (isSnippetActive)
            {
                isSnippetActive = true;
                return;
            }
        }
        private void EraseSnippet()
        {
            if (!isSnippetActive)
            {
                return;
            }
            isSnippetActive = false;
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
        private void ManageMouseCursor()
        {
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            ProcessItemInteractiveObjects(ray);
            ProcessCursor();
        }
        private void ProcessItemInteractiveObjects(Ray ray)
        {
            if (Physics.Raycast(ray, out var hitInfo, 100, interactiveObjectsLayer))
            {
                var interactiveObject = hitInfo.collider.gameObject;
                Debug.Log($"[ProcessItemInteractiveObjects] Object clicked: {interactiveObject.name}");
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
                _isTouchingInteractiveObject = false;
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
            isMouseStill = Input.mousePosition == _mLastMousePosition;
            if (isMouseStill)
            {
                currentStillTime += Time.deltaTime;
                if (currentStillTime >= snipetWaitTime)
                {
                    ActivateSnippet(snippetText);
                }
                return;
            }
            currentStillTime = 0;
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
                if (_currentlyClickedObject == null)
                {
                    Debug.Log("[ManageMouseClick] Nothing clicked before. Check if object hovered available");
                    if (_mHoveredInteractiveObject == null)
                    {
                        Debug.Log("[ManageMouseClick]Nothing clicked before, nothing to click now.");
                        return;
                    }
                    Debug.Log($"[ManageMouseClick] Clicked on hovered object {_mHoveredInteractiveObject}.");
                    ProcessInteractiveItemClicked();
                }
                
            }
        }
        private void ProcessInteractiveItemClicked()
        {
            _mHoveredInteractiveObject.ReceiveSelectClickEvent();
            _currentlyClickedObject = _mHoveredInteractiveObject;
        }
        #endregion

    }
}