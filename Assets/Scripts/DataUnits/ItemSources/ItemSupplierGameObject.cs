using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataUnits.GameCatalogues.JsonCatalogueLoaders;
using DataUnits.JobSources;
using DialogueSystem;
using DialogueSystem.Interfaces;
using DialogueSystem.Units;
using GameDirection;
using GamePlayManagement.BitDescriptions.Suppliers;
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
        private int _mOmniCredits;
        private string _mSpriteName;
        
        [SerializeField] protected Sprite supplierPortrait;
        public Sprite SupplierPortrait =>supplierPortrait;
        public int StorePhoneNumber { get; set; }
        public string StoreName { get; set; }
        public string StoreDescription { get; set; }
        public BitItemSupplier ItemSupplierId { get; set; }
        public int ItemTypesAvailable { get; set; }
        public int StoreUnlockPoints { get; set; }
        public DialogueSpeakerId SpeakerIndex { get; set; }
        public int StoreHighestUnlockedDialogue { get; set; }
        public int StoreHighestLockedDialogue { get; set; }

        
        private Dictionary<int, IDialogueObject> _mImportantDialoguesDict = new Dictionary<int, IDialogueObject>();
        private Dictionary<int, IDialogueObject> _mDeflectionDialoguesDict = new Dictionary<int, IDialogueObject>();

        public void SetStats(int reliance, int quality, int kindness, int omniCredits, string spriteName)
        {
            _mReliance = reliance;
            _mQuality = quality;
            _mKindness = kindness;
            _mOmniCredits = omniCredits;
            _mSpriteName = spriteName;
            StoreHighestUnlockedDialogue = 1;
        }

        public int Reliance => _mReliance;
        public int Quality => _mQuality;
        public int Kindness => _mKindness;
        public int OmniCredits => _mOmniCredits;
        public string SpriteName => _mSpriteName;


        public void StartCalling(int playerLevel)
        {
            if (playerLevel < StoreUnlockPoints)
            {
                RandomDeflection();
            }
            else
            {
                GetCurrentCallAnswer();
            }
        }

        private async void GetCurrentCallAnswer()
        {
            Debug.LogWarning("[GetCurrentCallAnswer] UNLOCKED STORE CALL");
            Random.InitState(DateTime.Now.Millisecond);
            var randomWaitTime = Random.Range(500, 12500);
            await Task.Delay(randomWaitTime);

            var randomAnswerIndex = Random.Range(StoreUnlockPoints, StoreHighestUnlockedDialogue);
            var randomDialogue = _mImportantDialoguesDict[randomAnswerIndex];
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
            var randomWaitTime = Random.Range(500, 12500);
            await Task.Delay(randomWaitTime);

            var randomDeflectionIndex = Random.Range(1, _mDeflectionDialoguesDict.Count - 1);
            var randomDialogue = _mDeflectionDialoguesDict[randomDeflectionIndex];
            PhoneCallOperator.Instance.AnswerPhone();
            GameDirector.Instance.GetDialogueOperator.StartNewDialogue(randomDialogue);
        }

        #region JsonDialogueManagement
        private SupplierDialoguesData _mDialogueData; 
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
                        //LoadDeflectionDialoguesFromJson(sourceJson);
                        break;
                    case DialogueType.Deflections:
                        LoadDeflectionDialoguesFromJson(sourceJson);
                        break;
                    case DialogueType.InsistenceDialogue:
                        //LoadDeflectionDialoguesFromJson(sourceJson);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(dialogueType), dialogueType, null);
                }
            }
        }
        private void LoadDeflectionDialoguesFromJson(string sourceJson)
        {
            Debug.Log($"[ItemSupplier.LoadDeflectionDialoguesFromJson] Begin request");
            _mDialogueData = JsonConvert.DeserializeObject<SupplierDialoguesData>(sourceJson);
            Debug.Log($"Finished parsing. Is Job Supplier Dialogue null?: {_mDialogueData == null}. {_mDialogueData}");
            _mDeflectionDialoguesDict = new Dictionary<int, IDialogueObject>();

            var lastDialogueIndex = 0;
            IDialogueObject currentDialogueObject;
            for (var i = 1; i < _mDialogueData.values.Count;i++)
            {
                var isDialogueNodeIndex = int.TryParse(_mDialogueData.values[i][0], out var currentDialogueObjectIndex);
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
                
                var hasDialogueNodeId = int.TryParse(_mDialogueData.values[i][1], out var dialogueLineId);
                if (!hasDialogueNodeId)
                {
                    Debug.LogWarning($"[ItemSupplier.LoadDeflectionDialoguesFromJson] Dialogues for Intro must have a dialogue node id");
                    return;
                }
                var isSpeakerId = int.TryParse(_mDialogueData.values[i][2], out var speakerId);
                if (speakerId == 0 || !isSpeakerId)
                {
                    Debug.LogWarning($"[ItemSupplier.LoadDeflectionDialoguesFromJson] Dialogues for Intro must have a speaker index greater than zero");
                    return;
                }

                var dialogueLineText = _mDialogueData.values[i][3];
                var cameraTargetName = _mDialogueData.values[i][4];
                var hasCameraTarget = cameraTargetName != "0";
                var eventNameId = _mDialogueData.values[i][5];
                var hasEventId = eventNameId != "0";
                
                var linksToString = _mDialogueData.values[i][6].Split(',');
                var linksToInts = DialogueProcessor.ProcessLinksStrings(linksToString);
                var linksToFinish = linksToInts[0] == 0;
                var hasChoices = linksToInts.Length > 1;

                var dialogueNode = new DialogueNodeData(currentDialogueObjectIndex, dialogueLineId, speakerId, dialogueLineText,
                    hasCameraTarget, cameraTargetName, hasChoices, hasEventId, eventNameId, linksToInts);
                _mDeflectionDialoguesDict[currentDialogueObjectIndex].DialogueNodes.Add(dialogueNode);
                Debug.Log($"[ItemSupplier.LoadDeflectionDialoguesFromJson] Finish dialogue object parse");
            }
            Debug.Log($"[ItemSupplier.LoadDeflectionDialoguesFromJson] Finish Deflection dialogues parse for {StoreName}");
        }
        #endregion
    }

    public interface IItemSupplierDataObject : ISupplierBaseObject, ICallableSupplier
    {
        public BitItemSupplier ItemSupplierId { get; set; }
        public int StoreUnlockPoints { get; set; }
        public int ItemTypesAvailable { get; set; }
        public string StoreDescription { get; set; }
        public void LoadDeflectionsDialogueData();
        public void SetStats(int reliance, int quality, int kindness, int omniCredits, string spriteName);
        public int Reliance { get; }
        public int Quality { get; }
        public int Kindness { get; }
        public int OmniCredits { get; }
        public string SpriteName { get; }
    }
}