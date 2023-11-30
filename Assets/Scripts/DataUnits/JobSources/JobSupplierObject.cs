using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataUnits.GameCatalogues.JsonCatalogueLoaders;
using DialogueSystem;
using DialogueSystem.Interfaces;
using DialogueSystem.Units;
using GameDirection;
using GamePlayManagement.BitDescriptions.Suppliers;
using GamePlayManagement.LevelManagement.LevelObjectsManagement;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Utils;
using Random = UnityEngine.Random;

namespace DataUnits.JobSources
{
    [Serializable]
    [CreateAssetMenu(menuName = "Jobs/JobSource")]
    public class JobSupplierObject : ScriptableObject, IJobSupplierObject
    {
        #region Constructor & API
        public JobSupplierObject(BitGameJobSuppliers bitId, string storeType, string storeName, string storeOwnerName,
            int storeUnlockPoints, string storeDescription, int storePhoneNumber, DialogueSpeakerId speakerIndexId)
        {
            
        }
        public BitGameJobSuppliers BitId { get; set; }
        public string StoreType{ get; set; }
        public string StoreName{ get; set; }
        public string StoreOwnerName{ get; set; }
        public int StoreUnlockPoints{ get; set; }
        public string StoreDescription{ get; set; }
        public int[] StoreMinMaxClients { get; set; }
        public int StorePhoneNumber{ get; set; }
        public int StoreOwnerAge{ get; set; }
        public DialogueSpeakerId SpeakerIndex { get; set; }
        public string SpriteName { get; set; }

        public int Sanity => _mSanity;
        public int Kindness => _mKindness;
        public int Violence => _mViolence;
        public int Intelligence => _mIntelligence;
        public int Money => _mMoney;
        #endregion

        #region SupplierStats
        private int _mSanity;
        private int _mKindness;
        private int _mViolence;
        private int _mIntelligence;
        private int _mMoney;

        ///Store Stats
        public void SetStats(int sanity, int kindness, int violence, int intelligence, int money)
        {
            _mSanity = sanity;
            _mKindness = kindness;
            _mViolence = violence;
            _mIntelligence = intelligence;
            _mMoney = money;
        }
        #endregion

        #region Members
        public int StoreHighestUnlockedDialogue => _mStoreHighestUnlockedDialogue;
        private int _mStoreHighestUnlockedDialogue;

        public int StoreHighestLockedDialogue => _mStoreHighestLockedDialogue;
        private int _mStoreHighestLockedDialogue;
        

        private Dictionary<int, IDialogueObject> _mUnderThresholdDialogues = new Dictionary<int, IDialogueObject>();
        private Dictionary<int, IDialogueObject> _mOverThresholdDialogues = new Dictionary<int, IDialogueObject>();
        #endregion

