using System.Collections.Generic;
using GamePlayManagement.BitDescriptions.Suppliers;
using GamePlayManagement.ProfileDataModules.ItemSuppliers.Stores;

namespace GamePlayManagement.ProfileDataModules.ItemSuppliers
{
    public interface IItemSuppliersModuleData
    {
        public int UnlockedItemSuppliers { get; }
        public Dictionary<BitItemSupplier, IItemSupplierShop> ActiveItemStores { get; }
    }
}