using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataUnits.GameCatalogues.JsonCatalogueLoaders;
using DataUnits.ItemSources;
using DialogueSystem;
using DialogueSystem.Units;
using GamePlayManagement.BitDescriptions.Suppliers;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Utils;

namespace DataUnits.GameCatalogues
{
    public class BaseItemSuppliersCatalogue : MonoBehaviour, IBaseItemSuppliersCatalogue
    {
        private static BaseItemSuppliersCatalogue _instance;
        public static IBaseItemSuppliersCatalogue Instance => _instance;
        private ItemSuppliersFromData _mItemSuppliersFromData;
    
        private List<IItemSupplierDataObject> _mIItemSuppliersInData;
        public List<IItemSupplierDataObject> GetItemSuppliersInData => _mIItemSuppliersInData;
    
        private void Awake()
        {
            DontDestroyOnLoad(this);
            if (_instance != null && _instance != this)
            {
                Destroy(this);
            }
            _instance = this;
            GetItemsCatalogueData();
        }

        private void Start()
        {
        }
        private void GetItemsCatalogueData()
        {
            Debug.Log($"START: COLLECTING Item suppliers DATA");
            var url = DataSheetUrls.ItemSuppliersGameData;
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
                LoadItemSuppliersFromJson(sourceJson);
            }
        }
        private void LoadItemSuppliersFromJson(string sourceJson)
        {
            Debug.Log($"BaseItemSuppliersCatalogue.LoadItemSuppliersFromJson");
            Debug.Log($"StartParsing Item Suppliers Catalogue: {sourceJson}");

            _mItemSuppliersFromData = JsonConvert.DeserializeObject<ItemSuppliersFromData>(sourceJson);
            Debug.Log($"Finished parsing. Is _mItemSuppliersFromData null?: {_mItemSuppliersFromData == null}");
            _mIItemSuppliersInData = new List<IItemSupplierDataObject>();
            int storePhone;
            int unlockPoints;
            for (var i = 1; i < _mItemSuppliersFromData.values.Count;i++)
            {
                var itemSupplierDataObj = (IItemSupplierDataObject)ScriptableObject.CreateInstance<ItemSupplierGameObject>();

                var gotId = int.TryParse(_mItemSuppliersFromData.values[i][0], out var supplierId);
                itemSupplierDataObj.ItemSupplierId = (BitItemSupplier) supplierId;
                    
                itemSupplierDataObj.StoreName = _mItemSuppliersFromData.values[i][1];
                
                var gotPhone = int.TryParse(_mItemSuppliersFromData.values[i][2], out storePhone);
                itemSupplierDataObj.StorePhoneNumber = storePhone;

                var gotItems = int.TryParse(_mItemSuppliersFromData.values[i][3], out var itemsAvailable);
                itemSupplierDataObj.ItemTypesAvailable = itemsAvailable;

                var gotUp = int.TryParse(_mItemSuppliersFromData.values[i][4], out unlockPoints);
                if (!gotUp)
                {
                    Debug.LogWarning("GetJobsCatalogueData");
                }
                itemSupplierDataObj.StoreUnlockPoints = unlockPoints;

                var storeDescription = _mItemSuppliersFromData.values[i][5];
                itemSupplierDataObj.StoreDescription = storeDescription;

                var gotSpeakerId = int.TryParse(_mItemSuppliersFromData.values[i][6], out var speakerId);
                if (!gotSpeakerId)
                {
                    Debug.LogWarning("[LoadItemSuppliersFromJson] Speaker Id Must be available");
                }
                itemSupplierDataObj.SpeakerIndex = (DialogueSpeakerId)speakerId;
                
                var gotReliance = int.TryParse(_mItemSuppliersFromData.values[i][7], out var reliance);
                if (!gotReliance)
                {
                    Debug.LogWarning("[LoadItemSuppliersFromJson] Reliance must be available");
                }
                
                var gotQuality = int.TryParse(_mItemSuppliersFromData.values[i][8], out var quality);
                if (!gotQuality)
                {
                    Debug.LogWarning("[LoadItemSuppliersFromJson] Quality must be available");
                }
                
                var gotKindness = int.TryParse(_mItemSuppliersFromData.values[i][9], out var kindness);
                if (!gotKindness)
                {
                    Debug.LogWarning("[LoadItemSuppliersFromJson] Reliance must be available");
                }
                
                var gotStockRefill = int.TryParse(_mItemSuppliersFromData.values[i][10], out var stockRefillPeriod);
                if (!gotStockRefill)
                {
                    Debug.LogWarning("[LoadItemSuppliersFromJson] StockRefillPeriod must be available");
                }
                var spriteName = _mItemSuppliersFromData.values[i][11];
                itemSupplierDataObj.SetStats(reliance,quality,kindness,stockRefillPeriod, spriteName);
                _mIItemSuppliersInData.Add(itemSupplierDataObj);
            }
        }
        
        public bool SupplierExists(BitItemSupplier itemSupplier)
        {
            return _mIItemSuppliersInData.Any(x => x.ItemSupplierId == itemSupplier);
        }

        public IItemSupplierDataObject GetSupplierFromId(BitItemSupplier itemId)
        {
            return _mIItemSuppliersInData.SingleOrDefault(x => x.ItemSupplierId == itemId);
        }

        public Tuple<bool,int> ItemSupplierPhoneExists(int dialedPhone)
        {
            var tuple = new Tuple<bool, int>(false, 0);
            if (dialedPhone == 0 || dialedPhone.ToString().Length != 7)
            {
                Debug.LogWarning("[BaseItemSuppliersCatalogue.SupplierPhoneExists] dialed Phone number must be checked before this instace");
                return tuple;
            }
            var isStoreNumber = _mIItemSuppliersInData.Any(x => x.StorePhoneNumber == dialedPhone);
            if (!isStoreNumber)
            {
                return tuple;
            }
            var itemSupplierId =
                (int)_mIItemSuppliersInData.SingleOrDefault(x => x.StorePhoneNumber == dialedPhone)!.ItemSupplierId;
            return new Tuple<bool, int>(isStoreNumber, itemSupplierId);
        }

        public IItemSupplierDataObject GetItemSupplierDataFromPhone(int supplierPhone)
        {
            if (supplierPhone == 0 || supplierPhone.ToString().Length != 7)
            {
                return null;
            }
            return _mIItemSuppliersInData.SingleOrDefault(x => x.StorePhoneNumber == supplierPhone);
        }

        public IItemSupplierDataObject GetItemSupplierData(BitItemSupplier jobSupplier)
        {
            return _mIItemSuppliersInData.SingleOrDefault(x => x.ItemSupplierId == jobSupplier);
        }
    }

    public interface IBaseItemSuppliersCatalogue
    {
        public bool SupplierExists(BitItemSupplier itemSupplier);
        public IItemSupplierDataObject GetSupplierFromId(BitItemSupplier itemId);
        public Tuple<bool, int> ItemSupplierPhoneExists(int dialedPhone);
        public IItemSupplierDataObject GetItemSupplierDataFromPhone(int supplierPhone);
        public IItemSupplierDataObject GetItemSupplierData(BitItemSupplier jobSupplier);
        public List<IItemSupplierDataObject> GetItemSuppliersInData { get; }
    }
    

}