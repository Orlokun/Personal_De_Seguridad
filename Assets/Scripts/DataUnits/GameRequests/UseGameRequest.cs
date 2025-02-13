using GamePlayManagement.BitDescriptions.RequestParameters;
using GamePlayManagement.BitDescriptions.Suppliers;

namespace DataUnits.GameRequests
{
    public interface IUseGameRequest : IGameRequest
    {
        public BitItemSupplier ItemOwner { get; }
        public int ItemId { get; }
    }
    public class UseGameRequest : GameRequest, IUseGameRequest
    {
        private int _mItemBitId;
        private BitItemSupplier _mItemOwner;

        public BitItemSupplier ItemOwner => _mItemOwner;
        public int ItemId => _mItemBitId;
        
        
        public UseGameRequest(int requesterSpeakId, int reqId, string reqTitle, 
            string reqDescription, RequirementActionType mChallengeType, RequirementObjectType objectTypeRequired, 
            RequirementLogicEvaluator mReqLogic, RequirementConsideredParameter mReqParameterType, BitItemSupplier itemOwner, int itemBitId, int quantity) 
            : base(requesterSpeakId, reqId, reqTitle, reqDescription, mChallengeType, objectTypeRequired, mReqLogic, mReqParameterType, quantity)
        {
            _mItemOwner = itemOwner;
            _mItemBitId = itemBitId;
        }
    }
}