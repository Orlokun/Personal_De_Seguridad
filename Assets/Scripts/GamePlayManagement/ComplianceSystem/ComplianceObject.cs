﻿using GameDirection.TimeOfDayManagement;
using GamePlayManagement.BitDescriptions.RequestParameters;

namespace GamePlayManagement.ComplianceSystem
{
    public class ComplianceObject : IComplianceObject
    {
        protected IComplianceObjectData MComplianceObjectData;
        public IComplianceObjectData GetComplianceObjectData => MComplianceObjectData;
        public void MarkAsActive()
        {
            MComplianceObjectData.SetComplianceStatus(ComplianceStatus.Active);
        }

        public ComplianceObject(int complianceId, DayBitId startDayId, DayBitId endDayId, bool needsUnlock, 
            ComplianceMotivationalLevel motivationLvl, ComplianceActionType actionType, ComplianceObjectType objectType, 
            RequirementConsideredParameter consideredParameter,string[] complianceReqValues,int toleranceValue, string[] rewardValues, string[] penaltyValues, string title, string description)
        {
            MComplianceObjectData = new ComplianceObjectData(complianceId, startDayId, endDayId, needsUnlock, motivationLvl, actionType, objectType, consideredParameter, complianceReqValues, toleranceValue, rewardValues, penaltyValues, title, description);
        }

    }
}