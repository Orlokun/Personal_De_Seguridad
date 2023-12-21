using System.Collections;
using System.Collections.Generic;
using DataUnits.GameCatalogues.JsonCatalogueLoaders;
using GameDirection;
using GamePlayManagement.LevelManagement.LevelObjectsManagement;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Utils;

namespace DataUnits.JobSources
{
    public class JobSupplierProductsModule : IJobSupplierProductsModule
    {
        private JobSupplierObject _supplierObject;
        public JobSupplierProductsModule(JobSupplierObject supplierObject)
        {
            _supplierObject = supplierObject;
        }
        
        private StoreProductsDataString _mProductsDataString;
        private Dictionary<ProductsLevelEden, IStoreProductObjectData> _mProductsInStore;
        public Dictionary<ProductsLevelEden, IStoreProductObjectData> ProductsInStore => _mProductsInStore;
        public void LoadProductData()
        {
            Debug.Log($"START: Collecting Product data for {_supplierObject.StoreName}");
            var url = DataSheetUrls.GetStoreProducts(_supplierObject.JobSupplierBitId);
            //TODO: Take this out of Dialogue operator
            GameDirector.Instance.ActCoroutine(LoadProductsFromServer(url));
        }
        private IEnumerator LoadProductsFromServer(string url)
        {
            //
            var webRequest = UnityWebRequest.Get(url);
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Jobs Catalogue Data for {_supplierObject.StoreName} must be reachable. Error: {webRequest.result}. {webRequest.error}");
            }
            else
            {
                var sourceJson = webRequest.downloadHandler.text;
                LoadProductsFromJson(sourceJson);
            }
        }
        private void LoadProductsFromJson(string sourceJson)
        {
            _mProductsInStore = new Dictionary<ProductsLevelEden, IStoreProductObjectData>();
            Debug.Log($"[LoadProductsFromJson] Start Serializing Job supplier's {_supplierObject.StoreName} Product Json data");
            _mProductsDataString = JsonConvert.DeserializeObject<StoreProductsDataString>(sourceJson);
            for (var i = 1; i < _mProductsDataString.values.Count; i++)
            {
                var gotId = int.TryParse(_mProductsDataString.values[i][0], out var productId);
                var productBitId = (ProductsLevelEden) productId;
                if (productId == 0 || !gotId)
                {
                    Debug.LogWarning(
                        $"[JobSupplierObject.LoadProductsFromJson] Product for {_supplierObject.StoreName} must have Id greater than zero");
                    return;
                }
                var productName = _mProductsDataString.values[i][1]; 
                
                var gotType = int.TryParse(_mProductsDataString.values[i][2], out var productType);
                if (productType == 0 || !gotType)
                {
                    Debug.LogWarning(
                        $"[JobSupplierObject.LoadProductsFromJson] Product named {productName} for {_supplierObject.StoreName} must have " +
                        $"Type greater than zero");
                    return;
                }
                
                var gotQuantity = int.TryParse(_mProductsDataString.values[i][3], out var productQuantity);
                if (productQuantity == 0 || !gotQuantity)
                {
                    Debug.LogWarning(
                        $"[JobSupplierObject.LoadProductsFromJson] Product named {productName} for {_supplierObject.StoreName} must have " +
                        $"Quantity greater than zero");
                    return;
                }
                
                var gotPrice = int.TryParse(_mProductsDataString.values[i][4], out var productPrice);
                if (productPrice == 0 || !gotPrice)
                {
                    Debug.LogWarning(
                        $"[JobSupplierObject.LoadProductsFromJson] Product named {productName} for {_supplierObject.StoreName} must have " +
                        $"Price greater than zero");
                    return;
                }
                var gotHideValue = int.TryParse(_mProductsDataString.values[i][5], out var productHideValue);
                if (productHideValue == 0 || !gotHideValue)
                {
                    Debug.LogWarning(
                        $"[JobSupplierObject.LoadProductsFromJson] Product named {productName} for {_supplierObject.StoreName} must have " +
                        $"HideValue greater than zero");
                    return;
                }
                var gotTempting = int.TryParse(_mProductsDataString.values[i][6], out var productTempting);
                if (productTempting == 0 || !gotTempting)
                {
                    Debug.LogWarning(
                        $"[JobSupplierObject.LoadProductsFromJson] Product named {productName} for {_supplierObject.StoreName} must have " +
                        $"Tempting greater than zero");
                    return;
                }
                
                var gotPunish = int.TryParse(_mProductsDataString.values[i][7], out var productPunish);
                if (productPunish == 0 || !gotPunish)
                {
                    Debug.LogWarning(
                        $"[JobSupplierObject.LoadProductsFromJson] Product named {productName} for {_supplierObject.StoreName} must have Punish " +
                        $"greater than zero");
                    return;
                }
                
                var prefabName = _mProductsDataString.values[i][8];
                var productBrand = _mProductsDataString.values[i][9];
                var prefabSpriteName = _mProductsDataString.values[i][10];
                var productDescription = _mProductsDataString.values[i][11];

                var productObject = new StoreProductObjectData(productBitId, productName, productType, productQuantity,
                    productPrice, productHideValue, productTempting, productPunish, prefabName, productBrand, 
                    prefabSpriteName, productDescription);
                _mProductsInStore.Add(productObject.ProductId, productObject);
            }
            Debug.Log($"[LoadProductsFromJson] Finish Serializing Job supplier's {_supplierObject.StoreName} Product Json data");
        }
    }
}