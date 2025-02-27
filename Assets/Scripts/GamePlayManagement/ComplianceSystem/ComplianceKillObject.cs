using GameDirection.TimeOfDayManagement;
using GamePlayManagement.BitDescriptions.RequestParameters;

namespace GamePlayManagement.ComplianceSystem
{
    public class ComplianceKillObject : ComplianceObject
    {
        public ComplianceKillObject(int complianceId, DayBitId startDayId, DayBitId endDayId, bool needsUnlock, 
            ComplianceMotivationalLevel motivationLvl, ComplianceActionType actionType, ComplianceObjectType objectType, 
            RequirementConsideredParameter consideredParameter, string[] complianceReqValues, int toleranceValue, string[] rewardValues, string[] penaltyValues, string title, string description) :
            base(complianceId, startDayId, endDayId, needsUnlock, motivationLvl, actionType, objectType, consideredParameter, complianceReqValues,toleranceValue,rewardValues, penaltyValues, title, description)
        {
            
        }
    }
}