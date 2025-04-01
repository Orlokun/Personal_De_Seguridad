using System;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement.BitDescriptions.RequestParameters;
using GamePlayManagement.BitDescriptions.Suppliers;
using UnityEngine;

namespace GamePlayManagement.GameRequests
{
    public class UseExactItemRequest : GameRequest, IUseExactItemRequest
    {
        public BitItemSupplier RequestConsideredSupplier => _mRequestConsideredSupplier;
        private BitItemSupplier _mRequestConsideredSupplier;

        public int RequestConsideredItemId => _mItemId;
        private int _mItemId;
        public UseExactItemRequest(int speakerId, int reqId, string reqTitle, string reqDescription, RequirementActionType requestType, 
            RequirementObjectType requiredObjectType, RequirementLogicEvaluator requirementLogicEvaluator, RequirementConsideredParameter requestParameterType,
            int quantity, string[] rewards, string[] punishment, DayBitId targetDayId, PartOfDay targetPartOfDay, string[] requestParameterValue) 
            : base(speakerId, reqId, reqTitle, reqDescription, requestType, requiredObjectType, requirementLogicEvaluator, requestParameterType, 
                quantity, rewards, punishment, targetDayId, targetPartOfDay)
        {
            ProcessExactItemRequest(requestParameterValue);
        }
        
        private void ProcessExactItemRequest(string[] requestParameterValue)
        {
            if (requestParameterValue.Length != 2)
            {
                Debug.LogError("[UseExactItemRequest.ProcessExactItemRequest] Exact Item Request must have only two elements.");
            }
            var isSupplierId = Enum.TryParse<BitItemSupplier>(requestParameterValue[0], out BitItemSupplier itemSupplierId);
            if (!isSupplierId)
            {
                Debug.LogError("[UseItemByQualityRequest.ProcessItemBySupplierRequest] Supplier ID must be Bit Operator Number");
                return;
            }
            _mRequestConsideredSupplier = itemSupplierId;
            
            var isItemId = int.TryParse(requestParameterValue[1], out int result);
            if (!isItemId)
            {
                Debug.LogError("[UseExactItemRequest.ProcessExactItemRequest] Item ID must be integer");
                return;
            }
            _mItemId = result;
        }
    }
    
    public class UseItemWithVariableRequest : GameRequest, IUseItemWithVariableRequest
    {
        public BitItemSupplier RequestConsideredSupplier => _mRequestConsideredSupplier;
        private BitItemSupplier _mRequestConsideredSupplier;

        public int RequestConsideredItemId => _mItemId;
        private int _mItemId;
        public UseItemWithVariableRequest(int speakerId, int reqId, string reqTitle, string reqDescription, RequirementActionType requestType, 
            RequirementObjectType requiredObjectType, RequirementLogicEvaluator requirementLogicEvaluator, RequirementConsideredParameter requestParameterType,
            int quantity, string[] rewards, string[] punishment, DayBitId targetDayId, PartOfDay targetPartOfDay, string[] requestParameterValue) 
            : base(speakerId, reqId, reqTitle, reqDescription, requestType, requiredObjectType, requirementLogicEvaluator, requestParameterType, 
                quantity, rewards, punishment, targetDayId, targetPartOfDay)
        {
            ProcessExactItemRequest(requestParameterValue);
        }
        
        private void ProcessExactItemRequest(string[] requestParameterValue)
        {
            if (requestParameterValue.Length != 2)
            {
                Debug.LogError("[UseExactItemRequest.ProcessExactItemRequest] Exact Item Request must have only two elements.");
            }
            var isSupplierId = Enum.TryParse<BitItemSupplier>(requestParameterValue[0], out BitItemSupplier itemSupplierId);
            if (!isSupplierId)
            {
                Debug.LogError("[UseItemByQualityRequest.ProcessItemBySupplierRequest] Supplier ID must be Bit Operator Number");
                return;
            }
            _mRequestConsideredSupplier = itemSupplierId;
            
            var isItemId = int.TryParse(requestParameterValue[1], out int result);
            if (!isItemId)
            {
                Debug.LogError("[UseExactItemRequest.ProcessExactItemRequest] Item ID must be integer");
                return;
            }
            _mItemId = result;
        }
    }
}