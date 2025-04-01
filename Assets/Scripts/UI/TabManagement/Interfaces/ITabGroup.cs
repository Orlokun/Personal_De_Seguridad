namespace UI.TabManagement.Interfaces
{
    public interface ITabGroup
    {
        public bool DeactivateItemsDetailBar();
        public int ActiveTab { get; }

    }
}