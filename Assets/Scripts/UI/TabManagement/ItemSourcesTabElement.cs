using InputManagement;
using UI.TabManagement.AbstractClasses;
using UI.TabManagement.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.TabManagement
{
    public class ItemSourcesTabElement : TabElement, ISnippet, IPointerExitHandler, IPointerEnterHandler
    {
        [SerializeField] private Button mTabButton;
        private new IItemsTabGroup MTabGroup => (IItemsTabGroup)base.MTabGroup;

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
        public override void Initialize(ITabGroup injectionClass)
        {
            base.Initialize(injectionClass);
            mTabButton.onClick.AddListener(TabSelected);
        }
        public override void TabSelected()
        {
            Debug.Log($"Horizontal Tab with index: {MTabElementIndex} selected");
            if(MTabGroup.ActiveTab == MTabElementIndex && MTabGroup.IsBarActive)
            {
                MTabGroup.DeactivateItemsDetailBar();
                Debug.Log($"[NoteBookHorizontalTabElement] Selected Tab index ({MTabElementIndex}) is not different than current selected");
                return;
            }
            MTabGroup.ActivateItemsBarInUI();
            MTabGroup.UpdateItemsContent(MTabElementIndex);
        }
    }
}