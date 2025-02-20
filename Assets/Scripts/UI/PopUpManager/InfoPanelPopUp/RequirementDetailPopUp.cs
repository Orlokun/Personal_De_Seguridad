using System.Collections.Generic;
using DataUnits.GameRequests;
using DataUnits.GameRequests.RewardsPenalties;
using GameDirection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.PopUpManager.InfoPanelPopUp
{
    public class RequirementDetailPopUp : PopUpObject, IRequirementDetailPopUp
    {
        private IGameRequest _mRequestData;
        [SerializeField] protected Transform reqRewardsParent;
        [SerializeField] protected Transform reqPenaltiesParent;
        [SerializeField] protected TMP_Text reqTitle;
        [SerializeField] protected TMP_Text reqOwner;
        [SerializeField] protected TMP_Text reqDescription;
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

        public void SetAndDisplayRequirementInfo(IGameRequest request)
        {
            _mRequestData = request;
            
            var speakerData =  GameDirector.Instance.GetSpeakerData(request.RequesterSpeakerId);
            reqOwner.text = speakerData.SpeakerName;
            reqTitle.text = _mRequestData.ReqTitle;
            reqDescription.text = _mRequestData.ReqDescription;
            UpdateRewardsContent();
            UpdatePenaltiesContent();
        }

        private void UpdatePenaltiesContent()
        {
            foreach (var penalty in _mRequestData.Penalties)
            {
                switch (penalty.Key)
                {
                    case RewardTypes.OmniCredits:
                        var penaltyData = (IOmniCreditRewardData) penalty.Value;
                        var omniPenaltyPrefab = Instantiate(omniCreditRewardPrefab, reqPenaltiesParent);
                        var penaltyPrefab = omniPenaltyPrefab.GetComponent<IRewardObjectPrefab>();
                        penaltyPrefab.SetRewardAmount(penaltyData.OmniCreditsAmount);
                        break;
                    case RewardTypes.Seniority:
                        var seniorityReward = (ISeniorityRewardData)penalty.Value;
                        var seniorityPenaltyPrefab = Instantiate(this.seniorityRewardPrefab, reqPenaltiesParent);
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
            var trustPenaltyPrefab = Instantiate(trustRewardPrefab, reqPenaltiesParent);
            var trustPenaltyObject = trustPenaltyPrefab.GetComponent<IRewardObjectPrefab>();
            trustPenaltyObject.SetRewardAmount(trustPenalty.TrustAmount);
            var trustPanel = (ITrustRewardObjectPrefab)trustPenaltyObject;
            var requesterData = GameDirector.Instance.GetSpeakerData(_mRequestData.RequesterSpeakerId);
            var icon = IconsSpriteData.GetSpriteForJobSupplierIcon(requesterData.SpriteName);
            trustPanel.SetTrusteeName(reqOwner.text);
            trustPanel.SetImageIcon(icon);
        }
        private void InstantiateTrustRewardsInfo(KeyValuePair<RewardTypes, IRewardData> rewards)
        {
            var trustReward = (ITrustRewardData)rewards.Value;
            var trustRewardInstantiatedPrefab = Instantiate(trustRewardPrefab, reqRewardsParent);
            var trustRewardObject = trustRewardInstantiatedPrefab.GetComponent<IRewardObjectPrefab>();
            trustRewardObject.SetRewardAmount(trustReward.TrustAmount);
            var trustPanel = (ITrustRewardObjectPrefab)trustRewardObject;
            var requesterData = GameDirector.Instance.GetSpeakerData(_mRequestData.RequesterSpeakerId);
            var icon = IconsSpriteData.GetSpriteForJobSupplierIcon(requesterData.SpriteName);
            trustPanel.SetTrusteeName(reqOwner.text);
            trustPanel.SetImageIcon(icon);
        }

        private void UpdateRewardsContent()
        {
            foreach (var reward in _mRequestData.Rewards)
            {
                switch (reward.Key)
                {
                    case RewardTypes.OmniCredits:
                        var rewardData = (IOmniCreditRewardData) reward.Value;
                        var omniRewardObject = Instantiate(omniCreditRewardPrefab, reqRewardsParent);
                        var penaltyPrefab = omniRewardObject.GetComponent<IRewardObjectPrefab>();
                        penaltyPrefab.SetRewardAmount(rewardData.OmniCreditsAmount);
                        break;
                    case RewardTypes.Seniority:
                        var seniorityReward = (ISeniorityRewardData)reward.Value;
                        var seniorityPenaltyPrefab = Instantiate(this.seniorityRewardPrefab, reqRewardsParent);
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