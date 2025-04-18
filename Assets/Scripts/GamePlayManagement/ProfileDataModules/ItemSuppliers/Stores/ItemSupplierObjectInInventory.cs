﻿using DataUnits.ItemScriptableObjects;
using GamePlayManagement.BitDescriptions;

namespace GamePlayManagement.ProfileDataModules.ItemSuppliers.Stores
{
    public class ItemSupplierObjectInInventory : IItemSupplierObjectInInventory
    {
        private IItemObject _mItemObjectBaseData;
        private int _mItemAmount;
        private int _mItemCost;
        private int _mMaxItemAmount;
        private bool _mIsLocked = true;
        

        public BitItemType GetItemType => _mItemObjectBaseData.ItemType;
        public IItemObject GetItemData => _mItemObjectBaseData;
        public int GetCurrentAmount => _mItemAmount;
        
        public void SetStock(int amount)
        {
            _mItemAmount = amount;
        }

        public bool IsLocked => _mIsLocked;
        public void ReplenishStock()
        {
            _mItemAmount = _mMaxItemAmount;
        }

        public void ReduceStock(int amount)
        {
            _mItemAmount -= amount;
        }

        public void AddStock(int amount)
        {
            _mItemAmount += amount;
            if (_mItemAmount > _mMaxItemAmount)
            {
                _mItemAmount = _mMaxItemAmount;
            }
        }
        public void Unlock()
        {
            _mIsLocked = false;
        }
        
        public void Lock()
        {
            _mIsLocked = true;
        }


        public ItemSupplierObjectInInventory(IItemObject itemData)
        {
            _mItemObjectBaseData = itemData;
            _mMaxItemAmount = itemData.ItemAmount;
            _mItemAmount = _mMaxItemAmount;
            _mItemCost = itemData.Cost;
        }
    }
}