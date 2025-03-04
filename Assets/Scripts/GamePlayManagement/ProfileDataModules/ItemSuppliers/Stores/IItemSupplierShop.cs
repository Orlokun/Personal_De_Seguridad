using System.Collections.Generic;
using DataUnits.ItemScriptableObjects;
using DataUnits.ItemSources;
using GamePlayManagement.BitDescriptions;
using GamePlayManagement.BitDescriptions.Suppliers;
using NUnit.Framework;

namespace GamePlayManagement.ProfileDataModules.ItemSuppliers.Stores
{
    public interface IItemSupplierShop
    {
        void UnlockItem(int item);
        public bool IsItemManaged(int bitItemId);
        public BitItemSupplier BitSupplierId { get; }
        public IItemObject GetItemObject(int bitItemId);
        public IItemSupplierDataObject GetSupplierData { get; }
        public List<IItemObject> GetItemsOfType(BitItemType itemType);
        public List<IItemSupplierObjectInInventory> GetAllSupplierItems { get; }

    }
}