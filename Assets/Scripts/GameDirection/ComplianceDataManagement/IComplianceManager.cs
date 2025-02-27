using System.Collections.Generic;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement.ComplianceSystem;
using GamePlayManagement.ProfileDataModules;

namespace GameDirection.ComplianceDataManagement
{
    public interface IComplianceManager : IProfileModule
    {
        public void LoadComplianceData();
        public void EndDayComplianceObjects(DayBitId dayBitId);

        public void UpdateComplianceDay(DayBitId dayBitId);
        public void CompleteCompliance(int id);
        public void FailCompliance(int id);
        public void UnlockCompliance(int id);
        public List<IComplianceObject>GetCompletedComplianceObjects { get; }
        public List<IComplianceObject>GetFailedComplianceObjects { get; }
        public List<IComplianceObject>GetActiveComplianceObjects { get; }
    }
}