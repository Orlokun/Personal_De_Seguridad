using DataUnits.ItemScriptableObjects;
using GamePlayManagement.BitDescriptions.Suppliers;

namespace GamePlayManagement.ProfileDataModules.ItemSuppliers
{
    public interface IStoreSuppliersModule : IProfileModule
    {
        public int ActiveItemSuppliers { get; }
        public bool IsSupplierActive(BitItemSupplier itemSupplier);
    }
    
    public interface IItemSuppliersModule : IProfileModule
    {
        public int AllActiveSuppliers { get; }
        //Suppliers API
        public bool IsSupplierActive(BitItemSupplier provider);
        public void ActivateSupplier(BitItemSupplier provider);
        public void RemoveSupplier(BitItemSupplier provider);
        
        //Items API
        public bool IsItemSupplied(BitItemSupplier supplier, int itemBitId);
        public void AddItemToSupplier(BitItemSupplier supplier, int itemBitId);
        public void RemoveItemFromSupplier(BitItemSupplier supplier, int itemBitId);
        public IItemObject GetItemObject(BitItemSupplier supplier, int itemBitId);

    }
}