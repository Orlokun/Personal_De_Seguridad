using DataUnits.ItemSources;

namespace DataUnits.GameCatalogues
{
    public class TransportDataObject : ITransportDataObject
    {
        private TransportTypesId _mTransportId;
        private string _mTransportName;
        private int _mTransportPrice;
        private int _mUnlockLevel;
        private int _mSpecialCondition;
        private int _mTransportEnergyBonus;
        private int _mTransportSanityBonus;
        public TransportDataObject(TransportTypesId mTransportId, string mTransportName, int mTransportPrice, int mUnlockLevel, int mSpecialCondition, 
            int mTransportEnergyBonus, int mTransportSanityBonus)
        {
            _mTransportName = mTransportName;
            _mTransportPrice = mTransportPrice;
            _mUnlockLevel = mUnlockLevel;
            _mSpecialCondition = mSpecialCondition;
            _mTransportEnergyBonus = mTransportEnergyBonus;
            _mTransportSanityBonus = mTransportSanityBonus;
            _mTransportId = mTransportId;
        }
        public TransportTypesId TransportId => _mTransportId;
        public string GetTransportName => _mTransportName;
        public int GetTransportPrice => _mTransportPrice;
        public int GetUnlockLevel => _mUnlockLevel;
        public int GetSpecialCondition => _mSpecialCondition;
        public int GetTransportEnergyBonus => _mTransportEnergyBonus;
        public int GetTransportSanityBonus => _mTransportSanityBonus;
    }
}