using System;
using System.Collections.Generic;
using DialogueSystem.Units;
using GamePlayManagement.BitDescriptions.RequestParameters;

namespace DataUnits.GameRequests
{
    public struct RewardData : IRewardData
    {
        private RewardTypes _mRewardType;
        private int _mRewardValue;
        public RewardTypes RewardType => _mRewardType;
        public int RewardValue => _mRewardValue;
        public RewardData(RewardTypes rewardType, int rewardValue)
        {
            _mRewardType = rewardType;
            _mRewardValue = rewardValue;
        }
    }

    public interface IRewardData
    {
        public RewardTypes RewardType { get; }
        public int RewardValue { get; }
    }
    
    public interface IPenaltyData
    {
        public RewardTypes RewardType { get; }
        public int RewardValue { get; }
    }

    public enum RewardTypes
    {
        OmniCredits,
        Seniority,
        Trust
    }
    
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
        void CompleteChallenge();
    }
    public class GameRequestData : IGameRequestData
    {
        private DialogueSpeakerId _mRequesterSpeakerId;
        private int _mRequestId;
        private string _mReqTitle;
        private string _mReqDescription;
        private int _mReqQuantity;
        
        private RequirementActionType _mChallengeType;
        private RequirementObjectType _mChallengeObjectType;
        private RequirementLogicEvaluator _mReqLogic;
        private RequirementConsideredParameter _mReqParameterType;
        private string[] _mReqParameterValue;
        private bool _mIsCompleted;
        
        private List<IRewardData> _mRewards;
        private List<IRewardData> _mPenalties;

        public GameRequestData(int mRequesterSpeakerId, int mRequestId, string mReqTitle, string mReqDescription,
            RequirementActionType mChallengeType, RequirementObjectType mChallengeObjectType, RequirementLogicEvaluator mReqLogic,
            RequirementConsideredParameter mReqParameterType, int quantity, string[] rewards, string[] penalties)
        {
            _mRequesterSpeakerId = (DialogueSpeakerId)mRequesterSpeakerId;
            _mRequestId = mRequestId;
            _mReqTitle = mReqTitle;
            _mReqDescription = mReqDescription;
            _mChallengeType = mChallengeType;
            _mChallengeObjectType = mChallengeObjectType;
            _mReqLogic = mReqLogic;
            _mReqParameterType = mReqParameterType;
            _mReqQuantity = quantity;
            ProcessRewards(rewards);
            ProcessPenalties(penalties);
        }

        private void ProcessPenalties(string[] penalties)
        {
            for (int i = 0; i < penalties.Length; i++)
            {
                var isRewardType = Enum.TryParse(penalties[0], out RewardTypes rewardType);
                if (!isRewardType)
                {
                    continue;
                }
                //TODO: use different parameters for rewards. Some need more info. Review.
                var reward = CreatePenalties(rewardType, penalties);
            }        
        }

        private object CreatePenalties(RewardTypes rewardType, string[] penalties)
        {
            throw new NotImplementedException();
        }

        private void ProcessRewards(string[] rewards)
        {
            for (int i = 0; i < rewards.Length; i++)
            {
                var isRewardType = Enum.TryParse(rewards[0], out RewardTypes rewardType);
                if (!isRewardType)
                {
                    continue;
                }
                //TODO: use different parameters for rewards. Some need more info. Review.
                var reward = CreateRewards(rewardType, int.Parse(rewards[1]));
            }
        }
        
        //TODO: use different parameters for rewards. Some need more info. Review.
        private IRewardData CreateRewards(RewardTypes rewType, int rewValue)
        {
            return new RewardData(rewType, rewValue);
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
        public void CompleteChallenge()
        {
            _mIsCompleted = true;
        }
    }
}