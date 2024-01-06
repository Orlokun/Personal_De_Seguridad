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
        #endregion

    }
}