using DataUnits.ItemSources;

namespace DataUnits.GameCatalogues
{
    public class RentDataObject : IRentDataObject
    {
        private RentTypesId _mRentId;
        private string _mRentName;
        private int _mRentPrice;
        private int _mUnlockLevel;
        private int _mSpecialCondition;
        private int _mRentEnergyBonus;
        private int _mRentSanityBonus;
        public RentDataObject(RentTypesId mRentId, string mRentName, int mRentPrice, int mUnlockLevel, int mSpecialCondition, 
            int mRentEnergyBonus, int mRentSanityBonus)
        {
            _mRentName = mRentName;
            _mRentPrice = mRentPrice;
            _mUnlockLevel = mUnlockLevel;
            _mSpecialCondition = mSpecialCondition;
            _mRentEnergyBonus = mRentEnergyBonus;
            _mRentSanityBonus = mRentSanityBonus;
            _mRentId = mRentId;
        }
        public RentTypesId RentId => _mRentId;
        public string GetRentName => _mRentName;
        public int GetRentPrice => _mRentPrice;
        public int GetUnlockLevel => _mUnlockLevel;
        public int GetSpecialCondition => _mSpecialCondition;
        public int GetRentEnergyBonus => _mRentEnergyBonus;
        public int GetRentSanityBonus => _mRentSanityBonus;
    }
}