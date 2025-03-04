using System.Collections.Generic;
using DataUnits.GameCatalogues;
using DataUnits.ItemScriptableObjects;
using GamePlayManagement.BitDescriptions;
using GamePlayManagement.BitDescriptions.Suppliers;
using GamePlayManagement.ProfileDataModules.ItemSuppliers.Stores;
using UI;
using UnityEngine;
using Utils;

namespace GamePlayManagement.ProfileDataModules.ItemSuppliers
{
    public class ItemSuppliersModule : IItemSuppliersModule
    {
        private IPlayerGameProfile _activePlayer;
        private int _mActiveProviders;
        private IItemsDataController _mItemDataController;
        private IBaseItemSuppliersCatalogue _mItemSuppliersCatalogue;
        
        private Dictionary<BitItemSupplier, IItemSupplierShop> _activeStores = new Dictionary<BitItemSupplier, IItemSupplierShop>();
        public Dictionary<BitItemSupplier, IItemSupplierShop> ActiveItemStores => _activeStores;

        public int UnlockedItemSuppliers => _mActiveProviders;

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
            return _activeStores[supplier].IsItemManaged(itemBitId);
        }
        public void UnlockItemInSupplier(BitItemSupplier supplier, int itemBitId)
        {
            var castSupplier = (int) supplier;
            if (!BitOperator.IsActive(_mActiveProviders,castSupplier))
            {
                Debug.LogWarning("[ItemSuppliersModule.AddItemToSupplier] Item supplier must be active in module");
                return;
            }
            _activeStores[supplier].UnlockItem(itemBitId);
        }

        public IItemObject GetItemObject(BitItemSupplier supplier, int itemBitId)
        {
            if (!IsSupplierActive(supplier))
            {
                return null;
            }
            return _activeStores[supplier].GetItemObject(itemBitId);
        }

        public List<IItemObject> GetItemsOfType(BitItemType itemType)
        {
            var itemsOfType = new List<IItemObject>();
            foreach (var activeProvider in _activeStores)
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
        public async void UnlockSupplier(BitItemSupplier provider)
        {
            Debug.Log($"[ItemSuppliersModule.UnlockSupplier] Start Download Unlocked Dialogues. Item Provider: {provider}");
            var castProvider = (int) provider;
            if(BitOperator.IsActive(_mActiveProviders, castProvider) || _activeStores.ContainsKey(provider))
            {
                return;
            }
            _mActiveProviders |= castProvider;
            var itemSupplier = BaseItemSuppliersCatalogue.Instance.GetItemSupplierData(provider);
            await itemSupplier.StartUnlockedData();
            var itemSupplierShop = Factory.CreateItemStoreSupplier(provider, _mItemDataController, _mItemSuppliersCatalogue);
            itemSupplier.InitializeStore(itemSupplierShop);
            _activeStores.Add(provider, itemSupplierShop);
            UIController.Instance.ElementUnlockedFeedback(provider);
        }

        public void RemoveSupplier(BitItemSupplier provider)
        {
            var castProvider = (int) provider;
            
            if ((_mActiveProviders & castProvider) == 0)
            {
                return;
            }
            
            _mActiveProviders &= ~castProvider;
            
            if (!_activeStores.ContainsKey(provider))
            {
                return;
            }
            _activeStores.Remove(provider);
        }

        #endregion

        public void SetProfile(IPlayerGameProfile currentPlayerProfile)
        {
            _activePlayer = currentPlayerProfile;
        }

        public void PlayerLostResetData()
        {
        }
    }
}