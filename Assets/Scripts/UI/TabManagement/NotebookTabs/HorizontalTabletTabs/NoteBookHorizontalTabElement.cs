using InputManagement;
using UI.TabManagement.AbstractClasses;
using UI.TabManagement.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.TabManagement.NotebookTabs.HorizontalTabletTabs
{
    public class NoteBookHorizontalTabElement : TabElement, ISnippet, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Button mTabButton;
        private new ITabletTabGroup MTabGroup => (INotebookHorizontalTabletTabGroup)base.MTabGroup;
        public override void TabSelected()
        {
            Debug.Log($"Horizontal Tab with index: {MTabElementIndex} selected");
            if(MTabGroup.ActiveTab == MTabElementIndex)
            {
                Debug.Log($"[NoteBookHorizontalTabElement] Selected Tab index ({MTabElementIndex}) is not different than current selected");
                return;
            }
            MTabGroup.ActivateTabletUI();
            MTabGroup.UpdateItemsContent(MTabElementIndex, 1);
        }

        public override void Initialize(ITabGroup injectionClass)
        {
            base.Initialize(injectionClass);
            mTabButton.onClick.AddListener(TabSelected);
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