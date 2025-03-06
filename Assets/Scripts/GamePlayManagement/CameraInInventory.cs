using DataUnits.ItemScriptableObjects;
using GamePlayManagement.BitDescriptions;
using GamePlayManagement.BitDescriptions.Suppliers;

namespace GamePlayManagement
{
    public class CameraInInventory : ICameraInInventory
    {
        private int _mAvailableCount;
        public int AvailableCount => _mAvailableCount;

        public CameraInInventory(IItemObject itemObject)
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
        
        public void RemoveFromInventory(int amountRemoved)
        {
            if (_mAvailableCount == 0)
            {
                return;
            }
            _mAvailableCount -= amountRemoved;
        }

        public BitItemType ItemType => BitItemType.CAMERA_ITEM_TYPE;
        public IItemObject ItemData => _mBaseItemData;

        private IItemObject _mBaseItemData;
    }
}