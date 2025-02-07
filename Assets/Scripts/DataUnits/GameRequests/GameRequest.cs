using DialogueSystem;
using GamePlayManagement.BitDescriptions.RequestParameters;

namespace DataUnits.GameRequests
{
    public interface IGameRequest
    {
        public DialogueSpeakerId RequesterSpeakerId { get; }
        public int RequestId { get; }
        public string ReqTitle { get; }
        public string ReqDescription { get; }
        public bool IsCompleted { get; }
        public void ProcessEvent();
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

        public DialogueSpeakerId RequesterSpeakerId => _mRequestData.RequesterSpeakerId;
        public string ReqTitle => _mRequestData.ReqTitle;
        public string ReqDescription => _mRequestData.ReqDescription;
        public int RequestId => _mRequestData.RequestId;
        public bool IsCompleted => _mRequestData.IsCompleted;
        public void ProcessEvent()
        {
            //Not implemented yet
        }
    }
}