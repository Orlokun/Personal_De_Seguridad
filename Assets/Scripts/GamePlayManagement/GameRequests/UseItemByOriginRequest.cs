using System;
using System.Collections.Generic;
using DataUnits.ItemScriptableObjects;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement.BitDescriptions.RequestParameters;

namespace GamePlayManagement.GameRequests
{
    public class UseItemByOriginRequest : GameRequest, IUseItemByOriginRequest
    {
        public List<ItemOrigin> RequestConsideredOrigins => _mRequestConsideredOrigins;

        private List<ItemOrigin> _mRequestConsideredOrigins;
        
        public UseItemByOriginRequest(int requesterSpeakId, int reqId, string reqTitle, string reqDescription,
            RequirementActionType mChallengeType, RequirementObjectType objectTypeRequired,
            RequirementLogicEvaluator mReqLogic,
            RequirementConsideredParameter mReqParameterType,
            int quantity, string[] rewards, string[] penalties, DayBitId targetDayId, PartOfDay targetPartOfDay,
            string[] requestParameterValue) 
            : base(requesterSpeakId, reqId, reqTitle, reqDescription, mChallengeType, objectTypeRequired, mReqLogic, 
                mReqParameterType, quantity, rewards, penalties, targetDayId, targetPartOfDay)
        {
            _mRequestConsideredOrigins = new List<ItemOrigin>();
            LoadOriginList(requestParameterValue);
        }

        private void LoadOriginList(string[] requestParameterValue)
        {
            foreach (var origin in requestParameterValue)
            {
                var isOrigin = Enum.TryParse(origin, out ItemOrigin itemOrigin);
                if (!isOrigin)
                {
                    continue;
                }
                _mRequestConsideredOrigins.Add(itemOrigin);
            }
        }

        public override void ProcessCompletionInObjectData()
        {
            if (MRequestDevelopmentCount < MRequestData.RequirementQuantityTolerance)
            {
                return;
            }
            switch (MRequestData.ChallengeType)
            {
                case RequirementActionType.Use:
                    MarkAsCompleted();
                    break;
                case RequirementActionType.NotUse:
                    MarkAsFailed();
                    break;
            }
        }
    }
}