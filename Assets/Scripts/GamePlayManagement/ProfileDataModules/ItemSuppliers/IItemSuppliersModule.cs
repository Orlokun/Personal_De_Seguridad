using System.Collections.Generic;
using DataUnits.ItemScriptableObjects;
using GamePlayManagement.BitDescriptions;
using GamePlayManagement.BitDescriptions.Suppliers;

namespace GamePlayManagement.ProfileDataModules.ItemSuppliers
{
    public interface IItemSuppliersModule : IProfileModule, IItemSuppliersModuleData
    {

        //Suppliers API
        public bool IsSupplierActive(BitItemSupplier provider);
        public void UnlockSupplier(BitItemSupplier provider);
        public void RemoveSupplier(BitItemSupplier provider);
        
        //Items API
        public bool IsItemSupplied(BitItemSupplier supplier, int itemBitId);
        public void UnlockItemInSupplier(BitItemSupplier supplier, int itemBitId);
        public IItemObject GetItemObject(BitItemSupplier supplier, int itemBitId);
        public List<IItemObject> GetItemsOfType(BitItemType itemType);
    }
}