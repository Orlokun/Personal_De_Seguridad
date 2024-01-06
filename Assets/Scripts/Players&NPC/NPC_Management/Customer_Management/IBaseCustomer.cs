using System;
using System.Collections.Generic;
using System.Linq;
using GameDirection.GeneralLevelManager;
using GamePlayManagement.LevelManagement.LevelObjectsManagement;

namespace Players_NPC.NPC_Management.Customer_Management
{
    public interface IBaseCustomer
    {
        public ICustomerTypeData CustomerTypeData { get; }
        public Guid CustomerId { get; }
        public void SetInitialMovementData(IStoreEntrancePosition entranceData);
        public void SetCustomerTypeData(ICustomerTypeData customerTypeData);
        public ICustomerPurchaseStealData GetCustomerVisitData { get; }
    }

    public interface ICustomerPurchaseStealData
    {
        public void StealProduct(Guid id, IStoreProductObjectData stolenProduct);
        public void PurchaseProduct(Guid id, IStoreProductObjectData purchasedProduct);

        public int StolenProductsValue { get; }
        public int PurchasedProductsValue { get; }
        public void ClearAllProducts();

    }
    
    public class CustomerPurchaseStealData : ICustomerPurchaseStealData
    {
        private Dictionary<Guid, IStoreProductObjectData> _mStolenProducts = new Dictionary<Guid, IStoreProductObjectData>();
        private Dictionary<Guid, IStoreProductObjectData> _mPurchasedProducts = new Dictionary<Guid, IStoreProductObjectData>();

        public void StealProduct(Guid id, IStoreProductObjectData stolenProduct)
        {
            
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
            _mStolenProducts.Clear();
        }

        public void ClearAllProducts()
        {
            ClearAllStolenProducts();
            ClearAllPurchasedProducts();
        }
    }
}