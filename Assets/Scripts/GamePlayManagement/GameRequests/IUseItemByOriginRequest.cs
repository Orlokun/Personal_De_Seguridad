using System.Collections.Generic;
using DataUnits.ItemScriptableObjects;

namespace GamePlayManagement.GameRequests
{
    public interface IUseItemByOriginRequest : IGameRequest
    {
        public List<ItemOrigin> RequestConsideredOrigins { get; }
    }
}