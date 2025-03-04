using DataUnits.ItemScriptableObjects;
using GamePlayManagement.BitDescriptions;
using GamePlayManagement.BitDescriptions.Suppliers;
using GamePlayManagement.ProfileDataModules;

namespace GamePlayManagement
{
    public interface IPlayerInventoryModule : IProfileModule
    {
        public int GetItemCount(BitItemSupplier itemSupplier, int itemId);
        public bool IsItemInInventory(BitItemSupplier itemSupplier, int itemId, BitItemType itemType);
        public void AddItemToInventory(IItemObject incomingItem, int amountAdded);
    }
}