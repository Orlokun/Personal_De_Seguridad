using GameDirection.TimeOfDayManagement;
using GamePlayManagement.BitDescriptions.RequestParameters;
using GamePlayManagement.BitDescriptions.Suppliers;

namespace DataUnits.GameRequests
{
    public class HireGameRequest : GameRequest, IHireGameRequest
    {
        private JobSupplierBitId _mSupplierRequired;
        public JobSupplierBitId JobHireObjective => _mSupplierRequired;
        public HireGameRequest(
            int requesterSpeakId, int reqId, string reqTitle, string reqDescription, RequirementActionType mChallengeType,
            RequirementObjectType objectTypeRequired, RequirementLogicEvaluator mReqLogic, RequirementConsideredParameter mReqParameterType, 
            JobSupplierBitId mSupplierRequired, int quantity, string[] rewards, string[] penalties, DayBitId targetDayId, PartOfDay targetPartOfday) 
            : base(requesterSpeakId, reqId, reqTitle, reqDescription, mChallengeType, objectTypeRequired, mReqLogic, 
                mReqParameterType, quantity, rewards, penalties, targetDayId, targetPartOfday)
        {
            _mSupplierRequired = mSupplierRequired;
        }
    }


}