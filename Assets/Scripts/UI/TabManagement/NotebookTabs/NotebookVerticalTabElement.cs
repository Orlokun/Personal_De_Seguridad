using System;
using InputManagement;
using TMPro;
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
        public string TabElementName { get; }
        public Sprite Icon { get; }
    
        [SerializeField] private Button mTabButton;
        [SerializeField] private TMP_Text mTabName;
        public void SetIcon(Sprite tabIcon)
        {
            _mTabIcon??= GetComponent<Image>();
            _mTabIcon.sprite = tabIcon;
        }
        
        public void SetName(string tabName)
        {
            mTabName.text = tabName;
        }

        private void Awake()
        {
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
        public override void TabSelected()
        {
            Debug.Log($"POINTER CLICKED OBJECT: {gameObject.name}");
            if (TabGroup.ActiveTab != MTabElementIndex)
            {
                var mTabGroup = (INotebookVerticalTabGroup)TabGroup;
                mTabGroup.UpdateTabSelection(MTabElementIndex);
            }
        }
    }
}


