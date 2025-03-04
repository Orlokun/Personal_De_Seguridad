using GamePlayManagement.BitDescriptions.Suppliers;

namespace DataUnits.ItemScriptableObjects
{
    public interface IWeaponBaseData : IItemTypeStats
    {
        public BitItemSupplier ItemSupplier { get; }
        public int Id { get; }
        public int WeaponType { get; }
        public int Damage { get; }
        public int Range { get; }
        public int Persuasiveness { get; }
        public int Precision { get; }
    }
}