        #region JsonDialogueManagement
        private SupplierDialoguesData _mDialogueData; 
        public void LoadDialogueData()
        {
            if (SpeakerIndex == 0)
            {
                Debug.LogWarning("Speaker Index must be set before loading data");
                return;
            }
            Debug.Log($"START: COLLECTING SPEAKER DATA FOR {StoreName}");
            var url = DataSheetUrls.SuppliersDialogueGameData(SpeakerIndex);
            
            DialogueOperator.Instance.LoadSupplierDialogues(LoadDialogueDataFromServer(url));
        }
        private IEnumerator LoadDialogueDataFromServer(string url)
        {
            //
            var webRequest = UnityWebRequest.Get(url);
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Jobs Catalogue Data for {StoreName} must be reachable. Error: {webRequest.result}. {webRequest.error}");
            }
            else
            {
                var sourceJson = webRequest.downloadHandler.text;
                LoadDialoguesFromJson(sourceJson);
            }
        }
        private void LoadDialoguesFromJson(string sourceJson)
        {
            //Debug.Log($"[JobSupplier.LoadDialoguesFromJson] Begin request");
            _mDialogueData = JsonConvert.DeserializeObject<SupplierDialoguesData>(sourceJson);
            //Debug.Log($"Finished parsing. Is Job Supplier Dialogue null?: {_mDialogueData == null}. {_mDialogueData}");
            _mUnderThresholdDialogues = new Dictionary<int, IDialogueObject>();
            _mOverThresholdDialogues = new Dictionary<int, IDialogueObject>();

            var lastDialogueIndex = 0;
            _mStoreHighestUnlockedDialogue = 0;
            _mStoreHighestLockedDialogue = 0;
            
            IDialogueObject baseDialogueObject;
            for (var i = 1; i < _mDialogueData.values.Count; i++)
            {
                var isDialogueIndex = int.TryParse(_mDialogueData.values[i][0], out var dialogueIndex);
                if (dialogueIndex == 0 || !isDialogueIndex)
                {
                    Debug.LogWarning($"[JobSupplierObject.LoadDialoguesFromJson] Dialogues for {StoreName} must have Index greater than zero");
                    return;
                }
                if (i == 1 || lastDialogueIndex != dialogueIndex)
                {
                    baseDialogueObject = (IDialogueObject) CreateInstance<BaseDialogueObject>();
                    lastDialogueIndex = dialogueIndex;
                    if (StoreUnlockPoints > dialogueIndex)
                    {
                        _mUnderThresholdDialogues.Add(dialogueIndex, baseDialogueObject);
                        _mStoreHighestLockedDialogue = lastDialogueIndex > StoreHighestLockedDialogue
                            ? lastDialogueIndex
                            : StoreHighestLockedDialogue;
                    }
                    else
                    {
                        _mOverThresholdDialogues.Add(dialogueIndex, baseDialogueObject);
                        //Keeps track of the highest index so it can later be used in dialogue management.
                        _mStoreHighestUnlockedDialogue = lastDialogueIndex > StoreHighestUnlockedDialogue
                            ? lastDialogueIndex
                            : StoreHighestUnlockedDialogue;
                    }
                }
                var dialogueObjectsDict = StoreUnlockPoints > dialogueIndex
                    ? _mUnderThresholdDialogues
                    : _mOverThresholdDialogues;
                var dialogueLine = _mDialogueData.values[i][1];
                dialogueObjectsDict[dialogueIndex].DialogueLines.Add(dialogueLine);
            }
        }
        #endregion
        
