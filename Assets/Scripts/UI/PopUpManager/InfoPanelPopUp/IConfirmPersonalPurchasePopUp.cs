namespace UI.PopUpManager.InfoPanelPopUp
{
    public interface IConfirmPersonalPurchasePopUp : IPopUpObject
    {
        public event ConfirmPersonalPurchasePopUp.ConfirmPurchaseButtonPressed OnPurchaseConfirmed;
        public void SetLostValue(int lostValue);
    }
}