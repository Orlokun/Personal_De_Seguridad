using System.Collections.Generic;

namespace DataUnits.ItemScriptableObjects
{
    public class GuardStats : IGuardStats
    {
        private readonly int _bitId;
        private readonly int _mIntelligence;
        private readonly int _mKindness;
        private readonly int _mProactive;
        private readonly int _mAggressive;
        private readonly int _mStrength;
        private readonly int _mAgility;
        private readonly int _mPersuasiveness;
        private readonly int _mSpeed;
        private readonly int _mFoVRadius;
        private readonly ItemOrigin _mItemOrigin;
        private readonly int _mItemTypes;
        private readonly ItemBaseQuality _mItemBaseQuality;
        public GuardStats(int bitId, int mIntelligence,int mKindness, int mProactive, int mAggressive, int mStrength, 
            int mAgility, int mPersuasiveness, int mSpeed, int mFoVRadius, ItemOrigin itemOrigin, int itemTypes, ItemBaseQuality itemBaseQuality)
        {
            _bitId = bitId;
            _mIntelligence = mIntelligence;
            _mKindness = mKindness;
            _mProactive = mProactive;
            _mAggressive = mAggressive;
            _mStrength = mStrength;
            _mAgility = mAgility;
            _mSpeed = mSpeed;
            _mPersuasiveness = mPersuasiveness;
            _mFoVRadius = mFoVRadius;
            _mItemOrigin = itemOrigin;
            _mItemTypes = itemTypes;
            _mItemBaseQuality = itemBaseQuality;
        }

        public int Id => _bitId;
        public int Intelligence => _mIntelligence;
        public int Kindness => _mKindness;
        public int Proactive => _mProactive;
        public int Aggressive => _mAggressive;
        public int Strength => _mStrength;
        public int Agility => _mAgility;
        public int Persuasiveness => _mPersuasiveness;

        public int Speed => _mSpeed;
        public int FoVRadius => _mFoVRadius;
        public ItemOrigin ItemOrigin => _mItemOrigin;
        public int ItemTypes => _mItemTypes;
        public ItemBaseQuality ItemBaseQuality => _mItemBaseQuality;
        

        public List<int> GetStats()
        {
            return new List<int>()
            {
                _mIntelligence,
                _mKindness,
                _mProactive,
                _mAggressive,
                _mStrength,
                _mAgility
            };
        }
    }
}