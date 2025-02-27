using System.Collections.Generic;
using DialogueSystem.Units;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement.BitDescriptions.RequestParameters;
using GamePlayManagement.GameRequests.RewardsPenalties;

namespace GamePlayManagement.GameRequests
{
    public interface IGameRequest
    {
        public DialogueSpeakerId RequesterSpeakerId { get; }
        public int RequestId { get; }
        public string ReqTitle { get; }
        public string ReqDescription { get; }
        public RequestStatus RequestStatus { get; }
        public void MarkAsCompleted();
        public void MarkAsFailed();
        public RequirementActionType ChallengeActionType { get; }
        public Dictionary<RewardTypes,IRewardData> Rewards { get; }
        public Dictionary<RewardTypes,IRewardData> Penalties { get; }
        public DayBitId ExpirationDayId { get; }
        public PartOfDay ExpirationPartOfDay { get; }
    }
}