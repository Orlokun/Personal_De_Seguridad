using DataUnits.ItemScriptableObjects;
using GamePlayManagement.BitDescriptions;

namespace GamePlayManagement.ProfileDataModules.ItemSuppliers.Stores
{
    public interface IItemSupplierObjectInInventory
    {
        BitItemType GetItemType { get; }
        IItemObject GetItemData { get; }
        int GetCurrentAmount { get; }
        void ReplenishStock();
        void ReduceStock(int amount);
        bool IsLocked { get; }
        void Unlock();
        void Lock();
    }
}