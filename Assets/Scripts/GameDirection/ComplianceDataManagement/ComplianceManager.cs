using System;
using System.Collections.Generic;
using System.Linq;
using DataUnits.ItemScriptableObjects;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement;
using GamePlayManagement.BitDescriptions.RequestParameters;
using GamePlayManagement.ComplianceSystem;
using GamePlayManagement.GameRequests.RewardsPenalties;
using UI;
using UnityEngine;

namespace GameDirection.ComplianceDataManagement
{
    public class ComplianceManager : IComplianceManager
    {
        private readonly IComplianceManagerData _mComplianceBaseData = new ComplianceManagerData();
        private List<IComplianceObject> _mTempCompletedComplianceObjects = new List<IComplianceObject>();
        private IPlayerGameProfile _mActivePlayerProfile;

        public void LoadComplianceData()
        {
            _mComplianceBaseData.LoadComplianceData();
        }

        public void StartComplianceEndOfDayProcess(DayBitId dayBitId)
        {
            if(_mComplianceBaseData.GetActiveComplianceObjects.All(x=> x.GetComplianceObjectData.EndDayId != dayBitId))
            {
                return;
            }
            ProcessEndOfDayData(dayBitId);
        }
        
        private void ProcessEndOfDayData(DayBitId dayBitId)
        {
            var finishedCompliance = _mComplianceBaseData.GetActiveComplianceObjects.Where(x => x.GetComplianceObjectData.EndDayId == dayBitId).ToList();
            foreach (var complianceObject in finishedCompliance)
            {
                if (complianceObject.GetComplianceObjectData.ComplianceStatus == ComplianceStatus.Active)
                {
                    complianceObject.GetComplianceObjectData.SetComplianceStatus(ComplianceStatus.Failed);
                    ActivateComplianceReward(complianceObject.GetComplianceObjectData.PenaltyValues);
                }
            }
            _mComplianceBaseData.UpdateActiveCompliance();
        }

        public List<IComplianceObject> GetCompletedComplianceObjects => _mComplianceBaseData.GetPassedComplianceObjects;
        public List<IComplianceObject> GetFailedComplianceObjects => _mComplianceBaseData.GetFailedComplianceObjects;
        public List<IComplianceObject> GetActiveComplianceObjects => _mComplianceBaseData.GetActiveComplianceObjects;
        public void CleanComplianceModule()
        {
            
            _mComplianceBaseData.CleanComplianceData();
        }


        private bool _mUpdateComplianceFunctionAvailable = true;
        private IPlayerGameProfile _mActivePlayer;

        public void UpdateComplianceDay(DayBitId dayBitId)
        {
            if (!_mUpdateComplianceFunctionAvailable)
            {
                return;
            }
            _mUpdateComplianceFunctionAvailable = false;
            if (dayBitId == 0)
            {
                return;
            }
            _mComplianceBaseData.StartDayComplianceObjects(dayBitId);
            _mUpdateComplianceFunctionAvailable = true;
        }

        #region UseItemComplianceEvaluation

        

        public void CheckItemPlacementCompliance(IItemObject itemObject)
        {
            if (GetActiveComplianceObjects == null || GetActiveComplianceObjects.Count == 0)
            {
                return;
            }

            //Confirm any Compliance objects make sense with this actions
            if (GetActiveComplianceObjects.All(x => x.GetComplianceObjectData.ActionType != ComplianceActionType.Use) &&
                GetActiveComplianceObjects.All(x =>
                    x.GetComplianceObjectData.ActionType != ComplianceActionType.NotUse))
            {
                Debug.Log("Speaker has no request considering the <b>use action</b>. Continue.");
                return;
            }
            
            //Filter compliance where using an item is considered
            var requests = GetActiveComplianceObjects.Where(x =>
                x.GetComplianceObjectData.ActionType == ComplianceActionType.Use ||
                x.GetComplianceObjectData.ActionType == ComplianceActionType.NotUse).ToList();
            if(!HasItemUseRequirements(requests))
            {
                return;
            }
            var useItemsRequests = requests.Where(x => x.GetComplianceObjectData.ObjectType == ComplianceObjectType.AnyItem || 
                                                       x.GetComplianceObjectData.ObjectType == ComplianceObjectType.Guard ||
                                                       x.GetComplianceObjectData.ObjectType == ComplianceObjectType.Camera ||
                                                       x.GetComplianceObjectData.ObjectType == ComplianceObjectType.Weapon ||
                                                       x.GetComplianceObjectData.ObjectType == ComplianceObjectType.Traps ||
                                                       x.GetComplianceObjectData.ObjectType == ComplianceObjectType.Other).ToList();            
            //Evaluate filtered compliance objects
            CheckUseItemCompliance(useItemsRequests, itemObject);
            if(_mTempCompletedComplianceObjects.Count > 0)
            {
                ProcessCompletedCompliance();
            }
        }

