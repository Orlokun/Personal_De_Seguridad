using System.Collections.Generic;
using DataUnits.ItemScriptableObjects;
using DataUnits.ItemSources;
using GamePlayManagement.BitDescriptions;
using GamePlayManagement.BitDescriptions.Suppliers;

namespace GamePlayManagement.ProfileDataModules.ItemSuppliers.Stores
{
    public interface IItemSupplierShop
    {
        public void SaveTempLastStock();
        public int GetItemTempLiveStock(int itemId);
        void UnlockItem(int item);
        public bool IsItemManaged(int bitItemId);
        public BitItemSupplier BitSupplierId { get; }
        public IItemObject GetItemObject(int bitItemId);
        public IItemSupplierDataObject GetSupplierData { get; }
        public List<IItemObject> GetItemsOfType(BitItemType itemType);
        public List<IItemSupplierObjectInInventory> GetAllSupplierItems { get; }
        public IItemSupplierObjectInInventory GetSupplierItemInStore(int itemId);
        public void AddItemToCart(int id);
        void ConfirmPurchase();
    }
}