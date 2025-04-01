using System;
using GamePlayManagement.ItemManagement.Guards;
using UnityEngine;

namespace GameDirection.GeneralLevelManager.ShopPositions
{
    public interface IShopInspectionSpots
    {
        public IShopInspectionPosition GetClosestPosition(Vector3 originSpot);
        public IShopInspectionPosition GetNextPosition(Guid currentSpotId);
    }
}