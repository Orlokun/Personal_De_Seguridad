using System.Collections.Generic;
using GameDirection;
using GamePlayManagement.ComplianceSystem;
using GamePlayManagement.GameRequests.RewardsPenalties;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.PopUpManager.InfoPanelPopUp
{
    public class ComplianceDetailPopUp : PopUpObject, IComplianceObjectDetailPopUp
    {
        private IComplianceObject _mComplianceObject;
        [SerializeField] protected Transform mRewardsParent;
        [SerializeField] protected Transform mPenaltiesParent;
        [SerializeField] protected TMP_Text mTitle;
        [SerializeField] protected TMP_Text mDescription;
        [SerializeField] protected TMP_Text mEndDay;
        
        [SerializeField] protected TMP_Text mCurrentActions;
        [SerializeField] protected TMP_Text mTolerance;
        
        [SerializeField] protected Button closeButton;

        [SerializeField] private GameObject omniCreditRewardPrefab;
        [SerializeField] private GameObject seniorityRewardPrefab;
        [SerializeField] private GameObject trustRewardPrefab;
        
        
        protected void Awake()
        {
            closeButton.onClick.AddListener(ClosePanel);
        }
        private void ClosePanel()
        {
            PopUpOperator.RemovePopUp(PopUpId);
        }
        
        public void SetAndDisplayComplianceObjectInfo(IComplianceObject complianceObjectData)
        {
            _mComplianceObject = complianceObjectData;
            mTitle.text = complianceObjectData.GetComplianceObjectData.GetTitle;
            mDescription.text = complianceObjectData.GetComplianceObjectData.GetDescription;
            mEndDay.text = complianceObjectData.GetComplianceObjectData.EndDayId.ToString();
            mTolerance.text = complianceObjectData.GetComplianceObjectData.ToleranceValue.ToString();
            mCurrentActions.text = complianceObjectData.ComplianceCurrentCount.ToString();
            UpdateRewardsContent();
            UpdatePenaltiesContent();
        }

        private void UpdatePenaltiesContent()
        {
            foreach (var penalty in _mComplianceObject.GetComplianceObjectData.PenaltyValues)
            {
                switch (penalty.Key)
                {
                    case RewardTypes.OmniCredits:
                        var penaltyData = (IOmniCreditRewardData) penalty.Value;
                        var omniPenaltyPrefab = Instantiate(omniCreditRewardPrefab, mRewardsParent);
                        var penaltyPrefab = omniPenaltyPrefab.GetComponent<IRewardObjectPrefab>();
                        penaltyPrefab.SetRewardAmount(penaltyData.OmniCreditsAmount);
                        break;
                    case RewardTypes.Seniority:
                        var seniorityReward = (ISeniorityRewardData)penalty.Value;
                        var seniorityPenaltyPrefab = Instantiate(this.seniorityRewardPrefab, mPenaltiesParent);
                        var seniorityPenaltyObject = seniorityPenaltyPrefab.GetComponent<IRewardObjectPrefab>();
                        seniorityPenaltyObject.SetRewardAmount(seniorityReward.SeniorityRewardAmount);
                        break;
                    case RewardTypes.Trust:
                        InstantiateTrustPenaltiesInfo(penalty);
                        break;
                    
                }
            }
        }

        private void InstantiateTrustPenaltiesInfo(KeyValuePair<RewardTypes, IRewardData> penalty)
        {
            var trustPenalty = (ITrustRewardData)penalty.Value;
            var trustPenaltyPrefab = Instantiate(trustRewardPrefab, mPenaltiesParent);
            var trustPenaltyObject = trustPenaltyPrefab.GetComponent<IRewardObjectPrefab>();
            trustPenaltyObject.SetRewardAmount(trustPenalty.TrustAmount);
            var trustPanel = (ITrustRewardObjectPrefab)trustPenaltyObject;
            var requesterData = GameDirector.Instance.GetSpeakerData(0);
            var icon = IconsSpriteData.GetSpriteForSupplierIcon(requesterData.SpriteName);
            trustPanel.SetTrusteeName(requesterData.SpeakerName);
            trustPanel.SetImageIcon(icon);
        }
        private void InstantiateTrustRewardsInfo(KeyValuePair<RewardTypes, IRewardData> rewards)
        {
            var trustReward = (ITrustRewardData)rewards.Value;
            var trustRewardInstantiatedPrefab = Instantiate(trustRewardPrefab, mRewardsParent);
            var trustRewardObject = trustRewardInstantiatedPrefab.GetComponent<IRewardObjectPrefab>();
            trustRewardObject.SetRewardAmount(trustReward.TrustAmount);
            var trustPanel = (ITrustRewardObjectPrefab)trustRewardObject;
            var requesterData = GameDirector.Instance.GetSpeakerData(0);
            var icon = IconsSpriteData.GetSpriteForSupplierIcon(requesterData.SpriteName);
            trustPanel.SetTrusteeName(requesterData.SpeakerName);
            trustPanel.SetImageIcon(icon);
        }

        private void UpdateRewardsContent()
        {
            foreach (var reward in _mComplianceObject.GetComplianceObjectData.RewardValues)
            {
                switch (reward.Key)
                {
                    case RewardTypes.OmniCredits:
                        var rewardData = (IOmniCreditRewardData) reward.Value;
                        var omniRewardObject = Instantiate(omniCreditRewardPrefab, mRewardsParent);
                        var penaltyPrefab = omniRewardObject.GetComponent<IRewardObjectPrefab>();
                        penaltyPrefab.SetRewardAmount(rewardData.OmniCreditsAmount);
                        break;
                    case RewardTypes.Seniority:
                        var seniorityReward = (ISeniorityRewardData)reward.Value;
                        var seniorityPenaltyPrefab = Instantiate(this.seniorityRewardPrefab, mRewardsParent);
                        var seniorityPenaltyObject = seniorityPenaltyPrefab.GetComponent<IRewardObjectPrefab>();
                        seniorityPenaltyObject.SetRewardAmount(seniorityReward.SeniorityRewardAmount);
                        break;
                    case RewardTypes.Trust:
                        InstantiateTrustRewardsInfo(reward);
                        break;
                    
                }
            }
        }
    }
}