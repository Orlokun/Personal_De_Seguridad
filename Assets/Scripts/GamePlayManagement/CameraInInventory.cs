using DataUnits.ItemScriptableObjects;
using GamePlayManagement.BitDescriptions.Suppliers;

namespace GamePlayManagement
{
    public class CameraInInventory : ICameraInInventory
    {
        private int _mAvailableCount;
        public int AvailableCount => _mAvailableCount;

        
        public int ItemId => _mBaseItemData.Id;
        public BitItemSupplier ItemSupplier => _mBaseItemData.ItemSupplier;
        public string ItemName { get; }
        
        public void AddToInventory(int amountAdded)
        {
            _mAvailableCount += amountAdded;
        }

        private ICameraBaseData _mBaseItemData;
    }
}