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
    public enum DialogueType
    {
        Deflections = 1,
        ImportantDialogue = 2,
        InsistenceDialogue = 3
    }
    
    [Serializable]
    [CreateAssetMenu(menuName = "Jobs/JobSource")]
    public class JobSupplierObject : ScriptableObject, IJobSupplierObject
    {
        #region Constructor & API
        public JobSupplierObject(JobSupplierBitId jobSupplierBitId, string storeType, string storeName, string storeOwnerName,
            int storeUnlockPoints, string storeDescription, int storePhoneNumber, DialogueSpeakerId speakerIndexId)
        {
            
        }
        public JobSupplierBitId JobSupplierBitId { get; set; }
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
        

        private Dictionary<int, IDialogueObject> _mRandomDeflectionDialogues = new Dictionary<int, IDialogueObject>();
        private Dictionary<int, IDialogueObject> _mImportantDialogues = new Dictionary<int, IDialogueObject>();
        private Dictionary<int, IDialogueObject> _mInsistenceDialogues = new Dictionary<int, IDialogueObject>();
        #endregion

        #region JsonDialogueManagement
        private SupplierDialoguesData _mImportantDialoguesData; 
        private SupplierDialoguesData _mRandomDeflectionData; 
        private SupplierDialoguesData _mInsistenceDialoguesData; 
        public void LoadDeflectionDialoguesData()
        {
            if (SpeakerIndex == 0)
            {
                Debug.LogWarning("Speaker Index must be set before loading data");
                return;
            }
            Debug.Log($"START: COLLECTING SPEAKER DATA FOR {StoreName}");
            var deflectionDialoguesUrl = DataSheetUrls.SuppliersDialogueGameData(SpeakerIndex, DialogueType.Deflections);
            GameDirector.Instance.ActCoroutine(DownloadDialogueData(DialogueType.Deflections, deflectionDialoguesUrl));

        }
        private IEnumerator DownloadDialogueData(DialogueType dialogueType, string url)
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
                switch (dialogueType)
                {
                    case DialogueType.ImportantDialogue:
                        Debug.Log($"[DownloadDialogueData.ImportantDialogue] Loading {StoreName} Important dialogues");
                        LoadImportantDialoguesFromJson(sourceJson);
                        break;
                    case DialogueType.Deflections:
                        Debug.Log($"[DownloadDialogueData.Deflections]  Loading {StoreName} Deflections dialogues");
                        LoadDeflectionDialoguesFromJson(sourceJson);
                        break;
                    case DialogueType.InsistenceDialogue:
                        Debug.Log($"[DownloadDialogueData.Deflections]  Loading {StoreName} InsistenceDialogue");
                        LoadInsistenceDialogues(sourceJson);
                        break;
                }
            }
        }
        private void LoadImportantDialoguesFromJson(string sourceJson)
        {
            //Debug.Log($"[JobSupplier.LoadImportantDialoguesFromJson] Begin request");
            _mImportantDialoguesData = JsonConvert.DeserializeObject<SupplierDialoguesData>(sourceJson);
            //Debug.Log($"Finished parsing. Is Job Supplier Dialogue null?: {_mDialogueData == null}. {_mDialogueData}");
            _mImportantDialogues = new Dictionary<int, IDialogueObject>();

            var lastDialogueObjectIndex = 0;
            _mStoreHighestUnlockedDialogue = 0;
            _mStoreHighestLockedDialogue = 0;
            
            IDialogueObject currentDialogueObject;
            for (var i = 1; i < _mImportantDialoguesData.values.Count; i++)
            {
                var isDialogueNodeIndex = int.TryParse(_mImportantDialoguesData.values[i][0], out var currentDialogueObjectIndex);
                if (currentDialogueObjectIndex == 0 || !isDialogueNodeIndex)
                {
                    Debug.LogWarning($"[JobSupplierObject.LoadDialoguesFromJson] Dialogues for {StoreName} must have node Index greater than zero");
                    return;
                }
                if (lastDialogueObjectIndex != currentDialogueObjectIndex || i == 1)
                {
                    lastDialogueObjectIndex = currentDialogueObjectIndex;
                    currentDialogueObject = ScriptableObject.CreateInstance<DialogueObject>();
                    _mImportantDialogues.Add(currentDialogueObjectIndex, currentDialogueObject);
                }
                
                var hasDialogueNodeId = int.TryParse(_mImportantDialoguesData.values[i][1], out var dialogueLineId);
                if (!hasDialogueNodeId)
                {
                    Debug.LogWarning($"[JobSupplierObject.LoadDialoguesFromJson] Dialogues for {StoreName} must have Index greater than zero");
                    return;
                }
                var isSpeakerId = int.TryParse(_mImportantDialoguesData.values[i][2], out var speakerId);
                if (!isSpeakerId)
                {
                    Debug.LogWarning($"[JobSupplierObject.LoadDialoguesFromJson] Dialogues for {StoreName} must have Index greater than zero");
                    return;
                }

                var dialogueLineText = _mImportantDialoguesData.values[i][3];
                var cameraTargetName = _mImportantDialoguesData.values[i][4];
                var hasCameraTarget = cameraTargetName != "0";
                var eventNameId = _mImportantDialoguesData.values[i][5];
                var hasEventId = eventNameId != "0";
                
                var linksToString = _mImportantDialoguesData.values[i][6].Split(',');
                var linksToInts = DialogueProcessor.ProcessLinksStrings(linksToString);
                var linksToFinish = linksToInts[0] == 0;
                var hasChoices = linksToInts.Length > 1;

                var dialogueNode = new DialogueNodeData(currentDialogueObjectIndex, dialogueLineId, speakerId, dialogueLineText,
                    hasCameraTarget, cameraTargetName, hasChoices, hasEventId, eventNameId, linksToInts);
                _mImportantDialogues[currentDialogueObjectIndex].DialogueNodes.Add(dialogueNode);
            }
        }
        private void LoadDeflectionDialoguesFromJson(string sourceJson)
        {
            Debug.Log($"[JobSupplier.LoadImportantDialoguesFromJson] Begin Random deflection dialogues request for {StoreName}");
            _mRandomDeflectionData = JsonConvert.DeserializeObject<SupplierDialoguesData>(sourceJson);
            _mRandomDeflectionDialogues = new Dictionary<int, IDialogueObject>();

            var lastDialogueObjectIndex = 0;
            if (_mRandomDeflectionData.values == null)
            {
                Debug.LogError($"[JobSupplier.LoadImportantDialoguesFromJson] Values were not properly loaded into {StoreName} dialogues");
            }
            IDialogueObject currentDialogueObject;
            for (var i = 1; i < _mRandomDeflectionData.values.Count; i++)
            {
                var isDialogueNodeIndex = int.TryParse(_mRandomDeflectionData.values[i][0], out var currentDialogueObjectIndex);
                if (currentDialogueObjectIndex == 0 || !isDialogueNodeIndex)
                {
                    Debug.LogWarning($"[JobSupplierObject.LoadDialoguesFromJson] Dialogues for {StoreName} must have node Index greater than zero");
                    return;
                }
                if (lastDialogueObjectIndex != currentDialogueObjectIndex || i == 1)
                {
                    lastDialogueObjectIndex = currentDialogueObjectIndex;
                    currentDialogueObject = ScriptableObject.CreateInstance<DialogueObject>();
                    _mRandomDeflectionDialogues.Add(currentDialogueObjectIndex, currentDialogueObject);
                }
                
                var hasDialogueNodeId = int.TryParse(_mRandomDeflectionData.values[i][1], out var dialogueLineId);
                if (!hasDialogueNodeId)
                {
                    Debug.LogWarning($"[JobSupplierObject.LoadDialoguesFromJson] Dialogues for {StoreName} must have Index greater than zero");
                    return;
                }
                var isSpeakerId = int.TryParse(_mRandomDeflectionData.values[i][2], out var speakerId);
                if (speakerId == 0 || !isSpeakerId)
                {
                    Debug.LogWarning($"[JobSupplierObject.LoadDialoguesFromJson] Dialogues for {StoreName} must have Index greater than zero");
                    return;
                }

                var dialogueLineText = _mRandomDeflectionData.values[i][3];
                var cameraTargetName = _mRandomDeflectionData.values[i][4];
                var hasCameraTarget = cameraTargetName != "0";
                var eventNameId = _mRandomDeflectionData.values[i][5];
                var hasEventId = eventNameId != "0";
                
                var linksToString = _mRandomDeflectionData.values[i][6].Split(',');
                var linksToInts = DialogueProcessor.ProcessLinksStrings(linksToString);
                var linksToFinish = linksToInts[0] == 0;
                var hasChoices = linksToInts.Length > 1;

                var dialogueNode = new DialogueNodeData(currentDialogueObjectIndex, dialogueLineId, speakerId, dialogueLineText,
                    hasCameraTarget, cameraTargetName, hasChoices, hasEventId, eventNameId, linksToInts);
                _mRandomDeflectionDialogues[currentDialogueObjectIndex].DialogueNodes.Add(dialogueNode);
            }
        }
        private void LoadInsistenceDialogues(string sourceJson)
        {
            //Debug.Log($"[JobSupplier.LoadImportantDialoguesFromJson] Begin request");
            _mInsistenceDialoguesData = JsonConvert.DeserializeObject<SupplierDialoguesData>(sourceJson);
            //Debug.Log($"Finished parsing. Is Job Supplier Dialogue null?: {_mInsistenceDialoguesData == null}. {_mInsistenceDialoguesData}");
            _mInsistenceDialogues = new Dictionary<int, IDialogueObject>();

            var lastDialogueObjectIndex = 0;
            
            IDialogueObject currentDialogueObject;
            for (var i = 1; i < _mInsistenceDialoguesData.values.Count; i++)
            {
                var isDialogueNodeIndex = int.TryParse(_mInsistenceDialoguesData.values[i][0], out var currentDialogueObjectIndex);
                if (currentDialogueObjectIndex == 0 || !isDialogueNodeIndex)
                {
                    Debug.LogWarning($"[JobSupplierObject.LoadDialoguesFromJson] Dialogues for {StoreName} must have node Index greater than zero");
                    return;
                }
                if (lastDialogueObjectIndex != currentDialogueObjectIndex || i == 1)
                {
                    lastDialogueObjectIndex = currentDialogueObjectIndex;
                    currentDialogueObject = ScriptableObject.CreateInstance<DialogueObject>();
                    _mInsistenceDialogues.Add(currentDialogueObjectIndex, currentDialogueObject);
                }
                
                var hasDialogueNodeId = int.TryParse(_mInsistenceDialoguesData.values[i][1], out var dialogueLineId);
                if (dialogueLineId == 0 || !hasDialogueNodeId)
                {
                    Debug.LogWarning($"[JobSupplierObject.LoadDialoguesFromJson] Dialogues for {StoreName} must have Index greater than zero");
                    return;
                }
                var isSpeakerId = int.TryParse(_mInsistenceDialoguesData.values[i][2], out var speakerId);
                if (speakerId == 0 || !isSpeakerId)
                {
                    Debug.LogWarning($"[JobSupplierObject.LoadDialoguesFromJson] Dialogues for {StoreName} must have Index greater than zero");
                    return;
                }

                var dialogueLineText = _mInsistenceDialoguesData.values[i][3];
                var cameraTargetName = _mInsistenceDialoguesData.values[i][4];
                var hasCameraTarget = cameraTargetName != "0";
                var eventNameId = _mInsistenceDialoguesData.values[i][5];
                var hasEventId = eventNameId != "0";
                
                var linksToString = _mInsistenceDialoguesData.values[i][6].Split(',');
                var linksToInts = DialogueProcessor.ProcessLinksStrings(linksToString);
                var linksToFinish = linksToInts[0] == 0;
                var hasChoices = linksToInts.Length > 1;

                var dialogueNode = new DialogueNodeData(currentDialogueObjectIndex, dialogueLineId, speakerId, dialogueLineText,
                    hasCameraTarget, cameraTargetName, hasChoices, hasEventId, eventNameId, linksToInts);
                _mInsistenceDialogues[currentDialogueObjectIndex].DialogueNodes.Add(dialogueNode);
            }
        }
        #endregion
        
        #region JsonProductManagement
        private StoreProductsDataString _mProductsDataString;
        private Dictionary<int, IStoreProductObjectData> _mProductsInStore;
        public void StartUnlockData()
        {
            GetImportantAndInsistenceDialogues();
            GetProductsData();
        }
        
        private async void GetImportantAndInsistenceDialogues()
        {
            var importantDialoguesUrl = DataSheetUrls.SuppliersDialogueGameData(SpeakerIndex, DialogueType.ImportantDialogue);
            var insistenceDialoguesUrl = DataSheetUrls.SuppliersDialogueGameData(SpeakerIndex, DialogueType.InsistenceDialogue);
            await Task.Delay(300);
            GameDirector.Instance.ActCoroutine(DownloadDialogueData(DialogueType.ImportantDialogue, importantDialoguesUrl));
            await Task.Delay(500);
            GameDirector.Instance.ActCoroutine(DownloadDialogueData(DialogueType.InsistenceDialogue, insistenceDialoguesUrl));
        }
        
        private void GetProductsData()
        {
            Debug.Log($"START: Collecting Product data for {StoreName}");
            var url = DataSheetUrls.GetStoreProducts(JobSupplierBitId);
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
            var randomDialogue = _mImportantDialogues[randomAnswerIndex];
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
            var randomDialogue = _mRandomDeflectionDialogues[randomDeflectionIndex];
            Random.InitState(DateTime.Now.Millisecond);
            var randomWaitTime = Random.Range(500, 12500);
            await Task.Delay(randomWaitTime);

            PhoneCallOperator.Instance.AnswerPhone();
            GameDirector.Instance.GetDialogueOperator.StartNewDialogue(randomDialogue);
        }
        #endregion
    }
}