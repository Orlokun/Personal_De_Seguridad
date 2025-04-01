using DataUnits.ItemScriptableObjects;

namespace UI.PopUpManager.InfoPanelPopUp
{
    public interface IItemInfoPanel : IPopUpObject
    {
        public void SetAndDisplayInfoPanelData(IItemObject itemToDisplay);
    }
}