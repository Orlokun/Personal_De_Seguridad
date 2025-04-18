using System.Collections.Generic;
using GamePlayManagement.BitDescriptions.Suppliers;

namespace DataUnits.ItemScriptableObjects
{
    public class GuardBaseData : IGuardBaseData
    {
        private readonly BitItemSupplier _mItemSupplier;
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
        private readonly List<ItemBaseType> _mItemTypes;
        private readonly ItemBaseQuality _mItemBaseQuality;
        public GuardBaseData(BitItemSupplier itemSupplier, int bitId, int mIntelligence,int mKindness, int mProactive, int mAggressive, int mStrength, 
            int mAgility, int mPersuasiveness, int mSpeed, int mFoVRadius, ItemOrigin itemOrigin, List<ItemBaseType> itemTypes, ItemBaseQuality itemBaseQuality)
        {
            //Unique only inside the Supplier
            _mItemSupplier = itemSupplier;
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

        public BitItemSupplier ItemSupplier => _mItemSupplier;
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
        public List<ItemBaseType> ItemTypes => _mItemTypes;
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