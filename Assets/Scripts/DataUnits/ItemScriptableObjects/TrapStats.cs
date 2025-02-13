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
        private readonly int _mItemTypes;
        private readonly ItemOrigin _mItemOrigin;
        private readonly ItemBaseQuality _mItemBaseQuality;
        
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

        public TrapStats(int bitId, int mEffectiveness, int mDamage, int mRange, int mPersuasiveness, ItemOrigin itemOrigin, int itemTypes, ItemBaseQuality itemBaseQuality)
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
        public ItemOrigin ItemOrigin => _mItemOrigin;
        public int ItemTypes => _mItemTypes;
        public ItemBaseQuality ItemBaseQuality => _mItemBaseQuality;
    }

    public class OtherItemsStats : IOtherItemsStats
    {
        private readonly int _bitId;
        private readonly int _mEffectiveness;
        private readonly int _mDamage;
        private readonly int _mRange;
        private readonly int _mPersuasiveness;
        private readonly int _mItemTypes;
        private readonly ItemOrigin _mItemOrigin;
        private readonly ItemBaseQuality _mItemBaseQuality;

        
        public OtherItemsStats(int bitId, int mEffectiveness, int mDamage, int mRange, int mPersuasiveness,  ItemOrigin itemOrigin, int itemTypes, ItemBaseQuality itemBaseQuality)
        {
            _bitId = bitId;
            _mEffectiveness = mEffectiveness;
            _mDamage = mDamage;
            _mRange = mRange;
            _mPersuasiveness = mPersuasiveness;
            _mItemOrigin = itemOrigin;
            _mItemTypes = itemTypes;
            _mItemBaseQuality = itemBaseQuality;
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
        public ItemOrigin ItemOrigin => _mItemOrigin;
        public int ItemTypes => _mItemTypes;
        public ItemBaseQuality ItemBaseQuality => _mItemBaseQuality;
    }
}