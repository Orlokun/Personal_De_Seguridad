using System;
using System.Collections.Generic;
using GameDirection.GeneralLevelManager;
using GameDirection.GeneralLevelManager.ShopPositions;
using GameDirection.GeneralLevelManager.ShopPositions.CustomerPois;
using Utils;

namespace GamePlayManagement.LevelManagement.LevelObjectsManagement
{
    public interface IShelfInMarket : IInitialize
    {
        public bool IsAnyPoiAvailable();
        public IShopPoiData ReturnAvailablePoi();
        public IShopPoiData GetCustomerPoi(Guid poiId);
        public List<ShopPoiObject> GetAllPois { get; }        
        public Guid ShelfId { get; }
    }
}