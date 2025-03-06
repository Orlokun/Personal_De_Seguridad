using System.Collections.Generic;
using System.Linq;
using DataUnits.GameCatalogues;
using DataUnits.ItemScriptableObjects;
using DataUnits.ItemSources;
using GamePlayManagement.BitDescriptions;
using GamePlayManagement.BitDescriptions.Suppliers;
using UnityEngine;
using Utils;

namespace GamePlayManagement.ProfileDataModules.ItemSuppliers.Stores
{
    public class ItemSupplierShop : IItemSupplierShop
    {
        public BitItemSupplier BitSupplierId => _mSupplierId;
        private BitItemSupplier _mSupplierId;

        private int _mUnlockedItems;
        
        private IItemSupplierDataObject _mSupplierData;
        private IItemsDataController _mItemDataController;
        private List<IItemSupplierObjectInInventory> _mAllSupplierItems = new List<IItemSupplierObjectInInventory>();
        public  List<IItemSupplierObjectInInventory> GetAllSupplierItems => _mAllSupplierItems;
        
        private Dictionary<int,int> _mTempLastStock;

        public void SaveTempLastStock()
        {
            _mTempLastStock = new Dictionary<int, int>();
            foreach (var item in _mAllSupplierItems)
            {
                _mTempLastStock.Add(item.GetItemData.BitId, item.GetCurrentAmount);
            }
        }

        public int GetItemTempLiveStock(int itemId)
        {
            if (!_mTempLastStock.ContainsKey(itemId))
            {
                return 0;
            }
            return _mTempLastStock[itemId];
        }

        public IItemSupplierObjectInInventory GetSupplierItemInStore(int itemId)
        {
            if(!BitOperator.IsActive(_mUnlockedItems,itemId))
            {
                Debug.LogWarning($"[ItemSupplierShop.GetSupplierItemInStore] Item '{itemId}' must be managed in store");
                return null;
            }
            return _mAllSupplierItems.First(x => x.GetItemData.BitId == itemId);
        }

        public void AddItemToCart(int id)
        {
            var itemInStore = GetSupplierItemInStore(id);
            itemInStore.ReduceStock(1);
        }

        public void ConfirmPurchase()
        {
            SaveTempLastStock();
        }

        public IItemSupplierDataObject GetSupplierData => _mSupplierData;

        public ItemSupplierShop(BitItemSupplier bitSupplierId, IItemsDataController itemDataController, IBaseItemSuppliersCatalogue suppliersCatalogue)
        {
            _mSupplierId = bitSupplierId;
            var supplierItems = itemDataController.GetAllSupplierItems(bitSupplierId);
            CreateObjectsInShop(supplierItems);
            _mSupplierData = suppliersCatalogue.GetItemSupplierData(BitSupplierId);
        }

        private void CreateObjectsInShop(List<IItemObject> items)
        {
            foreach (var itemSupplierObjectInInventory in items)
            {
                var shopObject = new ItemSupplierObjectInInventory(itemSupplierObjectInInventory);
                _mAllSupplierItems.Add(shopObject);
            }
        }

        public List<IItemObject> GetItemsOfType(BitItemType itemType)
        {
            return _mAllSupplierItems.Where(item => item.GetItemType == itemType).Select(item => item.GetItemData).ToList();
        }
        public IItemObject GetItemObject(int bitItemId)
        {
            return BitOperator.IsActive(_mUnlockedItems, bitItemId) ? _mAllSupplierItems.First(x=>x.GetItemData.BitId == bitItemId).GetItemData : null;
        }
        public bool IsItemManaged(int bitItemId)
        {
            return BitOperator.IsActive(_mUnlockedItems,bitItemId);
        }
        public void UnlockItem(int bitItemId)
        {
            if (BitOperator.IsActive(_mUnlockedItems, bitItemId))
            {
                return;
            }
            _mUnlockedItems |= bitItemId;
            var unlockedItem = _mAllSupplierItems.First(x=> x.GetItemData.BitId == bitItemId);
            unlockedItem.Unlock();
        }
        public void LockItem(int bitItemId)
        {
            if (BitOperator.IsActive(_mUnlockedItems, bitItemId))
            {
                return;
            }
            _mUnlockedItems &= ~bitItemId;
            _mAllSupplierItems[bitItemId].Lock();
        }
    }
}