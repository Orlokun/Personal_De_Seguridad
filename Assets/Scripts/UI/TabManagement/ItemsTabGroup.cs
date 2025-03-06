using System.Collections.Generic;
using GameDirection;
using GamePlayManagement;
using GamePlayManagement.BitDescriptions;
using UI.TabManagement.AbstractClasses;
using UI.TabManagement.Interfaces;
using UI.TabManagement.ItemTypeTab;
using UnityEngine;
namespace UI.TabManagement
{
    public class ItemsTabGroup : TabGroup, IItemsTabGroup
    {
        [SerializeField] private BitItemType startType;
        [SerializeField] private Transform gridParentObject;
        [SerializeField] private Transform storeTitle;
        
        
        [SerializeField] private GameObject guardInGridPrefab;
        [SerializeField] private GameObject camInGridPrefab;
        [SerializeField] private GameObject weaponInGridPrefab;
        [SerializeField] private GameObject trapInGridPrefab;
        [SerializeField] private GameObject otherInGridPrefab;
        
        
        
        private IPlayerInventoryModule _mPlayerInventory;
        private List<IItemInInventory> _activeItems = new List<IItemInInventory>();

        public bool IsBarActive => MuiController.IsObjectActive(CanvasBitId.GamePlayCanvas,GameplayPanelsBitStates.ITEM_DETAILED_SIDEBAR);
        public void ActivateItemsBarInUI()
        {
            MuiController.ActivateObject(CanvasBitId.GamePlayCanvas,GameplayPanelsBitStates.ITEM_DETAILED_SIDEBAR);
        }

        public override bool DeactivateItemsDetailBar()
        {
            MIsTabActive = false;
            MuiController.DeactivateObject(CanvasBitId.GamePlayCanvas,GameplayPanelsBitStates.ITEM_DETAILED_SIDEBAR);
            return MIsTabActive;
        }
        

        public void UpdateItemsContent(int verticalTabIndex)
        {
            MActiveTab = verticalTabIndex;
            if (gridParentObject.childCount > 0)
            {
                ClearGrid();
            }
            _activeItems = GetItemsOfType((BitItemType) verticalTabIndex);
            foreach (var activeItem in _activeItems)
            {
                var itemTypePrefab = GetItemPrefabType((BitItemType) verticalTabIndex);
                var activeItemObject = Instantiate(itemTypePrefab, gridParentObject);
                
                //Update Object aspect in UI 
                var itemBaseObject = activeItemObject.GetComponent<BaseItemIconUIObject>();
                itemBaseObject.Initialize(activeItem);
            }
        }

        private GameObject GetItemPrefabType(BitItemType type)
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

        private List<IItemInInventory> GetItemsOfType(BitItemType itemType)
        {
            if (_mPlayerInventory == null)
            {
                _mPlayerInventory = GameDirector.Instance.GetActiveGameProfile.GetInventoryModule();
            }
            var itemsOfType = _mPlayerInventory.GetItemsOfType(itemType);
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

    public interface IItemsTabGroup : ITabGroup
    {
        public void ActivateItemsBarInUI();
        public void UpdateItemsContent(int verticalTabIndex);
        public bool IsBarActive { get; }
    }
}