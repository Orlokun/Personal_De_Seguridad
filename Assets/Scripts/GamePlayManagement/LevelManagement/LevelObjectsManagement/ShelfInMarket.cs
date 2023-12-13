using System;
using System.Collections.Generic;
using GameDirection;
using GameDirection.GeneralLevelManager;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GamePlayManagement.LevelManagement.LevelObjectsManagement
{
    public class ShelfInMarket : MonoBehaviour, IShelfInMarket
    {
        [SerializeField] private Transform customerPoI;
        [SerializeField] private List<Transform> positionTranforms;

        private const string ProductPrefabPath = "LevelManagementPrefabs/ProductPrefabs/";
        private IShopPoiData _customerPoIData;   //Shop Poi Data to know where the client should go
        private Dictionary<int, ProductPositionInShelf> _productPositionsInShelf = new Dictionary<int, ProductPositionInShelf>();
        private Dictionary<int, IStoreProduct> _productsInShelf = new Dictionary<int, IStoreProduct>();

        private void Awake()
        {
            ConfirmPoI();
            PopulateProductPositions();
            PopulateProductsInShelf();
        }

        private void ConfirmPoI()
        {
            if (customerPoI == null)
            {
                Debug.LogError($"[ConfirmPoi] Shelf named {gameObject.name} must have a customer PoI");
                return;
            }
            _customerPoIData = customerPoI.GetComponent<ShopPoiObject>();
            if (_customerPoIData == null)
            {
                Debug.LogError($"[ConfirmPoi] ShopPoiObject must be a component of the passed transform gameObject.");
                return;
            }
        }

        private void PopulateProductPositions()
        {
            for (var i = 0; i < positionTranforms.Count; i++)
            {
                var positionTransform = positionTranforms[i];
                var newProductPosition = new ProductPositionInShelf(positionTransform);
                _productPositionsInShelf.Add(i,newProductPosition);
            }
            positionTranforms.Clear();
        }
        
        private void PopulateProductsInShelf()
        {
            Debug.Log($"[PopulateProductsInShelf] Start Populating Products");
            var activeJobsModule = GameDirector.Instance.GetActiveGameProfile.GetActiveJobsModule();
            var currentSupplierId = activeJobsModule.CurrentEmployer;
            var storeProducts = activeJobsModule.JobObjects[currentSupplierId].JobProductsModule.ProductsInStore;
            
            for (var i = 0; i < _productPositionsInShelf.Count; i++)
            {
                //Set 0 or 1
                var randomIndex = RandomProductIndex();
                var posInShelf = _productPositionsInShelf[i].PositionInShelf;
                var randomProductPrefabName = storeProducts[randomIndex].PrefabName;
                var path = ProductPrefabPath + randomProductPrefabName;
                Debug.Log($"[PopulateProductsInShelf]Start Instantiate of {randomProductPrefabName}");
                var productCreated = (GameObject)Instantiate(Resources.Load(path), posInShelf, new Quaternion(), transform);
                var storeProduct = (IStoreProduct)productCreated.AddComponent<StoreProductGameObject>();
                storeProduct.SetStoreProductGameObjectData(storeProducts[randomIndex], posInShelf);
                _productsInShelf.Add(i, storeProduct);
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
        
        
        public ShopPoiObject GetCustomerPoI => customerPoI.GetComponent<ShopPoiObject>();
        
        public IStoreProduct GetRandomProductPosition()
        {
            Random.InitState(DateTime.Now.Millisecond);
            var randomIndex = Random.Range(0, _productsInShelf.Count-1);
            return _productsInShelf[randomIndex];
        }
    }
}