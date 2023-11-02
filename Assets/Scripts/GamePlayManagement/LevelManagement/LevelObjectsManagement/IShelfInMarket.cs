using GameDirection.GeneralLevelManager;
using UnityEngine;

namespace GamePlayManagement.LevelManagement.LevelObjectsManagement
{
    public interface IShelfInMarket
    {
        ShopPoiObject GetCustomerPoI { get; }
        IProductInShelf GetRandomProductPosition();
    }
}