using System.Collections.Generic;
using DialogueSystem.Units;
using GamePlayManagement.BitDescriptions.RequestParameters;

namespace DataUnits.GameRequests.RewardsPenalties
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
        public Dictionary<RewardTypes,IRewardData> Rewards { get; }
        public Dictionary<RewardTypes,IRewardData> Penalties { get; }
        bool IsCompleted { get; }
        void CompleteChallenge();
    }
}