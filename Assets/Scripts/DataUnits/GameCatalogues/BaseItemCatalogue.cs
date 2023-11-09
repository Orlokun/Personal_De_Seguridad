using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataUnits.GameCatalogues.JsonCatalogueLoaders;
using DataUnits.ItemScriptableObjects;
using GamePlayManagement.BitDescriptions.Suppliers;
using Newtonsoft.Json;
using UI;
using UnityEngine;
using UnityEngine.Networking;
using Utils;

namespace DataUnits.GameCatalogues
{
    public interface IBaseItemDataCatalogue
    { 
        Dictionary<BitItemSupplier, List<IItemObject>> ExistingItemsInCatalogue { get; }
        public IItemObject GetItemFromCatalogue(BitItemSupplier itemSupplier, int itemBitId);
    }
    
    public class BaseItemCatalogue : MonoBehaviour, IBaseItemDataCatalogue
    {
        private static BaseItemCatalogue _instance;
        public static IBaseItemDataCatalogue Instance => _instance;

        private ItemCatalogueFromData _mItemCatalogueDataString;
        
        private Dictionary<BitItemSupplier, List<IItemObject>> _mCatalogueItemsFromData= new Dictionary<BitItemSupplier, List<IItemObject>>();
        public Dictionary<BitItemSupplier, List<IItemObject>> ExistingItemsInCatalogue => _mCatalogueItemsFromData;
        public void Awake()
        {
            DontDestroyOnLoad(this);
            if (_instance != null)
            {
                Destroy(this);
            }
            _instance = this;
            GetItemsCatalogueData();
        }
        private void GetItemsCatalogueData()
        {
            Debug.Log($"START: COLLECTING Item SOURCES DATA");
            var url = DataSheetUrls.ItemsCatalogueGameData;
            StartCoroutine(LoadItemsCatalogueData(url));
        }
        private IEnumerator LoadItemsCatalogueData(string url)
        {
            var webRequest = UnityWebRequest.Get(url);
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("GetItems Catalogue Data must be reachable");
            }
            else
            {
                var sourceJson = webRequest.downloadHandler.text;
                LoadFromJson(sourceJson);
            }
        }
        private void LoadFromJson(string sourceJson)
        {
            Debug.Log($"BaseJobsCatalogue.LoadFromJson");
            Debug.Log($"StartParsing Items Catalogue: {sourceJson}");

            _mItemCatalogueDataString = JsonConvert.DeserializeObject<ItemCatalogueFromData>(sourceJson);
            _mCatalogueItemsFromData = new Dictionary<BitItemSupplier, List<IItemObject>>();
            
            var lastItemSupplierId = 1;
            List<IItemObject> currentSupplierList = new List<IItemObject>();
            if (_mCatalogueItemsFromData == null)
            {
                Debug.LogError($"_mItemCatalogueDataString must not be null after parsing");
                return;
            }
            for (var i = 1; i < _mItemCatalogueDataString.values.Count; i++)
            {
                //Step 1: Get Item supplier Id
                var gotId = int.TryParse(_mItemCatalogueDataString.values[i][0], out var supplierId);
                if (!gotId)
                {
                    Debug.LogError("Item must have a supplier set from data");
                    return;
                }
                var supplierBitId = (BitItemSupplier) supplierId;
                //Step 1.1: If its new supplier, add current list to Dict and clean.
                if (lastItemSupplierId != supplierId || i == 1)
                {
                    _mCatalogueItemsFromData.Add(supplierBitId, currentSupplierList);
                    currentSupplierList = new List<IItemObject>();
                    lastItemSupplierId = supplierId;
                }
                
                //Step 2: Get Item Type
                var gotItemType = int.TryParse(_mItemCatalogueDataString.values[i][1], out var itemType);
                if (!gotItemType)
                {
                    Debug.LogError("Item must have a supplier set from data");
                    return;
                }
                var bitItemType = (BitItemType) itemType;
                
                //Step 3: Get Item ID
                var gotItemId = int.TryParse(_mItemCatalogueDataString.values[i][2], out var itemId);
                if (!gotItemId)
                {
                    Debug.LogError("Item must have a BitID set from data");
                    return;
                }
                
                //Step 4: Get Item name
                var itemName = _mItemCatalogueDataString.values[i][3];
                
                //Step 5: Get Item Unlock Points
                var gotUp = int.TryParse(_mItemCatalogueDataString.values[i][4], out var unlockPoints);
                if (!gotUp)
                {
                    Debug.LogWarning("");
                }

                //Step 6: Get Item price
                var gotPrice = int.TryParse(_mItemCatalogueDataString.values[i][5], out var itemPrice);
                if (!gotPrice)
                {
                    Debug.LogWarning("");
                }
                
                //Step 7: Create Item Scriptable Object and add to List in Dictionary
                IItemObject itemDataObject = ScriptableObject.CreateInstance<ItemObject>();
                itemDataObject.SetItemObjectData(supplierBitId, bitItemType, itemId, itemName, unlockPoints, itemPrice);
                _mCatalogueItemsFromData[supplierBitId].Add(itemDataObject);
                Debug.Log($"Added Item: {itemDataObject.ItemName}");
                Debug.Log($"Current Item Supplier: {itemDataObject.ItemSupplier}");
            }
        }
     
        public IItemObject GetItemFromCatalogue(BitItemSupplier itemSupplier, int itemBitId)
        {
            return _mCatalogueItemsFromData[itemSupplier].SingleOrDefault(x => x.BitId == itemBitId);
        }
    }
}