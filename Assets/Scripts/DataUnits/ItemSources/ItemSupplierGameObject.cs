using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataUnits.GameCatalogues.JsonCatalogueLoaders;
using DialogueSystem;
using DialogueSystem.Interfaces;
using DialogueSystem.Phone;
using DialogueSystem.Units;
using GameDirection;
using GamePlayManagement;
using GamePlayManagement.BitDescriptions.Suppliers;
using GamePlayManagement.ProfileDataModules.ItemSuppliers.Stores;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Utils;
using DialogueType = DataUnits.JobSources.DialogueType;
using Random = UnityEngine.Random;

namespace DataUnits.ItemSources
{
    [CreateAssetMenu(menuName = "ItemSource/BaseItemSource")]
    public class ItemSupplierGameObject : ScriptableObject, IItemSupplierDataObject
    {
        private int _mReliance;
        private int _mQuality;
        private int _mKindness;
        private int _mRefillStockPeriod;
        private string _mSpriteName;
        private IItemSupplierShop _supplierShop;
        private int _mFondness = 0;

        public IItemSupplierShop SupplierShop => _supplierShop;
        
        [SerializeField] protected Sprite supplierPortrait;
        public Sprite SupplierPortrait =>supplierPortrait;
        public int StorePhoneNumber { get; set; }
        public string StoreName { get; set; }
        public string StoreOwnerName { get; set; }

        public string StoreDescription { get; set; }
        public BitItemSupplier ItemSupplierId { get; set; }
        public int ItemTypesAvailable { get; set; }
        public int StoreUnlockPoints { get; set; }
        public string SpeakerName => StoreName;
        public DialogueSpeakerId SpeakerIndex { get; set; }


        public int StoreHighestUnlockedDialogue { get; }

        public int StoreHighestLockedDialogue { get; }
        
        private Dictionary<int, IDialogueObject> _mImportantDialoguesDict = new Dictionary<int, IDialogueObject>();
        private Dictionary<int, IDialogueObject> _mDeflectionDialoguesDict = new Dictionary<int, IDialogueObject>();
        private Dictionary<int, IDialogueObject> _mInsistenceDialoguesDict = new Dictionary<int, IDialogueObject>();

        public void SetStats(int reliance, int quality, int kindness, int stockRefillPeriod, string spriteName)
        {
            _mReliance = reliance;
            _mQuality = quality;
            _mKindness = kindness;
            _mRefillStockPeriod = stockRefillPeriod;
            _mSpriteName = spriteName;
        }

        public int Reliance => _mReliance;
        public int Quality => _mQuality;
        public int Kindness => _mKindness;
        public int RefillStockPeriod => _mRefillStockPeriod;
        public string SpriteName
        {
            get => _mSpriteName;
        }

        public async Task StartUnlockedData()
        {
            await GetImportantAndInsistenceDialogues();
        }
        public void PlayerLostResetData()
        {
            
        }

        public void InitializeStore(IItemSupplierShop shop)
        {
            _supplierShop = shop;
        }

        private async Task GetImportantAndInsistenceDialogues()
        {
            var importantDialoguesUrl = DataSheetUrls.SuppliersDialogueGameData(SpeakerIndex, DialogueType.ImportantDialogue);
            var insistenceDialoguesUrl = DataSheetUrls.SuppliersDialogueGameData(SpeakerIndex, DialogueType.InsistenceDialogue);
            await Task.Delay(300);
            GameDirector.Instance.ActCoroutine(LoadDialogueDataFromServer(DialogueType.ImportantDialogue, importantDialoguesUrl));
            await Task.Delay(500);
            GameDirector.Instance.ActCoroutine(LoadDialogueDataFromServer(DialogueType.InsistenceDialogue, insistenceDialoguesUrl));
        }
        
        public void ReceivePlayerCall(IPlayerGameProfile playerData)
        {
            if (!BitOperator.IsActive(playerData.GetActiveItemSuppliersModule().UnlockedItemSuppliers, (int)ItemSupplierId))
            {
                RandomDeflection();
            }
            else
            {
                GetCurrentUnlockedCall();
            }
        }

