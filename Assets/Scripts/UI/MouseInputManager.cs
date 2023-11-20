using UnityEngine;
namespace UI
{
    public class MouseInputManager : MonoBehaviour
    {
        [SerializeField] private Texture2D interactiveObjectMouseTexture;
        [SerializeField] private Texture2D defaultMouseTexture;
        [SerializeField] private LayerMask officeObjectLayer;

        private IOfficeInteractiveObject _mHoveredObject; 
        
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo, 100, officeObjectLayer))
            {
                var interactiveObject = hitInfo.collider.gameObject;
                if (!interactiveObject.TryGetComponent<IOfficeInteractiveObject>(out _mHoveredObject))
                {
                    Debug.LogError("[MouseInputManager.ManageMouseCursor] Mouse interactive object component not found");
                    return;
                }
                
                Cursor.SetCursor(interactiveObjectMouseTexture, Vector2.zero, CursorMode.Auto);   
            }
            else
            {            
                Cursor.SetCursor(defaultMouseTexture, Vector2.zero, CursorMode.Auto);
                _mHoveredObject = null;
            }
        }

        private void ManageMouseClick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (_mHoveredObject == null)
                {
                    return;
                }
                _mHoveredObject.SendClickObject();
            }
        }
    }
}