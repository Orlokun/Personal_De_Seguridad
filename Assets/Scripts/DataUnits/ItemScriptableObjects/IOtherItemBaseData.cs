using GamePlayManagement.BitDescriptions.Suppliers;

namespace DataUnits.ItemScriptableObjects
{
    public interface IOtherItemBaseData : IItemTypeStats
    {
        public BitItemSupplier ItemSupplier { get; }

        public int Id { get; }
        public int Effectiveness { get; }
        public int Damage { get; }
        public int Range { get; }
        public int Persuasiveness { get; }
    }
}