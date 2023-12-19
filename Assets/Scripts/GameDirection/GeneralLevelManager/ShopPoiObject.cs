using System;
using UnityEngine;

namespace GameDirection.GeneralLevelManager
{
    public class ShopPoiObject : MonoBehaviour, IShopPoiData
    {
        private bool _mIsOccupied;

        private Guid _occupierId;
        public Guid OccupierId => _occupierId;

        private Guid _shelfId;
        public Guid GetShelfId => _shelfId;
        public bool IsOccupied => _mIsOccupied;

        public Vector3 GetPosition => transform.position;

        public void AssignShelf(Guid shelfId)
        {
            _shelfId = shelfId;
        }
        public void OccupyPoi(Guid occupier)
        {
            if (occupier == _occupierId || IsOccupied)
            {
                return;
            }
            OccupyData(occupier);
        }
        public void LeavePoi(Guid leavingId)
        {
            if (leavingId != _occupierId)
            {
                return;
            }
            EmptyData();
        }
        private void OccupyData(Guid occupier)
        {
            _occupierId = occupier;
            _mIsOccupied = true;
        }
        private void EmptyData()
        {
            _occupierId = new Guid();
            _mIsOccupied = false;
        }
    }
}