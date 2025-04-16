using System.Collections.Generic;
using GamePlayManagement.BitDescriptions.Suppliers;

namespace DataUnits.ItemScriptableObjects
{
    public class CameraBaseData : ICameraBaseData
    {
        private readonly BitItemSupplier _mItemSupplier;
        private readonly int _bitId;
        private readonly int _mRange;
        private readonly int _mPeopleInSight;
        private readonly int _mClarity;
        private readonly int _mPersuasiveness;
        private readonly int _mFoVRadius;
        private readonly List<ItemBaseRace> _mItemTypes;
        private readonly ItemOrigin _mItemOrigin;
        private readonly ItemBaseQuality _mItemBaseQuality;

        public List<int> GetStats()
        {
            return new List<int>()
            {
                _mRange,
                _mPeopleInSight,
                _mClarity,
                _mPersuasiveness,
                _mFoVRadius
            };
        }

        public List<ItemBaseRace> ItemTypes => _mItemTypes;
        public ItemOrigin ItemOrigin { get; }
        public ItemBaseQuality ItemBaseQuality { get; }

        public CameraBaseData(BitItemSupplier itemSupplier, int bitId, int mRange,int peopleInSight, int mClarity, int persuasiveness, int fovRadius, ItemOrigin itemOrigin, List<ItemBaseRace> itemTypes, ItemBaseQuality itemBaseQuality)
        {
            _mItemSupplier = itemSupplier;
            _bitId = bitId;
            _mRange = mRange;
            _mPeopleInSight = peopleInSight;
            _mClarity = mClarity;
            _mPersuasiveness = persuasiveness;
            _mFoVRadius = fovRadius;
            _mItemOrigin = itemOrigin;
            _mItemTypes = itemTypes;
            _mItemBaseQuality = itemBaseQuality;
        }
        public BitItemSupplier ItemSupplier => _mItemSupplier;
        public int Id => _bitId;
        public int Range => _mRange;
        public int PeopleInSight => _mPeopleInSight;
        public int Clarity => _mClarity;
        public int Persuasiveness => _mPersuasiveness;
        public int FoVRadius => _mFoVRadius;
    }
}