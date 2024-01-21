using System;
using System.Collections.Generic;
using DataUnits.JobSources;
using GamePlayManagement.LevelManagement.LevelObjectsManagement;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameDirection.GeneralLevelManager.ShopPositions.CustomerPois
{
    public class ShopPoiObject : MonoBehaviour, IShopPoiData 
    {
        private const string ProductPrefabPath = "LevelManagementPrefabs/ProductPrefabs/";
        private bool _mIsOccupied;
        private Guid _occupierId;
        private Guid _poiId;
        private Guid _shelfId;
        public Guid PoiId => _poiId;
        public Guid OccupierId => _occupierId;

        [SerializeField] private List<Transform> productTranforms;
        [SerializeField] private ProductsLevelEden[] poiProducts;
        private IJobSupplierObject _jobSupplier;

        public Tuple<Transform, IStoreProductObjectData> ChooseRandomProduct()
        {
            Random.InitState(DateTime.Now.Millisecond);
            var randomIndex = Random.Range(0, poiProducts.Length-1);
            var randomPoiProduct = poiProducts[randomIndex];
            var productsInStore = _jobSupplier.JobProductsModule.ProductsInStore;
            if (!productsInStore.ContainsKey(randomPoiProduct))
            {
                Debug.LogError("[ShopPoiObject.ChooseRandomProduct] Product must exist in data");
                return null;
            }
            var productData = productsInStore[randomPoiProduct];
            var searchTransform = productTranforms[randomIndex];
            return new Tuple<Transform, IStoreProductObjectData>(searchTransform, productData);
        }

        public Guid GetShelfId => _shelfId;
        public bool IsOccupied => _mIsOccupied;
        public Vector3 GetPosition => transform.position;

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

        private bool _mIsInitialized;
        public bool IsInitialized => _mIsInitialized;

        /// <summary>
        /// Injection class 1 is shelveId
        /// Injection class 2 is PoiId
        /// </summary>
        /// <param name="injectionClass1"></param>
        /// <param name="injectionClass2"></param>
        public void Initialize(Guid injectionClass1, Guid injectionClass2)
        {
            Debug.Log($"[ShopPoiObject.Initialize] Initialize Poi: {gameObject.name}");
            if (_mIsInitialized)
            {
                return;
            }
            _jobSupplier = GameDirector.Instance.GetActiveGameProfile.GetActiveJobsModule().CurrentEmployerData();
            _shelfId = injectionClass1;
            _poiId = injectionClass2;
            _mIsInitialized = true;
        }
    }
}