using System;
using System.Collections.Generic;
using GameDirection.GeneralLevelManager;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GamePlayManagement.LevelManagement.LevelObjectsManagement
{
    public class ShelfInMarket : MonoBehaviour, IShelfInMarket
    {
        [SerializeField] private List<ProductInShelf> productPrefabs;
        [SerializeField] private Transform customerPoI;
        [SerializeField] private List<Transform> positionTranforms;

        private IShopPoiData customerPoIData;   //Shop Poi Data to know where the client should go
        private Dictionary<int, ProductPositionInShelf> _productPositionsInShelf = new Dictionary<int, ProductPositionInShelf>();
        private Dictionary<int, ProductInShelf> _productsInShelf = new Dictionary<int, ProductInShelf>();
        private Transform _getRandomProductPosition;

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
            customerPoIData = customerPoI.GetComponent<ShopPoiObject>();
            if (customerPoIData == null)
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
            for (var i = 0; i < _productPositionsInShelf.Count; i++)
            {
                //Set 0 or 1
                var prefabIndex = i % 2 == 0 ? 0 : 1; 
                var randomPrefab = productPrefabs[prefabIndex];
                Vector3 posInShelf = _productPositionsInShelf[i].PositionInShelf;
                var productCreated = Instantiate(randomPrefab.gameObject, posInShelf, new Quaternion());
                _productsInShelf.Add(i, productCreated.GetComponent<ProductInShelf>());
            }
        }

        public ShopPoiObject GetCustomerPoI => customerPoI.GetComponent<ShopPoiObject>();
        
        public IProductInShelf GetRandomProductPosition()
        {
            Random.InitState(DateTime.Now.Millisecond);
            var randomIndex = Random.Range(0, _productsInShelf.Count-1);
            return _productsInShelf[randomIndex];
        }
    }
}