using System;
using System.Collections.Generic;
using DataUnits.ItemScriptableObjects;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement.BitDescriptions.RequestParameters;

namespace GamePlayManagement.GameRequests
{
    public class UseItemByBaseTypeRequest : GameRequest, IUseItemByBaseTypeRequest
    {
        public List<ItemBaseRace> RequestConsideredTypes => _mRequestConsideredBaseTypes;
        private List<ItemBaseRace> _mRequestConsideredBaseTypes;
        public UseItemByBaseTypeRequest(int speakerId, int reqId, string reqTitle, string reqDescription, RequirementActionType requestType, 
            RequirementObjectType requiredObjectType, RequirementLogicEvaluator requirementLogicEvaluator, RequirementConsideredParameter requestParameterType,
            int quantity, string[] rewards, string[] punishment, DayBitId targetDayId, PartOfDay targetPartOfDay, string[] requestParameterValue) 
            : base(speakerId, reqId, reqTitle, reqDescription, requestType, requiredObjectType, requirementLogicEvaluator, requestParameterType, 
                quantity, rewards, punishment, targetDayId, targetPartOfDay)
        {
            ProcessItemByBaseTypeRequest(requestParameterValue);
        }
        
        private void ProcessItemByBaseTypeRequest(string[] requestParameterValue)
        {
            _mRequestConsideredBaseTypes = new List<ItemBaseRace>();
            foreach (var typeName in requestParameterValue)
            {
                var isBaseType = Enum.TryParse(typeName, out ItemBaseRace itemBaseType);
                if (!isBaseType)
                {
                    continue;
                }
                _mRequestConsideredBaseTypes.Add(itemBaseType);
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