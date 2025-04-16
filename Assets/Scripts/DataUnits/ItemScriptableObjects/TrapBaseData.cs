using System.Collections.Generic;
using GamePlayManagement.BitDescriptions.Suppliers;

namespace DataUnits.ItemScriptableObjects
{
    public class TrapBaseData : ITrapBaseData
    {
        private readonly int _bitId;
        private readonly int _mEffectiveness;
        private readonly int _mDamage;
        private readonly int _mRange;
        private readonly int _mPersuasiveness;
        private readonly List<ItemBaseRace> _mItemTypes;
        private readonly ItemOrigin _mItemOrigin;
        private readonly ItemBaseQuality _mItemBaseQuality;
        private readonly BitItemSupplier _mItemSupplier;

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

        public TrapBaseData(BitItemSupplier itemSupplier, int bitId, int mEffectiveness, int mDamage, int mRange, int mPersuasiveness, ItemOrigin itemOrigin, int itemTypes, ItemBaseQuality itemBaseQuality)
        {
            _mItemSupplier = itemSupplier;
            _bitId = bitId;
            _mEffectiveness = mEffectiveness;
            _mDamage = mDamage;
            _mRange = mRange;
            _mPersuasiveness = mPersuasiveness;
        }

        public BitItemSupplier ItemSupplier => _mItemSupplier;
        public int Id =>_bitId;
        public int Effectiveness =>_mEffectiveness;
        public int Damage =>_mDamage;
        public int Range =>_mRange;
        public int Persuasiveness=>_mPersuasiveness;
        public ItemOrigin ItemOrigin => _mItemOrigin;
        public List<ItemBaseRace> ItemTypes => _mItemTypes;
        public ItemBaseQuality ItemBaseQuality => _mItemBaseQuality;
    }
}