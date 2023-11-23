using System.Collections.Generic;
using DataUnits.GameCatalogues;
using DataUnits.ItemScriptableObjects;
using GamePlayManagement.BitDescriptions.Suppliers;
using GamePlayManagement.ProfileDataModules.ItemSuppliers.Stores;
using UI;
using UnityEngine;
using Utils;

namespace GamePlayManagement.ProfileDataModules.ItemSuppliers
{
    public class ItemSuppliersModule : IItemSuppliersModule
    {
        private int _mActiveProviders;
        private IItemsDataController _mItemDataController;
        private IBaseItemSuppliersCatalogue _mItemSuppliersCatalogue;
        private Dictionary<BitItemSupplier, IItemSupplierShop> _activeProviders = new Dictionary<BitItemSupplier, IItemSupplierShop>();
        public Dictionary<BitItemSupplier, IItemSupplierShop> ActiveProviderObjects => _activeProviders;

        public int AllActiveSuppliers => _mActiveProviders;

        public ItemSuppliersModule(IItemsDataController itemDataController, IBaseItemSuppliersCatalogue itemSuppliersDataCatalogue)
        {
            _mItemDataController = itemDataController;
            _mItemSuppliersCatalogue = itemSuppliersDataCatalogue;
        }
        
        /// <summary>
        /// Items Management 
        /// </summary>
        /// <returns></returns>
        #region ItemManagement
        public List<IItemObject> GetAllAvailableItems()
        {
            return null;
        }

        public bool IsItemSupplied(BitItemSupplier supplier, int itemBitId)
        {
            if ((_mActiveProviders & (int) supplier) == 0)
            {
                Debug.LogWarning("Item supplier is not available");
                return false;
            }
            return _activeProviders[supplier].IsItemManaged(itemBitId);
        }
        public void UnlockItemInSupplier(BitItemSupplier supplier, int itemBitId)
        {
            var castSupplier = (int) supplier;
            if ((castSupplier & _mActiveProviders) == 0)
            {
                Debug.LogWarning("[ItemSuppliersModule.AddItemToSupplier] Item supplier must be active in module");
                return;
            }
            _activeProviders[supplier].AddItemToSupplier(itemBitId);
        }

        public void RemoveItemFromSupplier(BitItemSupplier supplier, int itemBitId)
        {
            if (!IsSupplierActive(supplier))
            {
                return;
            }
            _activeProviders[supplier].RemoveItemFromSupplier(itemBitId);
        }

        public IItemObject GetItemObject(BitItemSupplier supplier, int itemBitId)
        {
            if (!IsSupplierActive(supplier))
            {
                return null;
            }
            return _activeProviders[supplier].GetItemObject(itemBitId);
        }

        public List<IItemObject> GetItemsOfType(BitItemType itemType)
        {
            var itemsOfType = new List<IItemObject>();
            foreach (var activeProvider in _activeProviders)
            {
                var providerItems = activeProvider.Value.GetItemsOfType(itemType);
                itemsOfType.AddRange(providerItems);
            }
            return itemsOfType;
        }

        #endregion
        
        
        /// <summary>
        /// Providers Management Section
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        #region ProviderManagement
        public bool IsSupplierActive(BitItemSupplier provider)
        {
            var castSupplier = (int) provider;
            return (_mActiveProviders & castSupplier) != 0;
        }
        public void UnlockSupplier(BitItemSupplier provider)
        {
            var castProvider = (int) provider;
            
            if ((_mActiveProviders & castProvider) != 0)
            {
                return;
            }
            _mActiveProviders |= castProvider;
            
            if (_activeProviders.ContainsKey(provider))
            {
                return;
            }

            var itemSupplier = Factory.CreateItemStoreSupplier(provider, _mItemDataController, _mItemSuppliersCatalogue);
            _activeProviders.Add(provider, itemSupplier);
        }

        public void RemoveSupplier(BitItemSupplier provider)
        {
            var castProvider = (int) provider;
            
            if ((_mActiveProviders & castProvider) == 0)
            {
                return;
            }
            
            _mActiveProviders &= ~castProvider;
            
            if (!_activeProviders.ContainsKey(provider))
            {
                return;
            }
            _activeProviders.Remove(provider);
        }

        #endregion

    }
}