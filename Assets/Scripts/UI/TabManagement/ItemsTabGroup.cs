using System.Collections.Generic;
using DataUnits.ItemScriptableObjects;
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
        [SerializeField] private GameObject itemInGridPrefab;
        private IItemSuppliersModule _suppliersModule;
        private List<IItemObject> _activeItems = new List<IItemObject>();

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
            _activeItems = GetItemsOfType((BitItemType) selectedTabIndex);
            foreach (var activeItem in _activeItems)
            {
                var activeItemObject = Instantiate(itemInGridPrefab, gridParentObject);
                var itemBaseObject = activeItemObject.GetComponent<BaseInventoryItem>();
                itemBaseObject.IconImage.sprite = activeItem.ItemIcon;
            }
        }

        private List<IItemObject> GetItemsOfType(BitItemType itemType)
        {
            _suppliersModule = GameDirector.Instance.GetActiveGameProfile.GetActiveSuppliersModule();
            var itemsOfType = _suppliersModule.GetItemsOfType(itemType);
            return itemsOfType;
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