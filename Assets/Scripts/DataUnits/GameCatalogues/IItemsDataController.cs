using System.Collections.Generic;
using DataUnits.ItemScriptableObjects;
using GamePlayManagement.BitDescriptions;
using GamePlayManagement.BitDescriptions.Suppliers;

namespace DataUnits.GameCatalogues
{
    public interface IItemsDataController
    {
        public bool AreSpecialStatsReady { get; }
        public Dictionary<BitItemSupplier, List<IItemObject>> ExistingBaseItemsInCatalogue { get; }
        public List<IItemObject> GetAllSupplierItems(BitItemSupplier suppliers);
        public IItemObject GetItemFromBaseCatalogue(BitItemSupplier itemSupplier, int itemBitId);
        public IItemTypeStats GetItemStats(BitItemSupplier itemSupplier, BitItemType itemType, int itemBitId);
        public IGuardBaseData GetStatsForGuard(BitItemSupplier itemSupplier, int itemBitId);
        public ICameraBaseData GetStatsForCamera(BitItemSupplier itemSupplier, int itemBitId);
        public IWeaponBaseData GetStatsForWeapon(BitItemSupplier itemSupplier, int itemBitId);
        public ITrapBaseData GetStatsForTrap(BitItemSupplier itemSupplier, int itemBitId);
        public IOtherItemBaseData GetStatsForOtherItemsType(BitItemSupplier itemSupplier, int itemBitId);
    }
}