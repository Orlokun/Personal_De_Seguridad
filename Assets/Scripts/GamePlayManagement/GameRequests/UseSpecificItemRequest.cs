﻿using GameDirection.TimeOfDayManagement;
using GamePlayManagement.BitDescriptions.RequestParameters;
using GamePlayManagement.BitDescriptions.Suppliers;

namespace GamePlayManagement.GameRequests
{
    public class UseSpecificItemRequest : GameRequest, IUseSpecificItemRequest
    {
        private int _mItemBitId;
        private BitItemSupplier _mItemOwner;

        public BitItemSupplier ItemOwner => _mItemOwner;
        public int ItemId => _mItemBitId;
        
        
        public UseSpecificItemRequest(int requesterSpeakId, int reqId, string reqTitle, string reqDescription, 
            RequirementActionType mChallengeType, RequirementObjectType objectTypeRequired, RequirementLogicEvaluator mReqLogic,
            RequirementConsideredParameter mReqParameterType, BitItemSupplier itemOwner, int itemBitId, 
            int quantity, string[] rewards, string[] penalties, DayBitId targetDayId, PartOfDay targetPartOfDay) 
            : base(requesterSpeakId, reqId, reqTitle, reqDescription, mChallengeType, objectTypeRequired, mReqLogic, 
                mReqParameterType, quantity, rewards, penalties, targetDayId, targetPartOfDay)
        {
            _mItemOwner = itemOwner;
            _mItemBitId = itemBitId;
        }
    }
}