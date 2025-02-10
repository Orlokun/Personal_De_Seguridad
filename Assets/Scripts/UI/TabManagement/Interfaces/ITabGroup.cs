namespace UI.TabManagement.Interfaces
{
    public interface ITabGroup
    {
        public void ActivateTabletUI();
        public bool DeactivateGroupInUI();
        public void UpdateItemsContent(int horizontalTabIndex, int verticalTabIndex);
        public int ActiveTab { get; }
    }
}