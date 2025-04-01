using UI.TabManagement.Interfaces;

namespace UI.TabManagement
{
    public interface IItemsTabGroup : ITabGroup
    {
        public void ActivateItemsBarInUI();
        public void UpdateItemsContent(int verticalTabIndex);
        public bool IsBarActive { get; }
    }
}