using UnityEngine;

namespace GameDirection.GeneralLevelManager.ShopPositions.CustomerPois
{
    public interface IMovingPointOfInterest : IShopPoiData
    {
        public Vector3 SecondPosition { get; }
    }
}