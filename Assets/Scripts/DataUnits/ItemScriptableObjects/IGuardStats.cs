namespace DataUnits.ItemScriptableObjects
{
    public interface IGuardStats : IItemTypeStats
    {
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

    public interface ITrapStats : IItemTypeStats
    {
        public int Id { get; }
        public int Effectiveness { get; }
        public int Damage { get; }
        public int Range { get; }
        public int Persuasiveness { get; }
    }
    public interface IOtherItemsStats : IItemTypeStats
    {
        public int Id { get; }
        public int Effectiveness { get; }
        public int Damage { get; }
        public int Range { get; }
        public int Persuasiveness { get; }
    }
}