        /// <summary>
        /// TODO: Implement Item Suppliers Dialogue Choose Logic
        /// </summary>
        private async void GetCurrentUnlockedCall()
        {
            Debug.LogWarning("[GetCurrentCallAnswer] UNLOCKED STORE CALL");
            Random.InitState(DateTime.Now.Millisecond);
            var randomWaitTime = Random.Range(1000, 12500);
            await Task.Delay(randomWaitTime);

            var randomDialogue = _mImportantDialoguesDict[1];
            PhoneCallOperator.Instance.PlayAnswerSound();
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
            var randomWaitTime = Random.Range(500, 12500);
            await Task.Delay(randomWaitTime);

            var randomDeflectionIndex = Random.Range(1, _mDeflectionDialoguesDict.Count - 1);
            var randomDialogue = _mDeflectionDialoguesDict[randomDeflectionIndex];
            PhoneCallOperator.Instance.PlayAnswerSound();
            GameDirector.Instance.GetDialogueOperator.StartNewDialogue(randomDialogue);
        }

        #region JsonDialogueManagement
        private SupplierDialoguesData _mDeflectionDialogueData; 
        private SupplierDialoguesData _mImportantDialogueData; 
        private SupplierDialoguesData _mInsistenceDialogueData; 
        public void LoadDeflectionsDialogueData()
        {
            if (SpeakerIndex == 0)
            {
                Debug.LogWarning("Speaker Index must be set before loading data");
                return;
            }
            Debug.Log($"START: COLLECTING SPEAKER DATA FOR {StoreName}");
            var deflectionUrl = DataSheetUrls.SuppliersDialogueGameData(SpeakerIndex, DialogueType.Deflections);
            GameDirector.Instance.ActCoroutine(LoadDialogueDataFromServer(DialogueType.Deflections, deflectionUrl));
        }
        private IEnumerator LoadDialogueDataFromServer(DialogueType dialogueType, string url)
        {
            //
            var webRequest = UnityWebRequest.Get(url);
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Item Suppliers Dialogue Data must be reachable. StoreName: {StoreName} Error: {webRequest.result}. {webRequest.error}");
            }
            else
            {
                var sourceJson = webRequest.downloadHandler.text;
                switch (dialogueType)
                {
                    case DialogueType.ImportantDialogue:
                        LoadImportantDialoguesFromJson(sourceJson);
                        break;
                    case DialogueType.Deflections:
                        LoadDeflectionDialoguesFromJson(sourceJson);
                        break;
                    case DialogueType.InsistenceDialogue:
                        LoadInsistenceDialoguesFromJson(sourceJson);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(dialogueType), dialogueType, null);
                }
            }
        }
        private void LoadDeflectionDialoguesFromJson(string sourceJson)
        {
            _mDeflectionDialogueData = JsonConvert.DeserializeObject<SupplierDialoguesData>(sourceJson);
            _mDeflectionDialoguesDict = new Dictionary<int, IDialogueObject>();

            var lastDialogueIndex = 0;
            IDialogueObject currentDialogueObject;
            for (var i = 1; i < _mDeflectionDialogueData.values.Count;i++)
            {
                var isDialogueNodeIndex = int.TryParse(_mDeflectionDialogueData.values[i][0], out var currentDialogueObjectIndex);
                if (currentDialogueObjectIndex == 0 || !isDialogueNodeIndex)
                {
                    Debug.LogWarning($"[ItemSupplier.LoadDeflectionDialoguesFromJson] Dialogues for Intro must have node Index greater than zero");
                    return;
                }
                if (lastDialogueIndex != currentDialogueObjectIndex || i == 1)
                {
                    lastDialogueIndex = currentDialogueObjectIndex;
                    currentDialogueObject = ScriptableObject.CreateInstance<DialogueObject>();
                    _mDeflectionDialoguesDict.Add(currentDialogueObjectIndex, currentDialogueObject);
                }
                
                var hasDialogueNodeId = int.TryParse(_mDeflectionDialogueData.values[i][1], out var dialogueLineId);
                if (!hasDialogueNodeId)
                {
                    Debug.LogWarning($"[ItemSupplier.LoadDeflectionDialoguesFromJson] Dialogues for Intro must have a dialogue node id");
                    return;
                }
                var isSpeakerId = int.TryParse(_mDeflectionDialogueData.values[i][2], out var speakerId);

                var dialogueLineText = _mDeflectionDialogueData.values[i][3];
                var cameraArgs = _mDeflectionDialogueData.values[i][4].Split(',');
                var hasCameraTarget = cameraArgs.Length >1;
                var eventNameId = _mDeflectionDialogueData.values[i][5];
                var hasEventId = eventNameId != "0";
                
                var linksToString = _mDeflectionDialogueData.values[i][6].Split(',');
                var linksToInts = DialogueProcessor.ProcessLinksStrings(linksToString);
                var hasChoices = linksToInts.Length > 1;
                
                //Highlight event
                var hasHighlightEvent = _mDeflectionDialogueData.values[i][7] != "0";
                var emptyString = new string[1] {"0"};
                var highlightEvent = hasHighlightEvent ? _mDeflectionDialogueData.values[i][7].Split(',') : emptyString;

                var dialogueNode = new DialogueNodeData(currentDialogueObjectIndex, dialogueLineId, speakerId, dialogueLineText,
                    hasCameraTarget, cameraArgs, hasChoices, hasEventId, eventNameId, linksToInts, hasHighlightEvent, highlightEvent);
                _mDeflectionDialoguesDict[currentDialogueObjectIndex].DialogueNodes.Add(dialogueNode);
            }
            Debug.Log($"[ItemSupplier.LoadDeflectionDialoguesFromJson] Finish Deflection dialogues parse for {StoreName}");
        }
        private void LoadImportantDialoguesFromJson(string sourceJson)
        {
            Debug.Log($"[ItemSupplier.LoadImportantDialoguesFromJson] Begin request");
            _mImportantDialogueData = JsonConvert.DeserializeObject<SupplierDialoguesData>(sourceJson);
            Debug.Log($"Finished parsing. Is Job Supplier Dialogue null?: {_mImportantDialogueData == null}. {_mImportantDialogueData}");
            _mImportantDialoguesDict = new Dictionary<int, IDialogueObject>();

            var lastDialogueIndex = 0;
            IDialogueObject currentDialogueObject;
            for (var i = 1; i < _mImportantDialogueData.values.Count;i++)
            {
                var isDialogueNodeIndex = int.TryParse(_mImportantDialogueData.values[i][0], out var currentDialogueObjectIndex);
                if (currentDialogueObjectIndex == 0 || !isDialogueNodeIndex)
                {
                    Debug.LogWarning($"[ItemSupplier.LoadImportantDialoguesFromJson] Dialogues for Intro must have node Index greater than zero");
                    return;
                }
                if (lastDialogueIndex != currentDialogueObjectIndex || i == 1)
                {
                    lastDialogueIndex = currentDialogueObjectIndex;
                    currentDialogueObject = ScriptableObject.CreateInstance<DialogueObject>();
                    _mImportantDialoguesDict.Add(currentDialogueObjectIndex, currentDialogueObject);
                }
                
                var hasDialogueNodeId = int.TryParse(_mImportantDialogueData.values[i][1], out var dialogueLineId);
                if (!hasDialogueNodeId)
                {
                    Debug.LogWarning($"[ItemSupplier.LoadImportantDialoguesFromJson] Dialogues for Intro must have a dialogue node id");
                    return;
                }
                var isSpeakerId = int.TryParse(_mImportantDialogueData.values[i][2], out var speakerId);
                if (speakerId == 0 || !isSpeakerId)
                {
                    Debug.LogWarning($"[ItemSupplier.LoadImportantDialoguesFromJson] Dialogues for Intro must have a speaker index greater than zero");
                }

                var dialogueLineText = _mImportantDialogueData.values[i][3];
                var cameraArgs = _mImportantDialogueData.values[i][4].Split(',');
                var hasCameraTarget = cameraArgs.Length >1;
                var eventNameId = _mImportantDialogueData.values[i][5];
                var hasEventId = eventNameId != "0";
                
                var linksToString = _mImportantDialogueData.values[i][6].Split(',');
                var linksToInts = DialogueProcessor.ProcessLinksStrings(linksToString);
                var linksToFinish = linksToInts[0] == 0;
                var hasChoices = linksToInts.Length > 1;

                //Highlight event
                var hasHighlightEvent = _mImportantDialogueData.values[i][7] != "0";
                var emptyString = new string[1] {"0"};
                var highlightEvent = hasHighlightEvent ? _mImportantDialogueData.values[i][7].Split(',') : emptyString;

                var dialogueNode = new DialogueNodeData(currentDialogueObjectIndex, dialogueLineId, speakerId, dialogueLineText,
                    hasCameraTarget, cameraArgs, hasChoices, hasEventId, eventNameId, linksToInts, hasHighlightEvent, highlightEvent);

                _mImportantDialoguesDict[currentDialogueObjectIndex].DialogueNodes.Add(dialogueNode);
            }
            Debug.Log($"[ItemSupplier.LoadImportantDialoguesFromJson] Finish Important dialogues parse for {StoreName}");
        }
        private void LoadInsistenceDialoguesFromJson(string sourceJson)
        {
            Debug.Log($"[ItemSupplier.LoadInsistenceDialoguesFromJson] Begin request");
            _mInsistenceDialogueData = JsonConvert.DeserializeObject<SupplierDialoguesData>(sourceJson);
            Debug.Log($"Finished parsing. Is Job Supplier Dialogue null?: {_mInsistenceDialogueData == null}. {_mInsistenceDialogueData}");
            _mInsistenceDialoguesDict = new Dictionary<int, IDialogueObject>();

            var lastDialogueIndex = 0;
            IDialogueObject currentDialogueObject;
            for (var i = 1; i < _mInsistenceDialogueData.values.Count;i++)
            {
                var isDialogueNodeIndex = int.TryParse(_mInsistenceDialogueData.values[i][0], out var currentDialogueObjectIndex);
                if (currentDialogueObjectIndex == 0 || !isDialogueNodeIndex)
                {
                    Debug.LogWarning($"[ItemSupplier.LoadInsistenceDialoguesFromJson] Dialogues for Intro must have node Index greater than zero");
                    return;
                }
                if (lastDialogueIndex != currentDialogueObjectIndex || i == 1)
                {
                    lastDialogueIndex = currentDialogueObjectIndex;
                    currentDialogueObject = ScriptableObject.CreateInstance<DialogueObject>();
                    _mInsistenceDialoguesDict.Add(currentDialogueObjectIndex, currentDialogueObject);
                }
                
                var hasDialogueNodeId = int.TryParse(_mInsistenceDialogueData.values[i][1], out var dialogueLineId);
                if (!hasDialogueNodeId)
                {
                    Debug.LogWarning($"[ItemSupplier.LoadInsistenceDialoguesFromJson] Dialogues for Intro must have a dialogue node id");
                    return;
                }
                var isSpeakerId = int.TryParse(_mInsistenceDialogueData.values[i][2], out var speakerId);
                if (speakerId == 0 || !isSpeakerId)
                {
                    Debug.LogWarning($"[ItemSupplier.LoadInsistenceDialoguesFromJson] Dialogues for Intro must have a speaker index greater than zero");
                }

                var dialogueLineText = _mInsistenceDialogueData.values[i][3];
                var cameraArgs = _mInsistenceDialogueData.values[i][4].Split(',');
                var hasCameraTarget = cameraArgs.Length >1;
                var eventNameId = _mInsistenceDialogueData.values[i][5];
                var hasEventId = eventNameId != "0";
                
                var linksToString = _mInsistenceDialogueData.values[i][6].Split(',');
                var linksToInts = DialogueProcessor.ProcessLinksStrings(linksToString);
                var linksToFinish = linksToInts[0] == 0;
                var hasChoices = linksToInts.Length > 1;

                //Highlight event
                var hasHighlightEvent = _mInsistenceDialogueData.values[i][7] != "0";
                var emptyString = new string[1] {"0"};
                var highlightEvent = hasHighlightEvent ? _mInsistenceDialogueData.values[i][7].Split(',') : emptyString;

                var dialogueNode = new DialogueNodeData(currentDialogueObjectIndex, dialogueLineId, speakerId, dialogueLineText,
                    hasCameraTarget, cameraArgs, hasChoices, hasEventId, eventNameId, linksToInts, hasHighlightEvent, highlightEvent);


                _mInsistenceDialoguesDict[currentDialogueObjectIndex].DialogueNodes.Add(dialogueNode);
            }
            Debug.Log($"[ItemSupplier.LoadInsistenceDialoguesFromJson] Finish Insistence dialogues parse for {StoreName}");
        }

        #endregion

        public void ReceiveFondness(int amount)
        {
            _mFondness += amount;
        }
    }
}