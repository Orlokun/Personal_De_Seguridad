using UI.TabManagement.NotebookTabs.HorizontalTabletTabs;
using UI.TabManagement.TabEnums;

namespace UI.TabManagement.NotebookTabs
{
    public interface INotebookVerticalTabGroup
    {
        public void SetNewTabState(NotebookHorizontalTabSource newSource, INotebookHorizontalTabletTabGroup horizontalTabletTab, int verticalTabIndex);
        public void UpdateTabSelection(int verticalTabIndex);
        public void UpdateDictionaryData();
    }
}