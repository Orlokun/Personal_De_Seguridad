using System.Collections.Generic;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement.ComplianceSystem;

namespace GameDirection.ComplianceDataManagement
{
    public interface IComplianceManagerData
    {
        void LoadComplianceData();
        void StartDayComplianceObjects(DayBitId dayBitId);
        void EndDayComplianceObjects(DayBitId dayBitId);
        List<IComplianceObject> GetActiveComplianceObjects { get; }
        List<IComplianceObject> GetPassedComplianceObjects { get; }
        List<IComplianceObject> GetFailedComplianceObjects { get; }
        void ChangeComplianceStatus(ComplianceStatus newStatus, int id);
        void UpdateActiveCompliance();
    }
}