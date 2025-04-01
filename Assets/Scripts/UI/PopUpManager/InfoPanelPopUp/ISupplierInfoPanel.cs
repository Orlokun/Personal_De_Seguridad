using DataUnits.JobSources;

namespace UI.PopUpManager.InfoPanelPopUp
{
    public interface ISupplierInfoPanel : IPopUpObject
    {
        public void SetAndDisplayInfoPanelData(ISupplierBaseObject supplierToDisplay);
    }
}