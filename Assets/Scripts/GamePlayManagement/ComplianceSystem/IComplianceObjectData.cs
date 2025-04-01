using System.Collections.Generic;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement.BitDescriptions.RequestParameters;
using GamePlayManagement.GameRequests.RewardsPenalties;

namespace GamePlayManagement.ComplianceSystem
{
    public interface IComplianceObjectData
    {
        public int ComplianceId { get; }
        public DayBitId StartDayId { get; }
        public DayBitId EndDayId { get; }
        public bool NeedsUnlock { get; }
        public ComplianceMotivationalLevel MotivationLvl { get; }
        public ComplianceActionType ActionType { get; }
        public ComplianceObjectType ObjectType { get; }
        public RequirementConsideredParameter ConsideredParameter { get; }
        public string[] ComplianceReqValues { get; }
        public int ToleranceValue { get; }
        public Dictionary<RewardTypes,IRewardData> RewardValues { get; }
        public Dictionary<RewardTypes,IRewardData> PenaltyValues { get; }
        ComplianceStatus ComplianceStatus { get; }
        string GetTitle { get; }
        string GetSubtitle { get; }
        string GetDescription { get; }
        public void SetComplianceStatus(ComplianceStatus status);
    }
}