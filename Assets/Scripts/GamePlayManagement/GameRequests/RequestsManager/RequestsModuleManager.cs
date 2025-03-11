using System;
using System.Collections.Generic;
using System.Linq;
using DataUnits.ItemScriptableObjects;
using DialogueSystem.Units;
using GameDirection;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement.BitDescriptions.RequestParameters;
using GamePlayManagement.BitDescriptions.Suppliers;
using GamePlayManagement.GameRequests.RewardsPenalties;
using UnityEngine;
using Utils;

namespace GamePlayManagement.GameRequests.RequestsManager
{
    public class RequestsModuleManager : IRequestsModuleManager
    {
        private IRequestModuleManagerData _mRequestModuleData;
        
        private IPlayerGameProfile _mActivePlayerProfile;
        
        #region API
        public Dictionary<DialogueSpeakerId, List<IGameRequest>> ActiveRequests => _mRequestModuleData.ActiveRequests;
        public Dictionary<DialogueSpeakerId, List<IGameRequest>> CompletedRequests => _mRequestModuleData.CompletedRequests;
        public Dictionary<DialogueSpeakerId, List<IGameRequest>> FailedRequests => _mRequestModuleData.FailedRequests;

        public void HandleIncomingRequestActivation(DialogueSpeakerId speaker, int requestId)
        {
            //Confirm request exists in base data.
            if (!_mRequestModuleData.RequestExistsInData(speaker, requestId))
            {
                return;
            }
            var request = _mRequestModuleData.BaseRequestsData[speaker].Find(r => r.RequestId == requestId);
            _mRequestModuleData.ActivateRequestInData(request);
        }
        //Completed Hire Challenge assume only one challenge is completed in the event. 
        public void CheckHireChallenges(JobSupplierBitId newJobSupplier)
        {
            if (ActiveRequests == null || ActiveRequests.Count == 0)
            {
                return;
            }
            var completedHireChallenges = new List<int[]>();
            for (var supplierIndex = 0; supplierIndex<ActiveRequests.Count;supplierIndex++) 
            {
                var supplierRequests = ActiveRequests.ElementAt(supplierIndex);
                if (supplierRequests.Value.All(x => x.ChallengeActionType != RequirementActionType.Hire))
                {
                    continue;
                }
                var hireChallenges = supplierRequests.Value.Where(x => x.ChallengeActionType == RequirementActionType.Hire);
                foreach (var challenge in hireChallenges)
                {
                    var hireChallenge = (IHireGameRequest)challenge;
                    if (hireChallenge.JobHireObjective == newJobSupplier)
                    {
                        challenge.MarkAsCompleted();
                        completedHireChallenges.Add(new[] {supplierIndex, challenge.RequestId});
                        _mRequestModuleData.AddCompletedRequestInData((DialogueSpeakerId)supplierIndex, challenge);
                        ProcessRewardsAndPenalties(challenge.Rewards);
                    }
                }
            }
            RemoveCompletedChallengesFromActive(completedHireChallenges);
        }

        public void CheckItemPlacementChallenges(IItemObject itemObject)
        {
            Debug.LogError("[RequestsModuleManager.CheckItemPlacementChallenges] Not Implemented");
        }

        #endregion

        #region Init
        public void SetProfile(IPlayerGameProfile currentPlayerProfile)
        {
            _mActivePlayerProfile = currentPlayerProfile;
            _mRequestModuleData = new RequestModuleManagerData();
            GameDirector.Instance.GetClockInDayManagement.OnPassTimeOfDay += CheckRequirementExpirationDate;            
            var url = DataSheetUrls.BaseGameRequests;
            _mRequestModuleData.LoadRequestsData(url);
        }

        public void PlayerLostResetData()
        {
            ActiveRequests.Clear();
            CompletedRequests.Clear();
            FailedRequests.Clear();
        }

        #endregion

        #region Utils
        private void CheckRequirementExpirationDate(PartOfDay dayTime)
        {
            if (dayTime == PartOfDay.EndOfDay)
            {
                ProcessEndOfDay();
                return;
            }
            ProcessActiveRequests(dayTime);
        }

