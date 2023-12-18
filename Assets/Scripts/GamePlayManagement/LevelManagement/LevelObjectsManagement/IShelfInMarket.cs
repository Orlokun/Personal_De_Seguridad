using System;
using GameDirection.GeneralLevelManager;

namespace GamePlayManagement.LevelManagement.LevelObjectsManagement
{
    public interface IShelfInMarket
    {
        ShopPoiObject GetCustomerPoI { get; }
        IStoreProduct ChooseRandomProduct();
        public Guid ShelfId { get; }
    }
}