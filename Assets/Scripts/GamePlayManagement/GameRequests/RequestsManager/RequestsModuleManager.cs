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
using UI;
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

        List<IGameRequest> _mToleranceReachedRequests = new List<IGameRequest>();
        
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
                        _mRequestModuleData.AddCompletedRequestInData((DialogueSpeakerId)supplierIndex, challenge);
                        ProcessRewardsAndPenalties(challenge.Rewards);
                    }
                }
            }
            CleanActiveRequests();
        }

        public void CheckItemUsedChallenges(IItemObject itemObject)
        {
            if (ActiveRequests == null || ActiveRequests.Count == 0)
            {
                return;
            }

            foreach (var activeRequester in ActiveRequests)
            {
                var requesterId = activeRequester.Key;
                var supplierHasRequests = ActiveRequests.TryGetValue(requesterId, out var requests);
                if (!supplierHasRequests)
                {
                    continue;
                }

                if (activeRequester.Value.All(x => x.ChallengeActionType != RequirementActionType.Use) &&
                    activeRequester.Value.All(x => x.ChallengeActionType != RequirementActionType.NotUse))
                {
                    Debug.Log("Speaker has no request considering the <b>use action</b>. Continue.");
                    continue;
                }
                CheckRequests(requests, itemObject);
            }
            if(_mToleranceReachedRequests.Count > 0)
            {
                ProcessCompletedRequests();
            }
        }

        public void CleanRequestModule()
        {
        }

        private bool HasItemUseRequirements(List<IGameRequest> requests)
        {
            return requests.Any(x=> x.ChallengeObjectType == RequirementObjectType.AnyItem || 
                                    x.ChallengeObjectType == RequirementObjectType.Guard ||
                                    x.ChallengeObjectType == RequirementObjectType.Camera ||
                                    x.ChallengeObjectType == RequirementObjectType.Weapon ||
                                    x.ChallengeObjectType == RequirementObjectType.Traps ||
                                    x.ChallengeObjectType == RequirementObjectType.Other);
        }

        private void CheckRequests(List<IGameRequest> supplierRequests, IItemObject itemObject)
        {
            var useRequests = supplierRequests.Where(x => x.ChallengeActionType == RequirementActionType.Use).ToList();
            useRequests.AddRange(supplierRequests.Where(x=> x.ChallengeActionType == RequirementActionType.NotUse));
            if(!HasItemUseRequirements(useRequests))
            {
                return;
            }
            var useItemsRequests = useRequests.Where(x => x.ChallengeObjectType == RequirementObjectType.AnyItem || 
                                                          x.ChallengeObjectType == RequirementObjectType.Guard ||
                                                          x.ChallengeObjectType == RequirementObjectType.Camera ||
                                                          x.ChallengeObjectType == RequirementObjectType.Weapon ||
                                                          x.ChallengeObjectType == RequirementObjectType.Traps ||
                                                          x.ChallengeObjectType == RequirementObjectType.Other).ToList();
            foreach (var request in useItemsRequests)
            {
                switch (request.ChallengeConsideredParameter)
                {
                    //Done
                    case RequirementConsideredParameter.Origin:
                        ProcessItemOriginRequest(request, itemObject);
                        break;
                    case RequirementConsideredParameter.BaseType:
                        ProcessItemBaseTypeRequest(request, itemObject);
                        break;
                    case RequirementConsideredParameter.Quality:
                        ProcessItemQualityRequest(request, itemObject);
                        break;
                    case RequirementConsideredParameter.ItemSupplier:
                        ProcessItemSupplierUseRequest(request, itemObject);
                        break;
                    case RequirementConsideredParameter.ItemValue:
                        ProcessItemValueRequest(request, itemObject);
                        break;
                    case RequirementConsideredParameter.Variable:
                        ProcessItemVariableRequest(request, itemObject);
                        break;
                    default: 
                        continue;
                }
                
                if (request.CurrentDevelopmentCount >= request.RequestQuantityTolerance)
                {
                    _mToleranceReachedRequests.Add(request);
                }
            }
        }

        private void ProcessCompletedRequests()
        {
            if (_mToleranceReachedRequests.Count == 0)
            {
                return;
            }

            foreach (var finalizedRequest in _mToleranceReachedRequests)
            {
                finalizedRequest.ProcessCompletionInObjectData();
                _mRequestModuleData.ResolveFinishedRequest(finalizedRequest.RequesterSpeakerId, finalizedRequest);

                if (finalizedRequest.RequestStatus == RequestStatus.Completed)
                {
                    ProcessRewardsAndPenalties(finalizedRequest.Rewards);
                }
                
                if (finalizedRequest.RequestStatus == RequestStatus.Failed)
                {
                    ProcessRewardsAndPenalties(finalizedRequest.Penalties);
                }
            }
            GameDirector.Instance.GetActiveGameProfile.UpdateProfileData();
            UIController.Instance.UpdateInfoUI();
            CleanActiveRequests();
            _mToleranceReachedRequests.Clear();
        }

        private void ProcessItemVariableRequest(IGameRequest request, IItemObject itemObject)
        {
            throw new NotImplementedException();
        }

        private void ProcessItemValueRequest(IGameRequest request, IItemObject itemObject)
        {
            throw new NotImplementedException();
        }

        private void ProcessItemSupplierUseRequest(IGameRequest request, IItemObject itemObject)
        {
            throw new NotImplementedException();
        }

        private void ProcessItemQualityRequest(IGameRequest request, IItemObject itemObject)
        {
            throw new NotImplementedException();
        }

        private void ProcessItemBaseTypeRequest(IGameRequest request, IItemObject itemObject)
        {
            var useItemByBaseTypeRequest = request as IUseItemByBaseTypeRequest;
            
            if (useItemByBaseTypeRequest.ChallengeLogicOperator == RequirementLogicEvaluator.Equal)
            {
                if(itemObject.ItemStats.ItemTypes.Any(x=> useItemByBaseTypeRequest.RequestConsideredTypes.Contains(x)))
                {
                    useItemByBaseTypeRequest.AdvanceRequestDevelopmentCount();
                }
            }
            if (useItemByBaseTypeRequest.ChallengeLogicOperator == RequirementLogicEvaluator.NotEqual)
            {
                if(itemObject.ItemStats.ItemTypes.Any(x=> useItemByBaseTypeRequest.RequestConsideredTypes.Contains(x)))
                {
                    useItemByBaseTypeRequest.AdvanceRequestDevelopmentCount();
                }
            }        
        }

        private void ProcessItemOriginRequest(IGameRequest request, IItemObject itemObject)
        {
            var originRequest = (IUseItemByOriginRequest)request;
            
            if (originRequest.ChallengeLogicOperator == RequirementLogicEvaluator.Equal)
            {
                if(originRequest.RequestConsideredOrigins.Contains(itemObject.ItemStats.ItemOrigin))
                {
                    originRequest.AdvanceRequestDevelopmentCount();
                }
            }
            if (originRequest.ChallengeLogicOperator == RequirementLogicEvaluator.NotEqual)
            {
                if(!originRequest.RequestConsideredOrigins.Contains(itemObject.ItemStats.ItemOrigin))
                {
                    originRequest.AdvanceRequestDevelopmentCount();
                }
            }
        }

        private void CheckOtherItemsUseRequest(IItemObject itemObject, IGameRequest request)
        {
            throw new NotImplementedException();
        }

        private void CheckTrapUseRequest(IItemObject itemObject, IGameRequest request)
        {
            throw new NotImplementedException();
        }

        private void CheckWeaponUseRequest(IItemObject itemObject, IGameRequest request)
        {
            throw new NotImplementedException();
        }

        private void CheckCameraUseRequest(IItemObject itemObject, IGameRequest request)
        {
            
        }

        private void CheckGuardUseRequest(IItemObject itemObject, IGameRequest request)
        {
            
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
            CleanActiveRequests();
            RemoveFailedRequestsFromActive(failedRequests);
        }

        private void CleanActiveRequests()
        {
            var failedRequests = new List<Tuple<DialogueSpeakerId, int>>();
            var succeededRequests = new List<Tuple<DialogueSpeakerId, int>>();
            if(ActiveRequests == null || ActiveRequests.Count == 0)
            {
                return;
            }

            foreach (var activeRequest in ActiveRequests)
            {
                if (activeRequest.Value.Count == 0)
                {
                    continue;
                }
                var requests = activeRequest.Value;
                for (var i = 0; i < requests.Count; i++)
                {
                    if (requests[i].RequestStatus == RequestStatus.Failed)
                    {
                        failedRequests.Add(new Tuple<DialogueSpeakerId, int>(activeRequest.Key, requests[i].RequestId));
                        _mRequestModuleData.AddFailedRequestInData(activeRequest.Key, requests[i]);
                    }
                    if (requests[i].RequestStatus == RequestStatus.Completed)
                    {
                        succeededRequests.Add(new Tuple<DialogueSpeakerId, int>(activeRequest.Key, requests[i].RequestId));
                        _mRequestModuleData.AddCompletedRequestInData(activeRequest.Key, requests[i]);
                    }
                }
            }
            RemoveCompletedChallengesFromActive(succeededRequests);
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
            var currentDay = GameDirector.Instance.GetActiveGameProfile.GetProfileCalendar().CurrentDayBitId;
            foreach (var activeRequest in ActiveRequests)
            {
                foreach (var request in activeRequest.Value)
                {
                    if(request.ExpirationPartOfDay < dayTime && request.ExpirationDayId <= currentDay)
                    {
                        request.MarkAsFailed();
                        _mRequestModuleData.AddFailedRequestInData(activeRequest.Key, request);
                    }
                }
            }
            CleanActiveRequests();
        }
        private void RemoveCompletedChallengesFromActive(List<Tuple<DialogueSpeakerId, int>> completedChallenges)
        {
            if(ActiveRequests == null || ActiveRequests.Count == 0)
            {
                return;
            }

            for (var i = 0; i<completedChallenges.Count;i++)
            {
                if (!ActiveRequests.ContainsKey(completedChallenges[i].Item1))
                {
                    continue;
                }
                //Get the requests from one supplier
                var supplierRequests = ActiveRequests[completedChallenges[i].Item1];
                
                if (supplierRequests.All(x => x.RequestId != completedChallenges[i].Item2))
                {
                    return;
                }
                supplierRequests.Remove(supplierRequests.Find(x => x.RequestId == completedChallenges[i].Item2));
            }
            Debug.Log("Cleared Completed Active Challenges");
        }

        #endregion
    }
}