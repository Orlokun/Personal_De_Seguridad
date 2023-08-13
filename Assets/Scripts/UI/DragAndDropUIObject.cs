using ItemPlacement;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class DragAndDropUIObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private Vector3 _offset;
        private bool _dragging;
        private bool _selected;

        private Camera _mainCamera;

        private Transform _naturalParent;
        private int _naturalChildIndex;

        private void Awake()
        {
            _mainCamera = FindObjectOfType<Camera>();
            _naturalParent = transform.parent;
            _naturalChildIndex = transform.GetSiblingIndex();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            transform.SetParent(_naturalParent.parent.parent.parent);
            //_offset = transform.position - _mainCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, distanceFromCamera));
            _dragging = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_dragging)
            {
                var newPosition = _mainCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, 0));
                Debug.Log($"[OnDrag] New Position = {newPosition}");
                transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            transform.SetParent(_naturalParent);
            transform.SetSiblingIndex(_naturalChildIndex);
            _dragging = false;
            FloorPlacementManager.Instance.ReleaseDraggedItem();
        }
    }
}

