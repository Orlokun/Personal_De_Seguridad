using System;
using UnityEngine;

namespace GameDirection.GeneralLevelManager
{
    public interface IShopPoiData
    {
        public void AssignShelf(Guid shelfId);
        public Guid GetShelfId { get; }
        public Vector3 GetPosition { get; }
        public bool IsOccupied { get; }
        public void OccupyPoi(Guid occupier);
        public void LeavePoi(Guid leavingId);
    }
}