namespace UI.PopUpManager.InfoPanelPopUp
{
    public interface ISupplierShopPanel
    {
        void AttemptAddToCart(int itemId);
        void AttemptRemoveFromCart(int itemId);
        int GetCurrentItemStock(int itemId);
    }
}