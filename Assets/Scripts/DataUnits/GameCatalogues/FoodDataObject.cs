using DataUnits.ItemSources;

namespace DataUnits.GameCatalogues
{
    public class FoodDataObject : IFoodDataObject
    {
        private FoodTypesId _mFoodId;
        private string _mFoodName;
        private int _mFoodPrice;
        private int _mUnlockLevel;
        private int _mSpecialCondition;
        private int _mFoodEnergyBonus;
        private int _mFoodSanityBonus;
        public FoodDataObject(FoodTypesId mFoodId, string mFoodName, int mFoodPrice, int mUnlockLevel, int mSpecialCondition, 
            int mFoodEnergyBonus, int mFoodSanityBonus)
        {
            _mFoodName = mFoodName;
            _mFoodPrice = mFoodPrice;
            _mUnlockLevel = mUnlockLevel;
            _mSpecialCondition = mSpecialCondition;
            _mFoodEnergyBonus = mFoodEnergyBonus;
            _mFoodSanityBonus = mFoodSanityBonus;
            _mFoodId = mFoodId;
        }
        public FoodTypesId FoodId => _mFoodId;
        public string GetFoodName => _mFoodName;
        public int GetFoodPrice => _mFoodPrice;
        public int GetUnlockLevel => _mUnlockLevel;
        public int GetSpecialCondition => _mSpecialCondition;
        public int GetFoodEnergyBonus => _mFoodEnergyBonus;
        public int GetFoodSanityBonus => _mFoodSanityBonus;
    }
}