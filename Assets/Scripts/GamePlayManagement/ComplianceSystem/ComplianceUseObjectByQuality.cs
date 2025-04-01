using System;
using DataUnits.ItemScriptableObjects;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement.BitDescriptions.RequestParameters;

namespace GamePlayManagement.ComplianceSystem
{
    public class ComplianceUseObjectByQuality : ComplianceObject, IComplianceUseObjectByQuality
    {
        private ItemBaseQuality mBaseQuality;
        public ItemBaseQuality ItemQuality => mBaseQuality;
        public ComplianceUseObjectByQuality(int complianceId, DayBitId startDayId, DayBitId endDayId, bool needsUnlock,
            ComplianceMotivationalLevel motivationLvl, ComplianceActionType actionType, ComplianceObjectType objectType,
            RequirementConsideredParameter consideredParameter, string[] complianceReqValues, int toleranceValue,
            string[] rewardValues, string[] penaltyValues, string title, string subtitle, string description,
            RequirementLogicEvaluator complianceLogic) :
            base(complianceId, startDayId, endDayId, needsUnlock, motivationLvl, actionType, objectType, consideredParameter, complianceReqValues,toleranceValue,rewardValues,
                penaltyValues, title, subtitle, description,complianceLogic)
        {
            LoadQuality(complianceReqValues);
        }

        private void LoadQuality(string[] complianceReqValues)
        {
            var complianceReqValue = complianceReqValues[0];
            var isQuality = Enum.TryParse(complianceReqValue, out ItemBaseQuality consideredQuality);
            if (!isQuality)
            {
                return;
            }
            mBaseQuality  = consideredQuality;
        }
    }
}