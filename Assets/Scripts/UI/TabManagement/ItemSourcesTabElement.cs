using UI.TabManagement.AbstractClasses;
using UnityEngine.EventSystems;
namespace UI.TabManagement
{
    public class ItemSourcesTabElement : TabElement, ISnippet, IPointerExitHandler, IPointerEnterHandler
    {
        public void ToggleSnippet(bool isActive)
        {
            
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            ToggleSnippet(true);
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            ToggleSnippet(false);
        }
    }
}