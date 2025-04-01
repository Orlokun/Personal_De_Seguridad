using System.Collections.Generic;
using UnityEngine;

namespace GameDirection.GeneralLevelManager.ShopPositions
{
    public interface IShopPositionsManager : IShopClientPoisManager, IShopGenericWaitingSpots, IShopInspectionSpots
    {
        public void Initialize();
        public List<IStoreEntrancePosition> StartPositions { get; }
        public Vector3 PayingPosition();
    }
}