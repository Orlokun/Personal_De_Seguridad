using GamePlayManagement.BitDescriptions.RequestParameters;

namespace DataUnits.GameRequests
{
    public interface IGameRequest
    {
        public int RequesterSpeakerId { get; }
        public int RequestId { get; }
        public bool IsCompleted { get; }
        public void CompleteChallenge();
    }

    public class GameRequest : IGameRequest
    {
        private IGameRequestData _mRequestData;
        public GameRequest(int requesterSpeakId, int reqId, string reqTitle, string reqDescription, 
            RequirementActionType mChallengeType, RequirementObjectType objectTypeRequired ,RequirementLogicEvaluator mReqLogic, 
            RequirementConsideredParameter mReqParameterType, string[] mReqParameterValues, int quantity)
        {
            _mRequestData = new GameRequestData(requesterSpeakId, reqId, reqTitle, reqDescription, mChallengeType, objectTypeRequired, mReqLogic, mReqParameterType, mReqParameterValues, quantity);
        }

        public int RequesterSpeakerId => _mRequestData.RequesterSpeakerId;
        public int RequestId => _mRequestData.RequestId;
        public bool IsCompleted => _mRequestData.IsCompleted;
        
        public void CompleteChallenge()
        {
        }
    }
}