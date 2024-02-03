using InputManagement;
using UI.TabManagement.AbstractClasses;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.TabManagement.NBVerticalTabs
{
    public class NotebookVerticalTabElement : TabElement, ISnippet, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]private Image _tabIcon;
    
        public void SetIcon(Sprite tabIcon)
        {
            _tabIcon.sprite = tabIcon;
        }
        public void ToggleSnippet(bool isActive)
        {
        }
    
        public void OnPointerEnter(PointerEventData eventData)
        {
        }
    
        public void OnPointerExit(PointerEventData eventData)
        {
        }
        
    }
}


