using GameDirection.TimeOfDayManagement;
using GamePlayManagement.BitDescriptions.RequestParameters;

namespace GamePlayManagement.ComplianceSystem
{
    public interface ICompliancePunishObject : IComplianceObject
    {
        
    }
    public class CompliancePunishObject : ComplianceObject, ICompliancePunishObject
    {
        public CompliancePunishObject(int complianceId, DayBitId startDayId, DayBitId endDayId, bool needsUnlock, 
            ComplianceMotivationalLevel motivationLvl, ComplianceActionType actionType, ComplianceObjectType objectType, 
            RequirementConsideredParameter consideredParameter, string[] complianceReqValues, int toleranceValue, string[] rewardValues, string[] penaltyValues) :
            base(complianceId, startDayId, endDayId, needsUnlock, motivationLvl, actionType, objectType, consideredParameter, complianceReqValues,toleranceValue,rewardValues, penaltyValues)
        {
            
        }
    }
}