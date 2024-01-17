namespace DataUnits.ItemScriptableObjects
{
    public interface IWeaponStats : IItemTypeStats
    {
        public int Id { get; }
        public int WeaponType { get; }
        public int Damage { get; }
        public int Range { get; }
        public int Persuasiveness { get; }
        public int Precision { get; }
    }
}