        #region JsonProductManagement
        private StoreProductsDataString _mProductsDataString;
        private Dictionary<int, IStoreProductObjectData> _mProductsInStore;
        public void LoadProductsData()
        {
            Debug.Log($"START: Collecting Product data for {StoreName}");
            var url = DataSheetUrls.GetStoreProducts(BitId);
            
            //TODO: Take this out of Dialogue operator
            DialogueOperator.Instance.LoadSupplierDialogues(LoadProductsFromServer(url));
        }
        private IEnumerator LoadProductsFromServer(string url)
        {
            //
            var webRequest = UnityWebRequest.Get(url);
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Jobs Catalogue Data for {StoreName} must be reachable. Error: {webRequest.result}. {webRequest.error}");
            }
            else
            {
                var sourceJson = webRequest.downloadHandler.text;
                LoadProductsFromJson(sourceJson);
            }
        }
        private void LoadProductsFromJson(string sourceJson)
        {
            _mProductsInStore = new Dictionary<int, IStoreProductObjectData>();
            Debug.Log($"[LoadProductsFromJson] Start Serializing Job supplier's {StoreName} Product Json data");
            _mProductsDataString = JsonConvert.DeserializeObject<StoreProductsDataString>(sourceJson);
            for (var i = 1; i < _mProductsDataString.values.Count; i++)
            {
                var gotId = int.TryParse(_mProductsDataString.values[i][0], out var productId);
                if (productId == 0 || !gotId)
                {
                    Debug.LogWarning(
                        $"[JobSupplierObject.LoadProductsFromJson] Product for {StoreName} must have Id greater than zero");
                    return;
                }
                var productName = _mProductsDataString.values[i][1]; 
                
                var gotType = int.TryParse(_mProductsDataString.values[i][2], out var productType);
                if (productType == 0 || !gotType)
                {
                    Debug.LogWarning(
                        $"[JobSupplierObject.LoadProductsFromJson] Product named {productName} for {StoreName} must have " +
                        $"Type greater than zero");
                    return;
                }
                
                var gotQuantity = int.TryParse(_mProductsDataString.values[i][3], out var productQuantity);
                if (productQuantity == 0 || !gotQuantity)
                {
                    Debug.LogWarning(
                        $"[JobSupplierObject.LoadProductsFromJson] Product named {productName} for {StoreName} must have " +
                        $"Quantity greater than zero");
                    return;
                }
                
                var gotPrice = int.TryParse(_mProductsDataString.values[i][4], out var productPrice);
                if (productPrice == 0 || !gotPrice)
                {
                    Debug.LogWarning(
                        $"[JobSupplierObject.LoadProductsFromJson] Product named {productName} for {StoreName} must have " +
                        $"Price greater than zero");
                    return;
                }
                var gotHideValue = int.TryParse(_mProductsDataString.values[i][5], out var productHideValue);
                if (productHideValue == 0 || !gotHideValue)
                {
                    Debug.LogWarning(
                        $"[JobSupplierObject.LoadProductsFromJson] Product named {productName} for {StoreName} must have " +
                        $"HideValue greater than zero");
                    return;
                }
                var gotTempting = int.TryParse(_mProductsDataString.values[i][6], out var productTempting);
                if (productTempting == 0 || !gotTempting)
                {
                    Debug.LogWarning(
                        $"[JobSupplierObject.LoadProductsFromJson] Product named {productName} for {StoreName} must have " +
                        $"Tempting greater than zero");
                    return;
                }
                
                var gotPunish = int.TryParse(_mProductsDataString.values[i][7], out var productPunish);
                if (productPunish == 0 || !gotPunish)
                {
                    Debug.LogWarning(
                        $"[JobSupplierObject.LoadProductsFromJson] Product named {productName} for {StoreName} must have Punish " +
                        $"greater than zero");
                    return;
                }
                
                var prefabName = _mProductsDataString.values[i][8];
                var productBrand = _mProductsDataString.values[i][9];
                var prefabSpriteName = _mProductsDataString.values[i][10];
                var productDescription = _mProductsDataString.values[i][11];

                var productObject = new StoreProductObjectData(productId, productName, productType, productQuantity,
                    productPrice, productHideValue, productTempting, productPunish, prefabName, productBrand, 
                    prefabSpriteName, productDescription);
                _mProductsInStore.Add(productObject.ProductId, productObject);
            }
            Debug.Log($"[LoadProductsFromJson] Finish Serializing Job supplier's {StoreName} Product Json data");
        }
        
        #endregion
        #region CallManagement
        private int _lastCallExp = 0;
        public void StartCalling(int playerLevel)
        {
            if (playerLevel < StoreUnlockPoints)
            {
                RandomDeflection();
            }
            else
            {
                GetCurrentUnlockedCall();
            }
        }

        private void CheckLastCallsStatus()
        {
            
        }
        
        private async void GetCurrentUnlockedCall()
        {
            Debug.LogWarning("[GetCurrentCallAnswer] UNLOCKED STORE CALL");
            Random.InitState(DateTime.Now.Millisecond);
            var randomWaitTime = Random.Range(500, 4500);
            await Task.Delay(randomWaitTime);

            var randomAnswerIndex = Random.Range(StoreUnlockPoints, StoreHighestUnlockedDialogue);
            var randomDialogue = _mOverThresholdDialogues[randomAnswerIndex];
            PhoneCallOperator.Instance.AnswerPhone();
            GameDirector.Instance.GetDialogueOperator.StartNewDialogue(randomDialogue);
        }
        
        private async void RandomDeflection()
        {
            Debug.LogWarning("[RandomDeflection] STORE NOT UNLOCKED");
            if (GameDirector.Instance.GetDialogueOperator.CurrentDialogueState != UIDialogueState.NotDisplayed)
            {
                return;
            }
            Random.InitState(DateTime.Now.Millisecond);
            var randomDeflectionIndex = Random.Range(1, StoreHighestLockedDialogue);
            var randomDialogue = _mUnderThresholdDialogues[randomDeflectionIndex];
            Random.InitState(DateTime.Now.Millisecond);
            var randomWaitTime = Random.Range(500, 12500);
            await Task.Delay(randomWaitTime);

            PhoneCallOperator.Instance.AnswerPhone();
            GameDirector.Instance.GetDialogueOperator.StartNewDialogue(randomDialogue);
        }
        #endregion
    }
}