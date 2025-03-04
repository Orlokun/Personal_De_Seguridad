using GamePlayManagement.BitDescriptions.Suppliers;

namespace GamePlayManagement
{
    public interface IItemInInventory
    {
        public int ItemId { get; }
        public BitItemSupplier ItemSupplier { get; }
        public string ItemName { get; }
        public int AvailableCount { get; }
        
        public void AddToInventory(int amountAdded);
    }
}