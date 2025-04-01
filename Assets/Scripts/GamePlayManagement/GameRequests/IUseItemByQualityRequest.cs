using System.Collections.Generic;
using DataUnits.ItemScriptableObjects;

namespace GamePlayManagement.GameRequests
{
    public interface IUseItemByQualityRequest
    {
        public List<ItemBaseQuality> RequestConsideredQuality  { get; }
    }
}