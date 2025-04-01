namespace UI.TabManagement.Interfaces
{
    public interface ITabletTabGroup : ITabGroup
    {
        public void ActivateTabletUI();
        public void UpdateItemsContent(int horizontalTabIndex, int verticalTabIndex);
    }
}