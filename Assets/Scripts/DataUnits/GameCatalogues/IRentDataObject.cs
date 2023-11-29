using DataUnits.ItemSources;

namespace DataUnits.GameCatalogues
{
    public interface IRentDataObject
    {
        public RentTypesId RentId { get; }
        public string GetRentName { get; }
        public int GetRentPrice { get; }
        public int GetUnlockLevel { get; }
        public int GetSpecialCondition { get; }
        public int GetRentEnergyBonus { get; }
        public int GetRentSanityBonus { get; }
    }
}