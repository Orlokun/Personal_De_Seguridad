using DataUnits.ItemSources;

namespace DataUnits.GameCatalogues
{
    public interface IFoodDataObject
    {
        public FoodTypesId FoodId { get; }
        public string GetFoodName { get; }
        public int GetFoodPrice { get; }
        public int GetUnlockLevel { get; }
        public int GetSpecialCondition { get; }
        public int GetFoodEnergyBonus { get; }
        public int GetFoodSanityBonus { get; }
    }
}