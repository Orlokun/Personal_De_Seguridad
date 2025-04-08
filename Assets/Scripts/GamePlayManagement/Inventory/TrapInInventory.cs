using DataUnits.ItemScriptableObjects;
using GamePlayManagement.BitDescriptions;
using GamePlayManagement.BitDescriptions.Suppliers;

namespace GamePlayManagement.Inventory
{
    public class TrapInInventory : ITrapInInventory
    {
        private int _mAvailableCount;
        public int AvailableCount => _mAvailableCount;

        public TrapInInventory(IItemObject itemObject)
        {
            _mBaseItemData = itemObject;
        }
        
        public int ItemId => _mBaseItemData.BitId;
        public BitItemSupplier ItemSupplier => _mBaseItemData.ItemSupplier;
        public string ItemName { get; }
        
        public void AddToInventory(int amountAdded)
        {
            _mAvailableCount += amountAdded;
        }
        public BitItemType ItemType => BitItemType.TRAP_ITEM_TYPE;

        public IItemObject ItemData => _mBaseItemData;
        private IItemObject _mBaseItemData;
    }
}