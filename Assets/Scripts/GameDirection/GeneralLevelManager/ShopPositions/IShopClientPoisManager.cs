using System;
using System.Collections.Generic;
using GameDirection.GeneralLevelManager.ShopPositions.CustomerPois;

namespace GameDirection.GeneralLevelManager.ShopPositions
{
    public interface IShopClientPoisManager
    {
        public List<Guid> GetFirstPoiOfInterestIds(int numberOfPois);
        public List<IShopPoiData> GetPoisOfInterestData(List<Guid> poisId);
        public void OccupyPoi(Guid occupier, Guid occupiedPoi);
        public void ReleasePoi(Guid occupier, Guid occupiedPoi);
        public IShopPoiData GetPoiData(Guid poiId);
    }
}