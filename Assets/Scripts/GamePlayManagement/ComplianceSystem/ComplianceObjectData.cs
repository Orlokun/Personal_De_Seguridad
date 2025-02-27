using System;
using System.Collections.Generic;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement.BitDescriptions.RequestParameters;
using GamePlayManagement.GameRequests.RewardsPenalties;
using UnityEngine;

namespace GamePlayManagement.ComplianceSystem
{
    public class ComplianceObjectData : IComplianceObjectData
    {
        public int ComplianceId => _mComplianceId;
        public DayBitId StartDayId => _mStartDayId;
        public DayBitId EndDayId => _mEndDayId;
        public bool NeedsUnlock => _mNeedsUnlock;
        public ComplianceMotivationalLevel MotivationLvl => _mMotivationLvl;
        public ComplianceActionType ActionType => _mActionType;
        public ComplianceObjectType ObjectType => _mObjectType;
        public RequirementConsideredParameter ConsideredParameter => _mConsideredParameter;
        public string[] ComplianceReqValues => _mComplianceReqRawValues;
        public int ToleranceValue => _mToleranceValue;
        public Dictionary<RewardTypes,IRewardData> RewardValues => _mRewards;
        public Dictionary<RewardTypes,IRewardData> PenaltyValues => _mPenalties;
        public ComplianceStatus ComplianceStatus => _mComplianceStatus;

        private int _mComplianceId;
        private DayBitId _mStartDayId;
        private DayBitId _mEndDayId;
        private bool _mNeedsUnlock;
        private ComplianceMotivationalLevel _mMotivationLvl;
        private ComplianceActionType _mActionType;
        private ComplianceObjectType _mObjectType;
        private RequirementConsideredParameter _mConsideredParameter;
        private string[] _mComplianceReqRawValues;
        private int _mToleranceValue;
        private string[] _mRewardValues;
        private string[] _mPenaltyValues;
        private ComplianceStatus _mComplianceStatus;
        private Dictionary<RewardTypes,IRewardData> _mRewards = new Dictionary<RewardTypes,IRewardData>();
        private Dictionary<RewardTypes,IRewardData>_mPenalties =  new Dictionary<RewardTypes,IRewardData>();
        
        public ComplianceObjectData(int complianceId, DayBitId startDayId, DayBitId endDayId, bool needsUnlock, 
            ComplianceMotivationalLevel motivationLvl, ComplianceActionType actionType, ComplianceObjectType objectType, 
            RequirementConsideredParameter consideredParameter,string[] complianceReqValues,int toleranceValue, string[] rewardValues, string[] penaltyValues)
        {
            _mComplianceId = complianceId;
            _mStartDayId = startDayId;
            _mEndDayId = endDayId;
            _mNeedsUnlock = needsUnlock;
            _mMotivationLvl = motivationLvl;
            _mActionType = actionType;
            _mObjectType = objectType;
            _mConsideredParameter = consideredParameter;
            _mComplianceReqRawValues = complianceReqValues;
            _mToleranceValue = toleranceValue;
            _mRewardValues = rewardValues;
            _mPenaltyValues = penaltyValues;
            _mComplianceStatus = ComplianceStatus.Locked;
            ProcessRewards(rewardValues);
            ProcessPenalties(penaltyValues);
        }

        public void SetComplianceStatus(ComplianceStatus status)
        {
            _mComplianceStatus = status;
        }

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
        private IRewardData CreateRewards(RewardTypes rewType, string[]rewards)
        {
            switch (rewType)
            {
                case RewardTypes.OmniCredits:
                    return new OmniCreditRewardData(rewType, rewards);
                case RewardTypes.Seniority:
                    return new SeniorityRewardData(rewType, rewards);
                case RewardTypes.Trust:
                    return new TrustRewardData(rewType, rewards, 0);
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

        #endregion

    }
}