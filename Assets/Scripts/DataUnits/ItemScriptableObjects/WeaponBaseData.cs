using System.Collections.Generic;
using GamePlayManagement.BitDescriptions.Suppliers;

namespace DataUnits.ItemScriptableObjects
{
    public class WeaponBaseData : IWeaponBaseData
    {
        private readonly int _bitId;
        private readonly int _mWeaponQuality;
        private readonly int _mDamage;
        private readonly int _mRange;
        private readonly int _mPersuasiveness;
        private readonly int _mPrecision;
        private readonly List<ItemBaseRace> _mItemTypes;
        private readonly ItemOrigin _mItemOrigin;
        private readonly ItemBaseQuality _mItemBaseQuality;
        private readonly BitItemSupplier _mItemSupplier;

        public List<int> GetStats()
        {
            return new List<int>()
            {
                _bitId,
                _mWeaponQuality,
                _mDamage,
                _mRange,
                _mPersuasiveness,
                _mPrecision,
            };
        }
        public WeaponBaseData(BitItemSupplier itemSupplier, int bitId, int weaponQuality ,int damage, int mRange, int persuasiveness, int precision, ItemOrigin itemOrigin, List<ItemBaseRace> itemTypes, ItemBaseQuality itemBaseQuality)
        {
            _mItemSupplier = itemSupplier;
            _bitId = bitId;
            _mWeaponQuality = weaponQuality;
            _mDamage = damage;
            _mRange = mRange;
            _mPersuasiveness = persuasiveness;
            _mPrecision = precision;
            _mItemOrigin = itemOrigin;
            _mItemTypes = itemTypes;
            _mItemBaseQuality = itemBaseQuality;
        }

        public BitItemSupplier ItemSupplier => _mItemSupplier;
        public int Id => _bitId;
        public int WeaponType => _mWeaponQuality;
        public int Damage => _mDamage;
        public int Range => _mRange;
        public int Persuasiveness => _mPersuasiveness;
        public int Precision => _mPrecision;
        public ItemOrigin ItemOrigin => _mItemOrigin;
        public List<ItemBaseRace> ItemTypes => _mItemTypes;
        public ItemBaseQuality ItemBaseQuality => _mItemBaseQuality;
    }
}