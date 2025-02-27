using System.Collections.Generic;
using System.Linq;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement;
using GamePlayManagement.ComplianceSystem;

namespace GameDirection.ComplianceDataManagement
{
    public class ComplianceManager : IComplianceManager
    {
        private readonly IComplianceManagerData _mComplianceBaseData = new ComplianceManagerData();

        
        public void LoadComplianceData()
        {
            _mComplianceBaseData.LoadComplianceData();
        }

        public void EndDayComplianceObjects(DayBitId dayBitId)
        {
            _mComplianceBaseData.EndDayComplianceObjects(dayBitId);
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
                _mComplianceBaseData.UpdateActiveCompliance();
            }
        }
        
        public void FailCompliance(int id)
        {
            if (_mComplianceBaseData.GetActiveComplianceObjects.Any(x => x.GetComplianceObjectData.ComplianceId == id))
            {
                _mComplianceBaseData.GetActiveComplianceObjects.First(x => x.GetComplianceObjectData.ComplianceId == id).GetComplianceObjectData.SetComplianceStatus(ComplianceStatus.Failed);
                _mComplianceBaseData.UpdateActiveCompliance();
            }        
        }

        public void SetProfile(IPlayerGameProfile currentPlayerProfile)
        {
            _mActivePlayer = currentPlayerProfile;
        }

        public void PlayerLostResetData()
        {
            
        }
    }
}