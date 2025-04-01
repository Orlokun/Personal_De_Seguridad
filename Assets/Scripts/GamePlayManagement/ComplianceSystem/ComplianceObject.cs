using GameDirection.TimeOfDayManagement;
using GamePlayManagement.BitDescriptions.RequestParameters;

namespace GamePlayManagement.ComplianceSystem
{
    public class ComplianceObject : IComplianceObject
    {
        protected IComplianceObjectData MComplianceObjectData;
        private int _mComplianceActionCount;
        public int ComplianceCurrentCount => _mComplianceActionCount;
        public IComplianceObjectData GetComplianceObjectData => MComplianceObjectData;
        public void MarkAsActive()
        {
            MComplianceObjectData.SetComplianceStatus(ComplianceStatus.Active);
        }

        public void MarkOneAction()
        {
            _mComplianceActionCount++;
        }

        public ComplianceObject(int complianceId, DayBitId startDayId, DayBitId endDayId, bool needsUnlock, 
            ComplianceMotivationalLevel motivationLvl, ComplianceActionType actionType, ComplianceObjectType objectType, 
            RequirementConsideredParameter consideredParameter,string[] complianceReqValues,int toleranceValue, string[] rewardValues, string[] penaltyValues, string title, string subtitle, string description, RequirementLogicEvaluator logicEvaluator)
        {
            MComplianceObjectData = new ComplianceObjectData(complianceId, startDayId, endDayId, needsUnlock, motivationLvl, actionType, objectType, consideredParameter, complianceReqValues, toleranceValue, rewardValues, penaltyValues, title, subtitle, description, logicEvaluator);
        }
        public bool IsToleranceLevelReached => _mComplianceActionCount >= MComplianceObjectData.ToleranceValue;
        public void ProcessEndCompliance()
        {
            if(MComplianceObjectData.MotivationLvl == ComplianceMotivationalLevel.Forbidden || MComplianceObjectData.MotivationLvl == ComplianceMotivationalLevel.Discouraged || 
               MComplianceObjectData.ActionType == ComplianceActionType.NotUse)
            {
                MComplianceObjectData.SetComplianceStatus(ComplianceStatus.Failed);
            }
            else
            {
                MComplianceObjectData.SetComplianceStatus(ComplianceStatus.Passed);
            }
        }
    }
}