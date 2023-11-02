using UnityEngine;

namespace GameDirection.GeneralLevelManager
{
    public interface IMovingPointOfInterest : IShopPoiData
    {
        public Vector3 SecondPosition { get; }
    }
}