        private void ProcessEndOfDay()
        {
            var failedRequests = new List<Tuple<DialogueSpeakerId, int>>();
            var currentDay = GameDirector.Instance.GetActiveGameProfile.GetProfileCalendar().CurrentDayBitId;
            foreach (var activeRequest in ActiveRequests)
            {
                foreach (var request in activeRequest.Value)
                {
                    if(request.ExpirationDayId <= currentDay)
                    {
                        FailRequest(request);
                        failedRequests.Add(new Tuple<DialogueSpeakerId,int>(activeRequest.Key, request.RequestId));
                        _mRequestModuleData.AddFailedRequestInData(activeRequest.Key, request);
                    }
                }
            }
            RemoveFailedRequestsFromActive(failedRequests);
        }

        private void FailRequest(IGameRequest request)
        {
            if(request.RequestStatus == RequestStatus.Failed)
            {
                return;
            }
            ProcessRewardsAndPenalties(request.Penalties);
            request.MarkAsFailed();
        }

        private void ProcessRewardsAndPenalties(Dictionary<RewardTypes, IRewardData> incomingData)
        {
            foreach (var rewardData in incomingData)
            {
                switch (rewardData.Key)
                {
                    case RewardTypes.OmniCredits:
                        var omniCreditRewardData = (IOmniCreditRewardData)rewardData.Value;
                        _mActivePlayerProfile.GetStatusModule().ReceiveOmniCredits(omniCreditRewardData.OmniCreditsAmount);
                        break;
                    case RewardTypes.Seniority:
                        var seniorityRewardData = (ISeniorityRewardData)rewardData.Value;
                        _mActivePlayerProfile.GetStatusModule().ReceiveSeniority(seniorityRewardData.SeniorityRewardAmount);
                        break;
                    case RewardTypes.Trust:
                        var trustRewardData = (ITrustRewardData)rewardData.Value;
                        _mActivePlayerProfile.AddFondnessToActiveSupplier(trustRewardData);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            GameDirector.Instance.GetUIController.UpdateInfoUI();
        }
        
        private void RemoveFailedRequestsFromActive(List<Tuple<DialogueSpeakerId, int>> failedRequests)
        {            
            if(ActiveRequests == null || ActiveRequests.Count == 0)
            {
                return;
            }
            for (var i = 0; i<failedRequests.Count;i++)
            {
                if (!ActiveRequests.ContainsKey(failedRequests[i].Item1))
                {
                    continue;
                }
                //Get the requests from one supplier
                var supplierRequests = ActiveRequests[failedRequests[i].Item1];
                if (supplierRequests.All(x => x.RequestId != failedRequests[i].Item2))
                {
                    return;
                }
                supplierRequests.Remove(supplierRequests.Find(x => x.RequestId == failedRequests[i].Item2));
            }
            Debug.Log("Cleared Failed Challenges from Active");
        }

        private void ProcessActiveRequests(PartOfDay dayTime)
        {
            var failedRequests = new List<Tuple<DialogueSpeakerId, int>>();
            var currentDay = GameDirector.Instance.GetActiveGameProfile.GetProfileCalendar().CurrentDayBitId;
            foreach (var activeRequest in ActiveRequests)
            {
                foreach (var request in activeRequest.Value)
                {
                    if(request.ExpirationPartOfDay < dayTime && request.ExpirationDayId <= currentDay)
                    {
                        request.MarkAsFailed();
                        failedRequests.Add(new Tuple<DialogueSpeakerId,int>(activeRequest.Key, request.RequestId));
                        _mRequestModuleData.AddFailedRequestInData(activeRequest.Key, request);
                    }
                }
            }
            RemoveFailedRequestsFromActive(failedRequests);
        }
        private void RemoveCompletedChallengesFromActive(List<int[]> completedHireChallenges)
        {
            if(ActiveRequests == null || ActiveRequests.Count == 0)
            {
                return;
            }

            for (var i = 0; i<completedHireChallenges.Count;i++)
            {
                if (!ActiveRequests.ContainsKey((DialogueSpeakerId)completedHireChallenges[i][0]))
                {
                    continue;
                }
                //Get the requests from one supplier
                var supplierRequests = ActiveRequests[(DialogueSpeakerId)completedHireChallenges[i][0]];
                
                if (supplierRequests.All(x => x.RequestId != completedHireChallenges[i][1]))
                {
                    return;
                }
                supplierRequests.Remove(supplierRequests.Find(x => x.RequestId == completedHireChallenges[i][1]));
            }
            Debug.Log("Cleared Completed Active Challenges");
        }

        #endregion
    }
}