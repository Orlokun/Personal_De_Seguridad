using System.Collections.Generic;

namespace DataUnits.ItemScriptableObjects
{
    public class CameraStats : ICameraStats
    {
        private readonly int _bitId;
        private readonly int _mRange;
        private readonly int _mPeopleInSight;
        private readonly int _mClarity;
        private readonly int _mPersuasiveness;

        public List<int> GetStats()
        {
            return new List<int>()
            {
                _mRange,
                _mPeopleInSight,
                _mClarity,
                _mPersuasiveness
            };
        }
        public CameraStats(int bitId, int mRange,int peopleInSight, int mClarity, int persuasiveness)
        {
            _bitId = bitId;
            _mRange = mRange;
            _mPeopleInSight = peopleInSight;
            _mClarity = mClarity;
            _mPersuasiveness = persuasiveness;
        }
        public int Id => _bitId;
        public int Range => _mRange;
        public int PeopleInSight => _mPeopleInSight;
        public int Clarity => _mClarity;
        public int Persuasiveness => _mPersuasiveness;
    }
}