using DataUnits.ItemScriptableObjects;
using GamePlayManagement.BitDescriptions;
using GamePlayManagement.BitDescriptions.Suppliers;

namespace GamePlayManagement.Inventory
{
    public class OtherItemInInventory : IOtherItemInInventory
    {
        private int _mAvailableCount;
        public int AvailableCount => _mAvailableCount;
        public int ItemId => _mBaseItemData.BitId;
        public BitItemSupplier ItemSupplier => _mBaseItemData.ItemSupplier;
        public string ItemName => _mBaseItemData.ItemName;
        public BitItemType ItemType => BitItemType.SPECIAL_ITEM_TYPE;

        public OtherItemInInventory(IItemObject itemObject)
        {
            _mBaseItemData = itemObject;
        }
        
        public void AddToInventory(int amountAdded)
        {
            _mAvailableCount += amountAdded;
        }
        public IItemObject ItemData => _mBaseItemData;
        private IItemObject _mBaseItemData;
    }
}