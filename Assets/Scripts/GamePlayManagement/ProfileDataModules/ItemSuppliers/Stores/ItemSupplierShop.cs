using System.Collections.Generic;
using DataUnits.ItemScriptableObjects;
using GamePlayManagement.BitDescriptions.Suppliers;
using UnityEngine;

namespace GamePlayManagement.ProfileDataModules.ItemSuppliers.Stores
{
    public interface IItemSupplierShop
    {
        void AddItemToSupplier(int item);
        void RemoveItemFromSupplier(int item);
        public bool IsItemManaged(int bitItemId);
        public BitItemSupplier BitSupplierId { get; }
        public IItemObject GetItemObject(int bitItemId);

    }

    public class ItemSupplierShop : IItemSupplierShop
    {
        public BitItemSupplier BitSupplierId { get; }

        private int _mActiveItems = 0;
        private Dictionary<int, IItemObject> _activeItemsData = new Dictionary<int, IItemObject>();
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bitSupplierId"></param>
        public ItemSupplierShop(BitItemSupplier bitSupplierId)
        {
            BitSupplierId = bitSupplierId;
        }
        
        public IItemObject GetItemObject(int bitItemId)
        {
            return (_mActiveItems & bitItemId) == 0 ? null : _activeItemsData[bitItemId];
        }
        public bool IsItemManaged(int bitItemId)
        {
            return (_mActiveItems & bitItemId) != 0;
        }
        public void AddItemToSupplier(int bitItemId)
        {
            if ((_mActiveItems & bitItemId) != 0)
            {
                return;
            }
            _mActiveItems |= bitItemId;
            
            if (_activeItemsData.ContainsKey(bitItemId))
            {
                return;
            }

            var getItem = BaseItemCatalogue.Instance.GetItemFromCatalogue(BitSupplierId, bitItemId);
            if (getItem == null)
            {
                Debug.LogError("[AddItemToSupplier] Item Added must exist in Base Catalogue");
                return;
            }
            _activeItemsData.Add(bitItemId,getItem);
        }
        
        public void RemoveItemFromSupplier(int item)
        {
            if ((_mActiveItems & item) == 0)
            {
                return;
            }
            _mActiveItems &= ~item;
            
            if (!_activeItemsData.ContainsKey(item))
            {
                Debug.LogError("[RemoveItemFromSupplier] Item removed must be managed in Data DICT");
                return;
            }
            _activeItemsData.Remove(item);
        }
    }
}