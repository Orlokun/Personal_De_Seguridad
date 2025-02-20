using DialogueSystem;
using DialogueSystem.Units;
using GamePlayManagement.BitDescriptions.RequestParameters;

namespace DataUnits.GameRequests
{
    public class GameRequest : IGameRequest
    {
        protected readonly IGameRequestData MRequestData;
        public GameRequest(int requesterSpeakId, int reqId, string reqTitle, string reqDescription, 
            RequirementActionType mChallengeType, RequirementObjectType objectTypeRequired ,RequirementLogicEvaluator mReqLogic, 
            RequirementConsideredParameter mReqParameterType, int quantity, string[] rewards, string[] penalties)
        {
            MRequestData = new GameRequestData(requesterSpeakId, reqId, reqTitle, reqDescription, 
                mChallengeType, objectTypeRequired, mReqLogic, mReqParameterType, quantity, rewards, penalties);
        }

        public DialogueSpeakerId RequesterSpeakerId => MRequestData.RequesterSpeakerId;
        public string ReqTitle => MRequestData.ReqTitle;
        public string ReqDescription => MRequestData.ReqDescription;
        public int RequestId => MRequestData.RequestId;
        public bool IsCompleted => MRequestData.IsCompleted;
        public void MarkAsCompleted()
        {
            MRequestData.CompleteChallenge();
        }
        public RequirementActionType ChallengeActionType => MRequestData.ChallengeType;
    }
}