using System;
using GamePlayManagement.LevelManagement.LevelObjectsManagement;

namespace Players_NPC.NPC_Management.Customer_Management.CustomerInterfaces
{
    public interface ICustomerPurchaseStealData
    {
        public void StealProduct(Guid id, IStoreProductObjectData stolenProduct);
        public void PurchaseProduct(Guid id, IStoreProductObjectData purchasedProduct);

        public int StolenProductsValue { get; }
        public int PurchasedProductsValue { get; }
        public void ClearAllProducts();

    }
}