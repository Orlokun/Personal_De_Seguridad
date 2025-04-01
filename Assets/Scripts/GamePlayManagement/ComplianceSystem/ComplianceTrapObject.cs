using GameDirection.TimeOfDayManagement;
using GamePlayManagement.BitDescriptions.RequestParameters;

namespace GamePlayManagement.ComplianceSystem
{
    public class ComplianceTrapObject : ComplianceObject
    {
        public ComplianceTrapObject(int complianceId, DayBitId startDayId, DayBitId endDayId, bool needsUnlock, 
            ComplianceMotivationalLevel motivationLvl, ComplianceActionType actionType, ComplianceObjectType objectType, 
            RequirementConsideredParameter consideredParameter, string[] complianceReqValues, int toleranceValue, string[] rewardValues, string[] penaltyValues, string title, string subtitle, string description, RequirementLogicEvaluator logicEvaluator) :
            base(complianceId, startDayId, endDayId, needsUnlock, motivationLvl, actionType, objectType, consideredParameter, complianceReqValues,toleranceValue, rewardValues,
                penaltyValues, title, subtitle, description, logicEvaluator)
        {
            
        }
    }
}