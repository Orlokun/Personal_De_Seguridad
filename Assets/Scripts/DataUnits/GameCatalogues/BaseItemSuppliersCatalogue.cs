using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataUnits.GameCatalogues.JsonCatalogueLoaders;
using DataUnits.ItemSources;
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
            if (_instance != null)
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
            Debug.Log($"StartParsing Items Catalogue: {sourceJson}");

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
                    Debug.LogWarning("GetJobsCatalogueData");
                }
                itemSupplierDataObj.SpeakerIndex = (DialogueSpeakerId)speakerId;
                //itemSupplierDataObj.LoadDialogueData();
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