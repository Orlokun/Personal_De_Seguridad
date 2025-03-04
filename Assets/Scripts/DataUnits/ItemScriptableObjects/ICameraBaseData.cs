using GamePlayManagement.BitDescriptions.Suppliers;

namespace DataUnits.ItemScriptableObjects
{
    public interface ICameraBaseData : IItemTypeStats
    {
        public BitItemSupplier ItemSupplier { get; }
        public int Id { get; }
        public int Range { get; }
        public int PeopleInSight { get; }
        public int Clarity { get; }
        public int Persuasiveness { get; }
        public int FoVRadius { get; }
    }
}