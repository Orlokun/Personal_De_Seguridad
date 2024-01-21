using System;
using UnityEngine;
using Utils;

namespace GameDirection.GeneralLevelManager.ShopPositions.WaitingPositions
{
    public interface ISingleWaitingSpot : IInitializeWithArg1<IShopWaitingSpots>
    {
        public Guid SpotId { get; }
        public Vector3 Position { get; }
        public Guid CharacterInPlace { get; }
        public bool IsOccupied { get; }
        public Tuple<ISingleWaitingSpot, bool> OccupyWaitingSpot(Guid occupierId);
        public void ReleaseWaitingSpot(Guid occupierId);
    }
    
}