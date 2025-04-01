using System;
using System.Collections.Generic;
using DataUnits.ItemScriptableObjects;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement.BitDescriptions.RequestParameters;

namespace GamePlayManagement.ComplianceSystem
{
    public class ComplianceUseObjectByOrigin : ComplianceObject, IComplianceUseObjectByOrigin
    {
        private List<ItemOrigin> _mConsideredOrigins;
        public List<ItemOrigin> ComplianceConsideredOrigins => _mConsideredOrigins;
        public ComplianceUseObjectByOrigin(int complianceId, DayBitId startDayId, DayBitId endDayId, bool needsUnlock,
            ComplianceMotivationalLevel motivationLvl, ComplianceActionType actionType, ComplianceObjectType objectType,
            RequirementConsideredParameter consideredParameter, string[] complianceReqValues, int toleranceValue,
            string[] rewardValues, string[] penaltyValues, string title, string subtitle, string description,
            RequirementLogicEvaluator complianceLogic) :
            base(complianceId, startDayId, endDayId, needsUnlock, motivationLvl, actionType, objectType, consideredParameter, complianceReqValues,
                toleranceValue,rewardValues, penaltyValues, title, subtitle, description, complianceLogic)
        {
            LoadConsideredOrigins(complianceReqValues);
        }

        private void LoadConsideredOrigins(string[] complianceReqValues)
        {
            _mConsideredOrigins = new List<ItemOrigin>();
            foreach (var complianceReqValue in complianceReqValues)
            {
                var isOrigin = Enum.TryParse(complianceReqValue, out ItemOrigin itemOrigin);
                if (!isOrigin)
                {
                    continue;
                }
                _mConsideredOrigins.Add(itemOrigin);
            }
        }

    }
}