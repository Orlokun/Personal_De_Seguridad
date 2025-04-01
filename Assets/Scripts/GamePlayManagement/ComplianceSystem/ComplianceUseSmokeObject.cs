using System.Collections.Generic;
using DataUnits.ItemScriptableObjects;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement.BitDescriptions.RequestParameters;

namespace GamePlayManagement.ComplianceSystem
{
    public class ComplianceUseSmokeObject : ComplianceObject, IComplianceUseObject
    {
        private List<ItemOrigin> _mConsideredOrigins;
        public List<ItemOrigin> ComplianceConsideredOrigins => _mConsideredOrigins;
        public ComplianceUseSmokeObject(int complianceId, DayBitId startDayId, DayBitId endDayId, bool needsUnlock,
            ComplianceMotivationalLevel motivationLvl, ComplianceActionType actionType, ComplianceObjectType objectType,
            RequirementConsideredParameter consideredParameter, string[] complianceReqValues, int toleranceValue,
            string[] rewardValues, string[] penaltyValues, string title, string subtitle, string description,
            RequirementLogicEvaluator complianceLogic) :
            base(complianceId, startDayId, endDayId, needsUnlock, motivationLvl, actionType, objectType, consideredParameter,
                complianceReqValues,toleranceValue,rewardValues, penaltyValues, title, subtitle, description, complianceLogic)
        {
        }
    }
}