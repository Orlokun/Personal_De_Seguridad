using System;
using System.Collections.Generic;
using System.Linq;
using GameDirection.GeneralLevelManager;
using GameDirection.GeneralLevelManager.ShopPositions;
using GameDirection.GeneralLevelManager.ShopPositions.CustomerPois;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GamePlayManagement.LevelManagement.LevelObjectsManagement
{
    public class ShelfInMarket : MonoBehaviour, IShelfInMarket 
    {
        [SerializeField] private List<ShopPoiObject> customerPoIsData;   //Shop Poi Data to know where the client should go
        private Dictionary<int, ProductPositionInShelf> _productPositionsInShelf = new Dictionary<int, ProductPositionInShelf>();
        private Dictionary<int, IStoreProduct> _productsInShelf = new Dictionary<int, IStoreProduct>();
        
        private Guid _mShelfId;
        public Guid ShelfId => _mShelfId;
        public bool IsInitialized => _mIsInitialized;
        private bool _mIsInitialized;
        
        public void Initialize()
        {
            if (_mIsInitialized)
            {
                return;
            }
            _mShelfId = Guid.NewGuid();
            ConfirmPoIs();
            _mIsInitialized = true;
        }
        private void ConfirmPoIs()
        {
            if (customerPoIsData.Count <= 0)
            {
                Debug.LogError($"[ConfirmPoi] Shelf named {gameObject.name} must have a customer PoI");
                return;
            }
            for (int i = 0; i < customerPoIsData.Count; i++)
            {
                var poiData = customerPoIsData[i];
                var poiGuid = Guid.NewGuid();
                poiData.Initialize(ShelfId, poiGuid);
            }
        }

        private int RandomProductIndex()
        {
            Random.InitState(DateTime.Now.Millisecond);
            var randomIndex = Random.Range(1, 19);
            var returnId = 1;
            for (var i = 1; i < randomIndex; i++)
            {
                returnId *= 2;
            }
            return returnId;
        }

        public bool IsAnyPoiAvailable()
        {
            return customerPoIsData.Any(x => x.IsOccupied != true);
        }
        public IShopPoiData ReturnAvailablePoi()
        {
            return customerPoIsData.FirstOrDefault(x => x.IsOccupied != true);
        }
        
        public IShopPoiData GetCustomerPoi(Guid poiId)
        {
            if(customerPoIsData.All(x => x.PoiId != poiId))
            {
                Debug.LogWarning("[shelfInMarket.GetCustomerPoi] Poi Guid must be available");
                return null;
            }
            return customerPoIsData.SingleOrDefault(x => x.PoiId == poiId);
        }
        public List<ShopPoiObject> GetAllPois => customerPoIsData;
        
        public IStoreProduct ChooseRandomProduct()
        {
            Random.InitState(DateTime.Now.Millisecond);
            var randomIndex = Random.Range(0, _productsInShelf.Count-1);
            return _productsInShelf[randomIndex];
        }
    }
}