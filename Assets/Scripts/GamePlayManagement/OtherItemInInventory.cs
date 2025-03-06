using DataUnits.ItemScriptableObjects;
using GamePlayManagement.BitDescriptions.Suppliers;

namespace GamePlayManagement
{
    public class OtherItemInInventory : IOtherItemInInventory
    {
        private int _mAvailableCount;
        public int AvailableCount => _mAvailableCount;
        public int ItemId => _mBaseItemData.BitId;
        public BitItemSupplier ItemSupplier => _mBaseItemData.ItemSupplier;
        public string ItemName => _mBaseItemData.ItemName;

        public OtherItemInInventory(IItemObject itemObject)
        {
            _mBaseItemData = itemObject;
        }
        
        public void AddToInventory(int amountAdded)
        {
            _mAvailableCount += amountAdded;
        }

        private IItemObject _mBaseItemData;
    }
}