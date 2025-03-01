using System;
using System.Collections.Generic;
using System.Linq;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement;
using GamePlayManagement.BitDescriptions.RequestParameters;
using GamePlayManagement.ComplianceSystem;
using GamePlayManagement.GameRequests.RewardsPenalties;
using UnityEngine;

namespace GameDirection.ComplianceDataManagement
{
    public class ComplianceManager : IComplianceManager
    {
        private readonly IComplianceManagerData _mComplianceBaseData = new ComplianceManagerData();

        
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