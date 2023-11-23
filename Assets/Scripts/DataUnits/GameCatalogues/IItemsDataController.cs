using System.Collections.Generic;
using DataUnits.ItemScriptableObjects;
using GamePlayManagement.BitDescriptions.Suppliers;
using UI;

namespace DataUnits.GameCatalogues
{
    public interface IItemsDataController
    { 
        public Dictionary<BitItemSupplier, List<IItemObject>> ExistingItemsInCatalogue { get; }
        public IItemObject GetItemFromCatalogue(BitItemSupplier itemSupplier, int itemBitId);
        public IItemTypeStats GetItemStats(BitItemSupplier itemSupplier, BitItemType itemType, int itemBitId);
        public IGuardStats GetStatsForGuard(BitItemSupplier itemSupplier, int itemBitId);
        public ICameraStats GetStatsForCamera(BitItemSupplier itemSupplier, int itemBitId);
        public IWeaponStats GetStatsForWeapon(BitItemSupplier itemSupplier, int itemBitId);
        public ITrapStats GetStatsForTrap(BitItemSupplier itemSupplier, int itemBitId);
        public IOtherItemsStats GetStatsForOtherItemsType(BitItemSupplier itemSupplier, int itemBitId);
    }
}