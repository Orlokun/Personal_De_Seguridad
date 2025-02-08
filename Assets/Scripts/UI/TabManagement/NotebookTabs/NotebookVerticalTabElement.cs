using InputManagement;
using UI.TabManagement.AbstractClasses;
using UI.TabManagement.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.TabManagement.NotebookTabs
{
    public class NotebookVerticalTabElement : TabElement, ISnippet, IPointerEnterHandler, IPointerExitHandler, IVerticaTabElement
    {
        private Image _mTabIcon;
        public Sprite Icon { get; }
        public string TabElementName { get; }
    
        [SerializeField] private Button mTabButton;
        public void SetIcon(Sprite tabIcon)
        {
            _mTabIcon??= GetComponent<Image>();
            _mTabIcon.sprite = tabIcon;
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
        public override void TabSelected()
        {
            Debug.Log($"POINTER CLICKED OBJECT: {gameObject.name}");
            if (TabGroup.ActiveTab != MTabElementIndex)
            {
                var mTabGroup = (INotebookVerticalTab)TabGroup;
                mTabGroup.UpdateTabSelection(MTabElementIndex);
            }
        }

        public override void Initialize(ITabGroup injectionClass)
        {
            base.Initialize(injectionClass);
            mTabButton.onClick.AddListener(TabSelected);
        }

    }

}


