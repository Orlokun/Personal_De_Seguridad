using System;
using System.Collections.Generic;
using DataUnits.ItemScriptableObjects;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement.BitDescriptions.RequestParameters;

namespace GamePlayManagement.GameRequests
{
    public class UseItemByQualityRequest : GameRequest, IUseItemByQualityRequest
    {
        public List<ItemBaseQuality> RequestConsideredQuality => _mRequestConsideredQuality;
        private List<ItemBaseQuality> _mRequestConsideredQuality;
        public UseItemByQualityRequest(int speakerId, int reqId, string reqTitle, string reqDescription, RequirementActionType requestType, 
            RequirementObjectType requiredObjectType, RequirementLogicEvaluator requirementLogicEvaluator, RequirementConsideredParameter requestParameterType,
            int quantity, string[] rewards, string[] punishment, DayBitId targetDayId, PartOfDay targetPartOfDay, string[] requestParameterValue) 
            : base(speakerId, reqId, reqTitle, reqDescription, requestType, requiredObjectType, requirementLogicEvaluator, requestParameterType, 
                quantity, rewards, punishment, targetDayId, targetPartOfDay)
        {
            ProcessItemByQualityRequest(requestParameterValue);
        }
        
        private void ProcessItemByQualityRequest(string[] requestParameterValue)
        {
            _mRequestConsideredQuality = new List<ItemBaseQuality>();
            foreach (var typeName in requestParameterValue)
            {
                var isQualitytype = Enum.TryParse(typeName, out ItemBaseQuality itemQuality);
                if (!isQualitytype)
                {
                    continue;
                }
                _mRequestConsideredQuality.Add(itemQuality);
            }
        }
    }
}