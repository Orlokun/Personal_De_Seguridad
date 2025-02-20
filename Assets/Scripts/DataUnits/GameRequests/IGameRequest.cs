using System.Collections.Generic;
using DataUnits.GameRequests.RewardsPenalties;
using DialogueSystem.Units;
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
        public void MarkAsCompleted();
        public RequirementActionType ChallengeActionType { get; }
        public Dictionary<RewardTypes,IRewardData> Rewards { get; }
        public Dictionary<RewardTypes,IRewardData> Penalties { get; }
    }
}