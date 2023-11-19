using UnityEngine;
namespace UI
{
    public class MouseInputManager : MonoBehaviour
    {
        [SerializeField] private Texture2D interactiveObjectMouseTexture;
        [SerializeField] private Texture2D defaultMouseTexture;
        [SerializeField] private LayerMask officeObjectLayer;
        
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo, 100, officeObjectLayer))
            {
                var interactiveObject = hitInfo.collider.gameObject;
                if (!interactiveObject.TryGetComponent<OfficeInteractiveObject>(out var interactiveComponent))
                {
                    return;
                }
                Cursor.SetCursor(interactiveObjectMouseTexture, Vector2.zero, CursorMode.Auto);   
                Debug.Log("[MouseInputManager] Hovering an interactive office object");
            }
            else
            {            
                Debug.Log("[MouseInputManager] Regular state");
                Cursor.SetCursor(defaultMouseTexture, Vector2.zero, CursorMode.Auto);   
            }
        }
    }
}