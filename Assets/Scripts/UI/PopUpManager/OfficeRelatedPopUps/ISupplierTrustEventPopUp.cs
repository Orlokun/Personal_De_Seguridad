using DataUnits;

namespace UI.PopUpManager.OfficeRelatedPopUps
{
    public interface ISupplierTrustEventPopUp
    {
        void StartTrustEventPopUp(ICallableSupplier supplier, int trustAmount);
        public void ClearPreviousIcons();

    }
}