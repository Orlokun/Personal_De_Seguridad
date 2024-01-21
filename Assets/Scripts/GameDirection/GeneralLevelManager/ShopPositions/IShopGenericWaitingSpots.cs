using System;
using System.Threading.Tasks;
using GameDirection.GeneralLevelManager.ShopPositions.WaitingPositions;

namespace GameDirection.GeneralLevelManager.ShopPositions
{
    public interface IShopGenericWaitingSpots
    {
        public IShopWaitingSpots GetWaitingSpot(Guid poiId);
        public bool AnyUnoccupiedSpot { get; }
        public Task<Tuple<ISingleWaitingSpot, bool>> OccupyEmptyWaitingSpot(Guid occupierCharacter);

    }
}