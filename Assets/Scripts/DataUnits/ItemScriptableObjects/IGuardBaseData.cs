using GamePlayManagement.BitDescriptions.Suppliers;

namespace DataUnits.ItemScriptableObjects
{
    public interface IGuardBaseData : IItemTypeStats
    {
        public BitItemSupplier ItemSupplier { get; }
        public int Id { get; }
        public int Intelligence { get; }
        public int Kindness { get; }
        public int Proactive { get; }
        public int Aggressive { get; }
        public int Strength { get; }
        public int Agility { get; }
        public int Persuasiveness { get; }
        public int Speed { get; }
        public int FoVRadius { get; }
    }
}