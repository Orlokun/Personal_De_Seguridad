using System.Collections.Generic;

namespace DataUnits.ItemScriptableObjects
{
    public class WeaponStats : IWeaponStats
    {
        private readonly int _bitId;
        private readonly int _mWeaponType;
        private readonly int _mDamage;
        private readonly int _mRange;
        private readonly int _mPersuasiveness;
        
        public List<int> GetStats()
        {
            throw new System.NotImplementedException();
        }
        public WeaponStats(int bitId, int weaponType ,int damage, int mRange, int persuasiveness)
        {
            _bitId = bitId;
            _mWeaponType = weaponType;
            _mDamage = damage;
            _mRange = mRange;
            _mPersuasiveness = persuasiveness;
        }

        public int Id => _bitId;
        public int WeaponType => _mWeaponType;
        public int Damage => _mDamage;
        public int Range => _mRange;
        public int Persuasiveness => _mPersuasiveness;
    }
}