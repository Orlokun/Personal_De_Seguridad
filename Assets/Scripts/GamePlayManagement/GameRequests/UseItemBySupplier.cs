using System;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement.BitDescriptions.RequestParameters;
using GamePlayManagement.BitDescriptions.Suppliers;
using UnityEngine;

namespace GamePlayManagement.GameRequests
{
    public class UseItemBySupplier : GameRequest, IUseItemBySupplierRequest
    {
        public BitItemSupplier RequestConsideredSupplier => _mRequestConsideredSupplier;
        private BitItemSupplier _mRequestConsideredSupplier;
        public UseItemBySupplier(int speakerId, int reqId, string reqTitle, string reqDescription, RequirementActionType requestType, 
            RequirementObjectType requiredObjectType, RequirementLogicEvaluator requirementLogicEvaluator, RequirementConsideredParameter requestParameterType,
            int quantity, string[] rewards, string[] punishment, DayBitId targetDayId, PartOfDay targetPartOfDay, string[] requestParameterValue) 
            : base(speakerId, reqId, reqTitle, reqDescription, requestType, requiredObjectType, requirementLogicEvaluator, requestParameterType, 
                quantity, rewards, punishment, targetDayId, targetPartOfDay)
        {
            ProcessItemBySupplierRequest(requestParameterValue);
        }
        
        private void ProcessItemBySupplierRequest(string[] requestParameterValue)
        {
            _mRequestConsideredSupplier = 0;
            foreach (var supplierConsidered in requestParameterValue)
            {
                var isSupplierId = Enum.TryParse<BitItemSupplier>(supplierConsidered, out BitItemSupplier result);
                if (!isSupplierId)
                {
                    Debug.LogError("[UseItemByQualityRequest.ProcessItemBySupplierRequest] Supplier ID must be Bit Operator Number");
                }
                _mRequestConsideredSupplier = result;
            }
        }
    }
}