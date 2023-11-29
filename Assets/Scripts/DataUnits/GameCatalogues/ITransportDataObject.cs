using DataUnits.ItemSources;

namespace DataUnits.GameCatalogues
{
    public interface ITransportDataObject
    {
        public TransportTypesId TransportId { get; }
        public string GetTransportName { get; }
        public int GetTransportPrice { get; }
        public int GetUnlockLevel { get; }
        public int GetSpecialCondition { get; }
        public int GetTransportEnergyBonus { get; }
        public int GetTransportSanityBonus { get; }
    }
}