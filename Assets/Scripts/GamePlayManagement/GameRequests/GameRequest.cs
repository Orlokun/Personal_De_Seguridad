using System.Collections.Generic;
using DialogueSystem.Units;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement.BitDescriptions.RequestParameters;
using GamePlayManagement.GameRequests.RewardsPenalties;

namespace GamePlayManagement.GameRequests
{
    public class GameRequest : IGameRequest
    {
        protected readonly IGameRequestData MRequestData;
        public GameRequest(int requesterSpeakId, int reqId, string reqTitle, string reqDescription, 
            RequirementActionType mChallengeType, RequirementObjectType objectTypeRequired ,RequirementLogicEvaluator mReqLogic, 
            RequirementConsideredParameter mReqParameterType, int quantity, string[] rewards, string[] penalties, DayBitId targetDayId, PartOfDay targetPartOfDay)
        {
            MRequestData = new GameRequestData(requesterSpeakId, reqId, reqTitle, reqDescription, 
                mChallengeType, objectTypeRequired, mReqLogic, mReqParameterType, quantity, rewards, penalties, targetDayId, targetPartOfDay);
        }

        public DialogueSpeakerId RequesterSpeakerId => MRequestData.RequesterSpeakerId;
        public string ReqTitle => MRequestData.ReqTitle;
        public string ReqDescription => MRequestData.ReqDescription;
        public RequestStatus RequestStatus => MRequestData.Status;
        public int RequestId => MRequestData.RequestId;
        public void MarkAsCompleted()
        {
            MRequestData.CompleteChallenge();
        }

        public void MarkAsFailed()
        {
            MRequestData.FailChallenge();
        }

        public RequirementActionType ChallengeActionType => MRequestData.ChallengeType;
        public Dictionary<RewardTypes, IRewardData> Rewards => MRequestData.Rewards;
        public Dictionary<RewardTypes, IRewardData> Penalties => MRequestData.Penalties;
        public DayBitId ExpirationDayId => MRequestData.TargetTime.Item1;
        public PartOfDay ExpirationPartOfDay => MRequestData.TargetTime.Item2;
    }
}