namespace DataUnits.ItemScriptableObjects
{
    public interface ICameraStats : IItemTypeStats
    {
        public int Id { get; }
        public int Range { get; }
        public int PeopleInSight { get; }
        public int Clarity { get; }
        public int Persuasiveness { get; }
    }
}