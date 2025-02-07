using DialogueSystem;
using GamePlayManagement.BitDescriptions.RequestParameters;

namespace DataUnits.GameRequests
{
    public interface IGameRequestData
    {
        public DialogueSpeakerId RequesterSpeakerId{ get; }
        public int RequestId{ get; }
        public string ReqTitle{ get; }
        public string ReqDescription { get; }
        
        public RequirementActionType ChallengeType  { get; }
        public RequirementObjectType ChallengeObjectType  { get; }
        public RequirementLogicEvaluator ReqLogic { get; }
        public RequirementConsideredParameter ReqParameterType  { get; }
        bool IsCompleted { get; }
    }
    public class GameRequestData : IGameRequestData
    {
        private DialogueSpeakerId _mRequesterSpeakerId;
        private int _mRequestId;
        private string _mReqTitle;
        private string _mReqDescription;
        
        private RequirementActionType _mChallengeType;
        private RequirementObjectType _mChallengeObjectType;
        private RequirementLogicEvaluator _mReqLogic;
        private RequirementConsideredParameter _mReqParameterType;
        private string[] _mReqParameterValue;
        private bool _mIsCompleted;

        public GameRequestData(int mRequesterSpeakerId, int mRequestId, string mReqTitle, string mReqDescription,
            RequirementActionType mChallengeType, RequirementObjectType mChallengeObjectType, RequirementLogicEvaluator mReqLogic,
            RequirementConsideredParameter mReqParameterType, string[] mReqParameterValue, int quantity)
        {
            _mRequesterSpeakerId = (DialogueSpeakerId)mRequesterSpeakerId;
            _mRequestId = mRequestId;
            _mReqTitle = mReqTitle;
            _mReqDescription = mReqDescription;
            _mChallengeType = mChallengeType;
            _mChallengeObjectType = mChallengeObjectType;
            _mReqLogic = mReqLogic;
            _mReqParameterType = mReqParameterType;
            _mReqParameterValue = mReqParameterValue;
        }

        public DialogueSpeakerId RequesterSpeakerId => _mRequesterSpeakerId;
        public int RequestId => _mRequestId;
        public string ReqTitle => _mReqTitle;
        public string ReqDescription => _mReqDescription;
        
        public RequirementActionType ChallengeType => _mChallengeType;
        public RequirementObjectType ChallengeObjectType => _mChallengeObjectType;
        public RequirementLogicEvaluator ReqLogic => _mReqLogic;
        public RequirementConsideredParameter ReqParameterType => _mReqParameterType;
        public bool IsCompleted => _mIsCompleted;
    }
}