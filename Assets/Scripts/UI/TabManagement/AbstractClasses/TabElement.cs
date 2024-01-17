using TMPro;
using UI.TabManagement.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Utils;

namespace UI.TabManagement.AbstractClasses
{
    public class TabElement : MonoBehaviour, IPointerClickHandler, ITabElement, IInitializeWithArg1<ITabGroup>
    {
        private bool _mInitialized;
        private ITabGroup _tabGroup;
        private int _mIndex;

        private bool _mLockState;

        [SerializeField] private string SnippetName;
        [SerializeField] private TMP_Text snippetNameText;
        private Image _mBackground;

        public void SetSnippetNameText(string snippetName)
        {
            SnippetName = snippetName;
            snippetNameText.text = SnippetName;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log($"POINTER CLICKED OBJECT: {gameObject.name}");
            if (_tabGroup.IsTabGroupActive && _tabGroup.ActiveTab != _mIndex)
            {
                _tabGroup.UpdateItemsContent(_mIndex);
                return;
            }
            
            if(_tabGroup.IsTabGroupActive && _tabGroup.ActiveTab == _mIndex)
            {
                _tabGroup.DeactivateGroupInUI();
                return;
            }

            if (!_tabGroup.IsTabGroupActive)
            {
                _tabGroup.ActivateTabInUI();
                _tabGroup.UpdateItemsContent(_mIndex);
            }
            Debug.Log($"[TabElement.OnPointerClick] Tab Group Named {gameObject.name}");
        }

        public bool IsInitialized => _mInitialized;
        public void Initialize(ITabGroup injectionClass)
        {
            if (_mInitialized)
            {
                return;
            }
            _tabGroup = injectionClass;
            _mInitialized = true;
        }

        public void SetTabIndex(int index)
        {
            _mIndex = index;
        }
    }
}