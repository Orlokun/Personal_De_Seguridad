using System;
using System.Collections.Generic;
using DialogueSystem.Units;
using GamePlayManagement.BitDescriptions.RequestParameters;
using UnityEngine;

namespace DataUnits.GameRequests.RewardsPenalties
{
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
        
        private Dictionary<RewardTypes,IRewardData> _mRewards = new Dictionary<RewardTypes,IRewardData>();
        private Dictionary<RewardTypes,IRewardData>_mPenalties =  new Dictionary<RewardTypes,IRewardData>();

        #region Constructor and Initialization
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

        /// <summary>
        /// Penalties and rewards are basically the same. Keeping them separated just in case it makes it easier for future behaviors.
        /// </summary>
        #region Penalties and Rewards Processing
        private void ProcessPenalties(string[] penalties)
        {
            for (int i = 0; i < penalties.Length; i++)
            {
                var penaltyRaw = penalties[i].Split(',');
                //Remember for now Enum is the same for Rewards and Penalties.
                var isPenaltyType = Enum.TryParse(penaltyRaw[0], out RewardTypes penaltyType);
                if (!isPenaltyType)
                {
                    continue;
                }
                var penalty = CreateRewards(penaltyType, penaltyRaw);
                _mPenalties.Add(penaltyType, penalty);
            }   
        }
        private void ProcessRewards(string[] rewards)
        {
            for (int i = 0; i < rewards.Length; i++)
            {
                var rewardRaw = rewards[i].Split(',');
                var isRewardType = Enum.TryParse(rewardRaw[0], out RewardTypes rewardType);
                if (!isRewardType)
                {
                    continue;
                }
                var reward = CreateRewards(rewardType, rewardRaw);
                _mRewards.Add(rewardType, reward);
            }
        }
        
        //TODO: use different parameters for rewards. Some need more info. Review.
        private IRewardData CreateRewards(RewardTypes rewType, string[]rewards)
        {
            switch (rewType)
            {
                case RewardTypes.OmniCredits:
                    return new OmniCreditRewardData(rewType, rewards);
                case RewardTypes.Seniority:
                    return new SeniorityRewardData(rewType, rewards);
                case RewardTypes.Trust:
                    return new TrustRewardData(rewType, rewards);
                
                //For now only the first three are used. The rest are placeholders.
                case RewardTypes.ItemSupplier:
                    return new ItemSupplierRewardData(rewType, rewards);
                case RewardTypes.ItemInSupplier:
                    return new ItemInSupplierRewardData(rewType, rewards);
                default:
                    Debug.LogError("[GameRequestData.CreateRewards] Reward Type must exist");
                    return new RewardData(rewType, rewards);
            }
        }
        
        private IRewardData CreatePenalties(RewardTypes penaltyType, string[] penalties)
        {
            switch (penaltyType)
            {
                case RewardTypes.OmniCredits:
                    return new OmniCreditRewardData(penaltyType, penalties);
                case RewardTypes.Seniority:
                    return new SeniorityRewardData(penaltyType, penalties);
                case RewardTypes.Trust:
                    return new TrustRewardData(penaltyType, penalties);
                
                //For now only the first three are used. The rest are placeholders.
                case RewardTypes.ItemSupplier:
                    return new ItemSupplierRewardData(penaltyType, penalties);
                case RewardTypes.ItemInSupplier:
                    return new ItemInSupplierRewardData(penaltyType, penalties);
                default:
                    Debug.LogError("[GameRequestData.CreateRewards] Reward Type must exist");
                    return new RewardData(penaltyType, penalties);
            }
        }
        #endregion
        #endregion
        
        public DialogueSpeakerId RequesterSpeakerId => _mRequesterSpeakerId;
        public int RequestId => _mRequestId;
        public string ReqTitle => _mReqTitle;
        public string ReqDescription => _mReqDescription;
        
        public RequirementActionType ChallengeType => _mChallengeType;
        public RequirementObjectType ChallengeObjectType => _mChallengeObjectType;
        public RequirementLogicEvaluator ReqLogic => _mReqLogic;
        public RequirementConsideredParameter ReqParameterType => _mReqParameterType;
        public Dictionary<RewardTypes,IRewardData> Rewards => _mRewards;
        public Dictionary<RewardTypes,IRewardData> Penalties => _mPenalties;

        public bool IsCompleted => _mIsCompleted;
        public void CompleteChallenge()
        {
            _mIsCompleted = true;
        }
    }
}