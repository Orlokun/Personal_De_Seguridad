using UI.TabManagement.NotebookTabs.HorizontalTabletTabs;
using UI.TabManagement.TabEnums;

namespace UI.TabManagement.NotebookTabs
{
    public interface INotebookVerticalTab
    {
        public void SetNewTabState(NotebookHorizontalTabSource newSource, INotebookHorizontalTabGroup horizontalTab, int verticalTabIndex);
        public void UpdateTabSelection(int verticalTabIndex);
    }
}