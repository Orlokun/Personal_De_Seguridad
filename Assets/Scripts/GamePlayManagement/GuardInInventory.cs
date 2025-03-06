using DataUnits.ItemScriptableObjects;
using GamePlayManagement.BitDescriptions;
using GamePlayManagement.BitDescriptions.Suppliers;

namespace GamePlayManagement
{
    public class GuardInInventory : IGuardInInventory
    {
        private int _mAvailableCount;
        public int AvailableCount => _mAvailableCount;
        
        public int ItemId => _mBaseItemData.BitId;
        public BitItemSupplier ItemSupplier => _mBaseItemData.ItemSupplier;
        public string ItemName { get; }

        public GuardInInventory(IItemObject itemObject)
        {
            _mBaseItemData = itemObject;
        }
        
        public void AddToInventory(int amountAdded)
        {
            _mAvailableCount += amountAdded;
        }
        public BitItemType ItemType => BitItemType.GUARD_ITEM_TYPE;
        public IItemObject ItemData => _mBaseItemData;
        private IItemObject _mBaseItemData;
    }
}