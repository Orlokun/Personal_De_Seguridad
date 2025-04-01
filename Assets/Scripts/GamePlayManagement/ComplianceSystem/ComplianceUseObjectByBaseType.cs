using System;
using System.Collections.Generic;
using DataUnits.ItemScriptableObjects;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement.BitDescriptions.RequestParameters;

namespace GamePlayManagement.ComplianceSystem
{
    public class ComplianceUseObjectByBaseType : ComplianceObject, IComplianceUseObjectByBaseType
    {
        private List<ItemBaseType> _mConsideredBaseTypes;
        public List<ItemBaseType> ComplianceConsideredBaseTypes => _mConsideredBaseTypes;
        public ComplianceUseObjectByBaseType(int complianceId, DayBitId startDayId, DayBitId endDayId, bool needsUnlock,
            ComplianceMotivationalLevel motivationLvl, ComplianceActionType actionType, ComplianceObjectType objectType,
            RequirementConsideredParameter consideredParameter, string[] complianceReqValues, int toleranceValue,
            string[] rewardValues, string[] penaltyValues, string title, string subtitle, string description,
            RequirementLogicEvaluator complianceLogic) :
            base(complianceId, startDayId, endDayId, needsUnlock, motivationLvl, actionType, objectType, consideredParameter, complianceReqValues,toleranceValue,rewardValues,
                penaltyValues, title, subtitle, description,complianceLogic)
        {
            LoadConsideredOrigins(complianceReqValues);
        }

        private void LoadConsideredOrigins(string[] complianceReqValues)
        {
            foreach (var complianceReqValue in complianceReqValues)
            {
                var isOrigin = Enum.TryParse(complianceReqValue, out ItemBaseType consideredBaseType);
                if (!isOrigin)
                {
                    continue;
                }
                _mConsideredBaseTypes.Add(consideredBaseType);
            }
        }
    }
}