using UI.TabManagement.Interfaces;
namespace UI.TabManagement.NBVerticalTabs
{
    public interface INotebookVerticalTab : ITabGroup
    {
        public void SetNewTabState(NotebookVerticalTabSource newSource, INotebookHorizontalTabGroup horizontalTab, int verticalTabIndex);
    }
}