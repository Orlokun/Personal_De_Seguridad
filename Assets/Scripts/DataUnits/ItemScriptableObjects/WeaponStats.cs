using System.Collections.Generic;

namespace DataUnits.ItemScriptableObjects
{
    public class WeaponStats : IWeaponStats
    {
        private readonly int _bitId;
        private readonly int _mWeaponQuality;
        private readonly int _mDamage;
        private readonly int _mRange;
        private readonly int _mPersuasiveness;
        private readonly int _mPrecision;
        
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
        public WeaponStats(int bitId, int weaponQuality ,int damage, int mRange, int persuasiveness, int precision)
        {
            _bitId = bitId;
            _mWeaponQuality = weaponQuality;
            _mDamage = damage;
            _mRange = mRange;
            _mPersuasiveness = persuasiveness;
            _mPrecision = _mPrecision;
        }

        public int Id => _bitId;
        public int WeaponType => _mWeaponQuality;
        public int Damage => _mDamage;
        public int Range => _mRange;
        public int Persuasiveness => _mPersuasiveness;
        public int Precision => _mPrecision;
    }
}