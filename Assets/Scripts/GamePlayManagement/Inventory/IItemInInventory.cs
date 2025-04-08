using DataUnits.ItemScriptableObjects;
using GamePlayManagement.BitDescriptions;
using GamePlayManagement.BitDescriptions.Suppliers;

namespace GamePlayManagement.Inventory
{
    public interface IItemInInventory
    {
        public int ItemId { get; }
        public BitItemSupplier ItemSupplier { get; }
        public string ItemName { get; }
        public int AvailableCount { get; }
        public void AddToInventory(int amountAdded);
        public BitItemType ItemType { get; }
        public IItemObject ItemData { get; }
    }
}