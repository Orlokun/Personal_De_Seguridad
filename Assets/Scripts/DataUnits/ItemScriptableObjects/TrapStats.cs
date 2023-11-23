using System.Collections.Generic;

namespace DataUnits.ItemScriptableObjects
{
    public class TrapStats : ITrapStats
    {
        private readonly int _bitId;
        private readonly int _mEffectiveness;
        private readonly int _mDamage;
        private readonly int _mRange;
        private readonly int _mPersuasiveness;
        public List<int> GetStats()
        {
            return new List<int>()
            {
                _mEffectiveness,
                _mDamage,
                _mRange,
                _mPersuasiveness
            };
        }

        public TrapStats(int bitId, int mEffectiveness, int mDamage, int mRange, int mPersuasiveness)
        {
            _bitId = bitId;
            _mEffectiveness = mEffectiveness;
            _mDamage = mDamage;
            _mRange = mRange;
            _mPersuasiveness = mPersuasiveness;
        }

        public int Id =>_bitId;
        public int Effectiveness =>_mEffectiveness;
        public int Damage =>_mDamage;
        public int Range =>_mRange;
        public int Persuasiveness=>_mPersuasiveness;
    }

    public class OtherItemsStats : IOtherItemsStats
    {
        private readonly int _bitId;
        private readonly int _mEffectiveness;
        private readonly int _mDamage;
        private readonly int _mRange;
        private readonly int _mPersuasiveness;
        
        public OtherItemsStats(int bitId, int mEffectiveness, int mDamage, int mRange, int mPersuasiveness)
        {
            _bitId = bitId;
            _mEffectiveness = mEffectiveness;
            _mDamage = mDamage;
            _mRange = mRange;
            _mPersuasiveness = mPersuasiveness;
        }
        public List<int> GetStats()
        {
            return new List<int>()
            {
                _mEffectiveness,
                _mDamage,
                _mRange,
                _mPersuasiveness
            };
        }
        
        public int Id =>_bitId;
        public int Effectiveness =>_mEffectiveness;
        public int Damage =>_mDamage;
        public int Range =>_mRange;
        public int Persuasiveness=>_mPersuasiveness;
    }
}