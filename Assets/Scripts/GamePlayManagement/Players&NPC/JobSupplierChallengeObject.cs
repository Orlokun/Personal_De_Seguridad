using DialogueSystem;
using GamePlayManagement.BitDescriptions.SupplierChallenges;
using GamePlayManagement.BitDescriptions.Suppliers;

namespace GamePlayManagement.Players_NPC
{
    public class JobSupplierChallengeObject : IJobSupplierChallengeObject
    {
        private JobSupplierBitId _mJobSupplierBitId;
        private RequestChallengeType _mChallengeType;
        private RequestChallengeLogicOperator _mChallengeLogic;
        private ConsideredParameter _mChallengeParameterType;
        private int _mChallengeParameterValue;

        private bool _mIsCompleted;

        public JobSupplierChallengeObject(JobSupplierBitId mJobSupplierBitId, RequestChallengeType mChallengeType, RequestChallengeLogicOperator mChallengeLogic, ConsideredParameter mChallengeParameterType, int mChallengeParameterValue)
        {
            _mJobSupplierBitId = mJobSupplierBitId;
            _mChallengeType = mChallengeType;
            _mChallengeLogic = mChallengeLogic;
            _mChallengeParameterType = mChallengeParameterType;
            _mChallengeParameterValue = mChallengeParameterValue;
            _mIsCompleted = false;
        }

        public bool IsCompleted { get; }
        
        public void CompleteChallenge()
        {
            _mIsCompleted = true;
        }
    }
}