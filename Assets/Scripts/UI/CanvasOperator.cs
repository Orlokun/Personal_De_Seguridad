using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace UI
{
    public interface ICanvasOperator
    {
        void ActivateThisElementsOnly(IEnumerable<int> newActiveUIElements);
        void DeactivateAllElements();
        void ToggleUIElement(int toggledElement);
        void ActivateUIElements(IEnumerable<int> addedElements);
        void DeactivateUIElements(IEnumerable<int> removedElements);
        void ActivateUIElement(int addedElement);
        void DeactivateUIElement(int removedElement);
        bool IsElementActive(int element);
        void SavePanelsDictData();
        CanvasBitId GetCanvasID { get; }
    }

    [Serializable]
    public class CanvasOperator : MonoBehaviour, ICanvasOperator
    {
        //Panels will be populated with the bit number assigned
        private Dictionary<int, GameObject> _panelsInCanvas = new Dictionary<int, GameObject>();

        [SerializeField] private CanvasBitId canvasId;
        public CanvasBitId GetCanvasID => canvasId;
        
        //UI Panels must be added in the same order as they are set in the incremental bits
        [SerializeField] private List<Transform> uiPanels;
        private int _activeUIElements = 0;
        public void ActivateThisElementsOnly(IEnumerable<int> newActiveUIElements)
        {
            var newActiveElements = newActiveUIElements.Sum();
            _activeUIElements = newActiveElements;
            UpdateUIObjects();
        }

        public void DeactivateAllElements()
        {
            _activeUIElements = 0;
            UpdateUIObjects();
        }

        public void ToggleUIElement(int toggledElement)
        {
            if ((_activeUIElements & toggledElement) != 0)
            {
                _activeUIElements &= ~toggledElement;
            }
            else
            {
                _activeUIElements |= toggledElement;
            }
            UpdateUIObjects();
        }
        
        public void ActivateUIElements(IEnumerable<int> addedElements)
        {
            foreach (var addedElement in addedElements)
            {
                if ((_activeUIElements & addedElement) != 0)
                {
                    continue;
                }
                _activeUIElements |= addedElement;
            }
            UpdateUIObjects();
        }
        
        public void DeactivateUIElements(IEnumerable<int> removedElements)
        {
            foreach (var removedElement in removedElements)
            {
                //Makes sure the UI element is activated in the current state before removing 
                if ((_activeUIElements & removedElement) == 0)
                {
                    continue;
                }
                _activeUIElements &= ~removedElement;
            }
            UpdateUIObjects();
        }

        public void ActivateUIElement(int addedElement)
        {
            if ((_activeUIElements & addedElement) != 0)
            {
                return;
            }
            _activeUIElements |= addedElement;
            UpdateUIObjects();
        }

        public void DeactivateUIElement(int removedElement)
        {
            if ((_activeUIElements & removedElement) == 0)
            {
                return;
            }
            _activeUIElements &= ~removedElement;
            UpdateUIObjects();
        }

        public bool IsElementActive(int element)
        {
            return (_activeUIElements & element) != 0;
        }
        public void SavePanelsDictData()
        {
            _panelsInCanvas = new Dictionary<int, GameObject>();
            SavePanelsInDictionary();
        }
        private void SavePanelsInDictionary()
        {
            var maxNumberInBit = ReturnMaxBitNumber();
            var currentListObject = 0;
            for (int i = 1; i <= maxNumberInBit; i *= 2)
            {
                var valueObject = uiPanels[currentListObject].gameObject;
                _panelsInCanvas.Add(i,valueObject);
                currentListObject++;
            }
        }
        private int ReturnMaxBitNumber()
        {
            if (uiPanels.Count == 0)
            {
                return 0;
            }
            var bitNumber = 1;
            for (var i = 0; i<uiPanels.Count;i++) 
            {
                if (i == 0)
                {
                    continue;
                }
                bitNumber = bitNumber * 2;
            }
            return bitNumber;
        }
        private void UpdateUIObjects()  
        {
            foreach (var bitPanelKvP in _panelsInCanvas)
            {
                var isActive = (_activeUIElements & bitPanelKvP.Key) != 0;
                
                UIOfficePanel component;
                if (!bitPanelKvP.Value)
                {
                    continue;
                }
                
                var isOfficePanel = bitPanelKvP.Value.TryGetComponent<UIOfficePanel>(out component);
                bitPanelKvP.Value.SetActive(isActive);
                if (GetCanvasID == CanvasBitId.Office && isActive && isOfficePanel)
                {
                    foreach (var officeFadeInElement in component.officeFadeInElements)
                    {
                        if (officeFadeInElement != null)
                        {
                            LeanTween.alpha(officeFadeInElement, 0, 0);
                            LeanTween.alpha(officeFadeInElement, 1, 2f).setEase(LeanTweenType.easeOutSine);
                        }
                    }
                } 
            }
        }
    }
}