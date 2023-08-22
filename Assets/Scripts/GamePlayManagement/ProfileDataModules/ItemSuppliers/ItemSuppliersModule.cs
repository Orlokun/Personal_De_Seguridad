using System.Collections.Generic;
using DataUnits.GameCatalogues;
using DataUnits.ItemScriptableObjects;
using GamePlayManagement.BitDescriptions.Suppliers;
using GamePlayManagement.ProfileDataModules.ItemSuppliers.Stores;
using UnityEngine;
using Utils;

namespace GamePlayManagement.ProfileDataModules.ItemSuppliers
{
    public class ItemSuppliersModule : IItemSuppliersModule
    {
        private int _mActiveProviders;
        private IBaseItemDataCatalogue _mItemDataCatalogue;
        private IBaseItemSuppliersCatalogue _mItemSuppliersCatalogue;
        private Dictionary<int, IItemSupplierShop> _activeProviders = new Dictionary<int, IItemSupplierShop>();
        public Dictionary<int, IItemSupplierShop> ActiveProviderObjects => _activeProviders;

        public int AllActiveSuppliers => _mActiveProviders;

        public ItemSuppliersModule(IBaseItemDataCatalogue itemDataCatalogue, IBaseItemSuppliersCatalogue itemSuppliersDataCatalogue)
        {
            _mItemDataCatalogue = itemDataCatalogue;
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
            return _activeProviders[(int) supplier].IsItemManaged(itemBitId);
        }
        public void AddItemToSupplier(BitItemSupplier supplier, int itemBitId)
        {
            var castSupplier = (int) supplier;
            if ((castSupplier & _mActiveProviders) == 0)
            {
                Debug.LogWarning("[ItemSuppliersModule.AddItemToSupplier] Item supplier must be active in module");
                return;
            }
            _activeProviders[castSupplier].AddItemToSupplier(itemBitId);
        }

        public void RemoveItemFromSupplier(BitItemSupplier supplier, int itemBitId)
        {
            if (!IsSupplierActive(supplier))
            {
                return;
            }
            _activeProviders[(int) supplier].RemoveItemFromSupplier(itemBitId);
        }

        public IItemObject GetItemObject(BitItemSupplier supplier, int itemBitId)
        {
            if (!IsSupplierActive(supplier))
            {
                return null;
            }
            return _activeProviders[(int) supplier].GetItemObject(itemBitId);
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
        public void ActivateSupplier(BitItemSupplier provider)
        {
            var castProvider = (int) provider;
            
            if ((_mActiveProviders & castProvider) != 0)
            {
                return;
            }
            _mActiveProviders |= castProvider;
            
            if (_activeProviders.ContainsKey(castProvider))
            {
                return;
            }

            var itemSupplier = Factory.CreateItemStoreSupplier(provider, _mItemDataCatalogue, _mItemSuppliersCatalogue);
            _activeProviders.Add(castProvider, itemSupplier);
        }

        public void RemoveSupplier(BitItemSupplier provider)
        {
            var castProvider = (int) provider;
            
            if ((_mActiveProviders & castProvider) == 0)
            {
                return;
            }
            
            _mActiveProviders &= ~castProvider;
            
            if (!_activeProviders.ContainsKey(castProvider))
            {
                return;
            }
            _activeProviders.Remove(castProvider);
        }

        #endregion

    }
}