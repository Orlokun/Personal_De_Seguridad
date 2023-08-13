namespace UI.TabManagement.Interfaces
{
    public interface ITabGroup
    {
        public bool IsTabGroupActive { get; }
        public bool ActivateTabInUI();
        public bool DeactivateGroupInUI();
        public void UpdateTabGroupContent(int selectedTabIndex);
        
        public int ActiveTab { get; }
    }
}