        private void ProcessCompletedCompliance()
        {
            if (_mTempCompletedComplianceObjects.Count == 0)
            {
                return;
            }
            if (_mActivePlayerProfile == null)
            {
                _mActivePlayer = GameDirector.Instance.GetActiveGameProfile;
            }
            foreach (var finalizedRequest in _mTempCompletedComplianceObjects)
            {
                finalizedRequest.ProcessEndCompliance();

                if (finalizedRequest.GetComplianceObjectData.ComplianceStatus == ComplianceStatus.Passed)
                {
                    ProcessRewardsAndPenalties(finalizedRequest.GetComplianceObjectData.RewardValues);
                }
                
                if (finalizedRequest.GetComplianceObjectData.ComplianceStatus == ComplianceStatus.Failed)
                {
                    ProcessRewardsAndPenalties(finalizedRequest.GetComplianceObjectData.PenaltyValues);
                }
            }
            _mComplianceBaseData.UpdateActiveCompliance();
            GameDirector.Instance.GetActiveGameProfile.UpdateProfileData();
            UIController.Instance.UpdateInfoUI();
            _mTempCompletedComplianceObjects.Clear();        
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

        private void CheckUseItemCompliance(List<IComplianceObject> requests, IItemObject itemObject)
        {
            foreach (var request in requests)
            {
                switch (request.GetComplianceObjectData.ConsideredParameter)
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
                if (request.IsToleranceLevelReached)
                {
                    _mTempCompletedComplianceObjects.Add(request);
                }
            }
            
        }

        private void ProcessItemVariableRequest(IComplianceObject request, IItemObject itemObject)
        {
            throw new NotImplementedException();
        }

        private void ProcessItemValueRequest(IComplianceObject request, IItemObject itemObject)
        {
            throw new NotImplementedException();
        }

        private void ProcessItemSupplierUseRequest(IComplianceObject request, IItemObject itemObject)
        {
            throw new NotImplementedException();
        }

        private void ProcessItemQualityRequest(IComplianceObject request, IItemObject itemObject)
        {
            throw new NotImplementedException();
        }

        private void ProcessItemBaseTypeRequest(IComplianceObject request, IItemObject itemObject)
        {
            throw new NotImplementedException();
        }

        private void ProcessItemOriginRequest(IComplianceObject request, IItemObject itemObject)
        {
            var originRequest = (IComplianceUseObjectByOrigin)request;
            
            if (originRequest.GetComplianceObjectData.LogicEvaluator == RequirementLogicEvaluator.Equal || originRequest.GetComplianceObjectData.LogicEvaluator == RequirementLogicEvaluator.NotEqual)
            {
                if(originRequest.ComplianceConsideredOrigins.Contains(itemObject.ItemStats.ItemOrigin))
                {
                    originRequest.MarkOneAction();
                }
            }
        }

        private bool HasItemUseRequirements(List<IComplianceObject> requests)
        {
            return requests.Any(x=> x.GetComplianceObjectData.ObjectType == ComplianceObjectType.AnyItem || 
                                    x.GetComplianceObjectData.ObjectType == ComplianceObjectType.Guard ||
                                    x.GetComplianceObjectData.ObjectType == ComplianceObjectType.Camera ||
                                    x.GetComplianceObjectData.ObjectType == ComplianceObjectType.Weapon ||
                                    x.GetComplianceObjectData.ObjectType == ComplianceObjectType.Traps ||
                                    x.GetComplianceObjectData.ObjectType == ComplianceObjectType.Other);
        }
        #endregion

        public void UnlockCompliance(int id)
        {
            if (_mComplianceBaseData.GetActiveComplianceObjects.Any(x => x.GetComplianceObjectData.ComplianceId == id))
            {
                var unlockedCompliance = _mComplianceBaseData.GetActiveComplianceObjects.First(x => x.GetComplianceObjectData.ComplianceId == id);
                if (unlockedCompliance.GetComplianceObjectData.ComplianceStatus != ComplianceStatus.Locked)
                {
                    return;
                }
                unlockedCompliance.GetComplianceObjectData.SetComplianceStatus(ComplianceStatus.Active);
            } 
        }
        
        public void CompleteCompliance(int id)
        {
            if (_mComplianceBaseData.GetActiveComplianceObjects.Any(x => x.GetComplianceObjectData.ComplianceId == id))
            {
                var passedCompliance = _mComplianceBaseData.GetActiveComplianceObjects.First(x => x.GetComplianceObjectData.ComplianceId == id).GetComplianceObjectData;
                passedCompliance.SetComplianceStatus(ComplianceStatus.Passed);
                ActivateComplianceReward(passedCompliance.RewardValues);
                _mComplianceBaseData.UpdateActiveCompliance();
            }
        }
        
        private void ActivateComplianceReward(Dictionary<RewardTypes, IRewardData> passedComplianceRewardValues)
        {
            foreach (var rewardData in passedComplianceRewardValues)
            {
                switch (rewardData.Key)
                {
                    case RewardTypes.OmniCredits:
                        var omniCreditRewardData = (IOmniCreditRewardData)rewardData.Value;
                        _mActivePlayer.GetStatusModule().ReceiveOmniCredits(omniCreditRewardData.OmniCreditsAmount);
                        break;
                    case RewardTypes.Seniority:
                        var seniorityRewardData = (ISeniorityRewardData)rewardData.Value;
                        _mActivePlayer.GetStatusModule().ReceiveSeniority(seniorityRewardData.SeniorityRewardAmount);
                        break;
                    case RewardTypes.Trust:
                        var trustRewardData = (ITrustRewardData)rewardData.Value;
                        _mActivePlayer.AddFondnessToActiveSupplier(trustRewardData);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            GameDirector.Instance.GetUIController.UpdateInfoUI();
        }
        
        public void FailCompliance(int id)
        {
            if (!_mComplianceBaseData.GetActiveComplianceObjects.Any(x => x.GetComplianceObjectData.ComplianceId == id))
            {
                return;
            }
            var passedCompliance = _mComplianceBaseData.GetActiveComplianceObjects.First(x => x.GetComplianceObjectData.ComplianceId == id).GetComplianceObjectData;
            passedCompliance.SetComplianceStatus(ComplianceStatus.Failed);
            ActivateComplianceReward(passedCompliance.RewardValues);
            _mComplianceBaseData.UpdateActiveCompliance();
        }

        public void SetProfile(IPlayerGameProfile currentPlayerProfile)
        {
            _mActivePlayer = currentPlayerProfile;
        }

        public void PlayerLostResetData()
        {
            
        }

        #region Compliance Evaluation API
        public void CheckRadioCompleted()
        {
            if(_mComplianceBaseData.GetActiveComplianceObjects.Any(x=> x.GetComplianceObjectData.ActionType == ComplianceActionType.Use && x.GetComplianceObjectData.ObjectType == ComplianceObjectType.Radio))
            {
                var radioCompliance = _mComplianceBaseData.GetActiveComplianceObjects.First(x=> x.GetComplianceObjectData.ActionType == ComplianceActionType.Use && x.GetComplianceObjectData.ObjectType == ComplianceObjectType.Radio);
                radioCompliance.MarkOneAction();
                if (radioCompliance.IsToleranceLevelReached)
                {
                    radioCompliance.GetComplianceObjectData.SetComplianceStatus(ComplianceStatus.Passed);
                    ActivateComplianceReward(radioCompliance.GetComplianceObjectData.RewardValues);
                    _mComplianceBaseData.UpdateActiveCompliance();
                    Debug.Log("Completed Radio Compliance Full Target");
                }
                Debug.Log($"Completed Radio Compliance {radioCompliance.ComplianceCurrentCount} times");
            }
        }
        #endregion
    }
}