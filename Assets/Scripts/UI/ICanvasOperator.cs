using System.Collections.Generic;

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
}