using System;
using System.Collections.Generic;
using System.Linq;
using GamePlayManagement.LevelManagement.LevelObjectsManagement;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.CustomerInterfaces;

namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management
{
    public class CustomerPurchaseStealData : ICustomerPurchaseStealData
    {
        private Dictionary<Guid, IStoreProductObjectData> _mStolenProducts = new Dictionary<Guid, IStoreProductObjectData>();
        private Dictionary<Guid, IStoreProductObjectData> _mPurchasedProducts = new Dictionary<Guid, IStoreProductObjectData>();

        public void StealProduct(Guid id, IStoreProductObjectData stolenProduct)
        {
            _mStolenProducts.Add(Guid.NewGuid(), stolenProduct);
        }
        
        public void PurchaseProduct(Guid id, IStoreProductObjectData purchasedProduct)
        {
            _mPurchasedProducts.Add(Guid.NewGuid(), purchasedProduct);
        }
        public int PurchasedProductsValue => _mPurchasedProducts.Select((t, i) => _mPurchasedProducts.ElementAtOrDefault(i).Value.Price).Sum();
        public int StolenProductsValue => _mStolenProducts.Select((t, i) => _mStolenProducts.ElementAtOrDefault(i).Value.Price).Sum();
        public void ClearAllStolenProducts()
        {
            _mStolenProducts.Clear();
        }
        public void ClearAllPurchasedProducts()
        {
            _mPurchasedProducts.Clear();
        }

        public void ClearAllProducts()
        {
            ClearAllStolenProducts();
            ClearAllPurchasedProducts();
        }

        public int GetStolenProductsCount => _mStolenProducts.Count;
        public int GetPurchasedProductsCount => _mPurchasedProducts.Count;
    }
}