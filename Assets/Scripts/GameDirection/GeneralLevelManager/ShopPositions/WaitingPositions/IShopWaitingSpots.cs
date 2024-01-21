using System;
using System.Collections.Generic;

namespace GameDirection.GeneralLevelManager.ShopPositions.WaitingPositions
{
    public interface IShopWaitingSpots
    {
        public Guid WaitingSpotId { get; }
        public bool AnySpotAvailable { get; }
        public List<ISingleWaitingSpot> WaitingSpots { get; }
        public Tuple<ISingleWaitingSpot, bool> OccupyWaitingSpot(Guid occupier);
        public void LeaveWaitingSpot(Guid occupier);
    }
    
    
    public class WaitingSpotStatus : IWaitingPointStatus
    {
        public WaitingSpotStatus()
        {
            
        }
    }

    public interface IWaitingPointStatus
    {
        
    }
}