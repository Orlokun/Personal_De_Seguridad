namespace UI.TabManagement.Interfaces
{
    public interface ITabGroup
    {
        public bool IsTabGroupActive { get; }
        public bool ActivateTabInUI();
        public bool DeactivateGroupInUI();
        public void UpdateDictionaryData();
        public void UpdateTabItemGroupTypes(int selectedTabIndex);
        
        public int ActiveTab { get; }
    }
}