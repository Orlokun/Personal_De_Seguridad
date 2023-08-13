using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts.ItemPlacement
{
    public class BaseSelectItemForPlacement : MonoBehaviour, IPointerDownHandler, IPointerClickHandler
    {
        protected GameObject MInstantiatedObject;
        [SerializeField]protected Image mUIBackgroundImage;
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("[base.OnPointerClick]");
            mUIBackgroundImage.gameObject.SetActive(true);
            if (MInstantiatedObject.activeInHierarchy != true)
            {
                MInstantiatedObject.SetActive(true);
            }
        }
    
        public virtual void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("[OnPointerDown]");
            if (MInstantiatedObject.activeInHierarchy != true)
            {
                MInstantiatedObject.SetActive(true);
            }
        }
    }
}