using GameDirection;
using GamePlayManagement.ProfileDataModules.ItemSuppliers;
using UI.TabManagement.AbstractClasses;
using UnityEngine;
namespace UI.TabManagement
{
    public class ItemsTabGroup : TabGroup
    {
        [SerializeField] private BitItemType startType;
        [SerializeField] private Transform gridParentObject;
        [SerializeField] private Transform storeTitle;

        private IItemSuppliersModule _activeItemsModule;
        
        public override bool ActivateTabInUI()
        {
            MIsTabActive = true;
            MuiController.ActivateObject(CanvasBitId.GamePlayCanvas,GameplayPanelsBitStates.ITEM_DETAILED_SIDEBAR);
            return MIsTabActive;
        }

        public override bool DeactivateGroupInUI()
        {
            MIsTabActive = false;
            MuiController.DeactivateObject(CanvasBitId.GamePlayCanvas,GameplayPanelsBitStates.ITEM_DETAILED_SIDEBAR);
            return MIsTabActive;
        }

        public override void UpdateTabGroupContent(int selectedTabIndex)
        {
            base.UpdateTabGroupContent(selectedTabIndex);
            if (gridParentObject.childCount > 0)
            {
                ClearGrid();
            }
            UpdateSelectedItemsModule((BitItemType) selectedTabIndex);
            var activeItems = _activeItemsModule.AllActiveSuppliers;
        }

        private void UpdateSelectedItemsModule(BitItemType index)
        {
            //_activeItemsModule = GameDirector.Instance.GetActiveGameProfile.GetActiveItemsInModule(index);
        }
        private void ClearGrid()
        {
            foreach (Transform itemObject in gridParentObject)
            {
                Destroy(itemObject.gameObject);
            }
        }
    }
}