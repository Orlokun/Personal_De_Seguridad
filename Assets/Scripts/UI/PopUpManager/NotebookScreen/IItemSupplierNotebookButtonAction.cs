namespace UI.PopUpManager.NotebookScreen
{
    public interface IItemSupplierNotebookButtonAction : IPopUpObject
    {
        public void SetSupplier(int selectedSupplier, CallableObjectType callableObjectType);
    }
}