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
        
        
        [SerializeField] private GameObject guardInGridPrefab;
        [SerializeField] private GameObject camInGridPrefab;
        [SerializeField] private GameObject weaponInGridPrefab;
        [SerializeField] private GameObject trapInGridPrefab;
        [SerializeField] private GameObject otherInGridPrefab;
        
        
        
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
                var itemTypePrefab = GetItemPrefab((BitItemType) selectedTabIndex);
                var activeItemObject = Instantiate(itemTypePrefab, gridParentObject);
                
                //Update Object aspect in UI 
                var itemBaseObject = activeItemObject.GetComponent<BaseInventoryItem>();
                itemBaseObject.IconImage.sprite = activeItem.ItemIcon;
            }
        }

        private GameObject GetItemPrefab(BitItemType type)
        {
            switch (type)
            {
                case BitItemType.GUARD_ITEM_TYPE:
                    return guardInGridPrefab;
                case BitItemType.CAMERA_ITEM_TYPE:
                    return camInGridPrefab;
                case BitItemType.WEAPON_ITEM_TYPE:
                    return weaponInGridPrefab;
                case BitItemType.TRAP_ITEM_TYPE:
                    return trapInGridPrefab;
                case BitItemType.OTHERS_ITEM_TYPE:
                    return otherInGridPrefab;
                default:
                    return null;
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