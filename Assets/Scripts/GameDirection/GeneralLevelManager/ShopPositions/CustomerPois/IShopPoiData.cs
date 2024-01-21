using System;
using GamePlayManagement.LevelManagement.LevelObjectsManagement;
using UnityEngine;
using Utils;

namespace GameDirection.GeneralLevelManager.ShopPositions.CustomerPois
{
    public interface IShopPoiData : IInitializeWithArg2<Guid, Guid>
    {
        public Guid PoiId { get; }
        public Guid GetShelfId { get; }
        public Vector3 GetPosition { get; }
        public bool IsOccupied { get; }
        public void OccupyPoi(Guid occupier);
        public void LeavePoi(Guid leavingId);
        public Guid OccupierId { get; }
        public Tuple<Transform, IStoreProductObjectData> ChooseRandomProduct();
    }
}