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

        
        private Dictionary<int, IDialogueObject> _mUnderThresholdDialogues = new Dictionary<int, IDialogueObject>();
        private Dictionary<int, IDialogueObject> _mOverThresholdDialogues = new Dictionary<int, IDialogueObject>();

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
            var randomWaitTime = Random.Range(500, 12500);
            await Task.Delay(randomWaitTime);

            var randomDeflectionIndex = Random.Range(1, _mUnderThresholdDialogues.Count - 1);
            var randomDialogue = _mUnderThresholdDialogues[randomDeflectionIndex];
            PhoneCallOperator.Instance.AnswerPhone();
            GameDirector.Instance.GetDialogueOperator.StartNewDialogue(randomDialogue);
        }

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
                Debug.LogError($"Item Suppliers Dialogue Data must be reachable. StoreName: {StoreName} Error: {webRequest.result}. {webRequest.error}");
            }
            else
            {
                var sourceJson = webRequest.downloadHandler.text;
                LoadDialoguesFromJson(sourceJson);
            }
        }
        private void LoadDialoguesFromJson(string sourceJson)
        {
            Debug.Log($"[JobSupplier.LoadDialoguesFromJson] Begin request");
            _mDialogueData = JsonConvert.DeserializeObject<SupplierDialoguesData>(sourceJson);
            Debug.Log($"Finished parsing. Is Job Supplier Dialogue null?: {_mDialogueData == null}. {_mDialogueData}");
            _mUnderThresholdDialogues = new Dictionary<int, IDialogueObject>();
            _mOverThresholdDialogues = new Dictionary<int, IDialogueObject>();

            var lastDialogueIndex = 0;
            IDialogueObject baseDialogueObject;
            for (var i = 1; i < _mDialogueData.values.Count; i++)
            {
                var isDialogueIndex = int.TryParse(_mDialogueData.values[i][0], out var dialogueIndex);
                if (dialogueIndex == 0 || !isDialogueIndex)
                {
                    Debug.LogWarning("[JobSupplierObject.LoadDialoguesFromJson] Dialogue must have Index greater than zero");
                    return;
                }
                if (i == 1 || lastDialogueIndex != dialogueIndex)
                {
                    baseDialogueObject = (IDialogueObject) CreateInstance<BaseDialogueObject>();
                    lastDialogueIndex = dialogueIndex;

                    if (StoreUnlockPoints > dialogueIndex)
                    {
                        _mUnderThresholdDialogues.Add(dialogueIndex, baseDialogueObject);
                    }
                    else
                    {
                        _mOverThresholdDialogues.Add(dialogueIndex, baseDialogueObject);
                    }
                }

                var dialogueObject = StoreUnlockPoints > dialogueIndex
                    ? _mUnderThresholdDialogues
                    : _mOverThresholdDialogues;
                
                var dialogueLine = _mDialogueData.values[i][1];
                dialogueObject[dialogueIndex].DialogueLines.Add(dialogueLine);
            }
        }
        #endregion

    }
    public interface IItemSupplierDataObject : ISupplierBaseObject, ICallableSupplier
    {
        public BitItemSupplier ItemSupplierId { get; set; }
        public int StoreUnlockPoints { get; set; }
        public int ItemTypesAvailable { get; set; }
        public string StoreDescription { get; set; }
        public void LoadDialogueData();
        public void SetStats(int reliance, int quality, int kindness, int omniCredits, string spriteName);
        public int Reliance { get; }
        public int Quality { get; }
        public int Kindness { get; }
        public int OmniCredits { get; }
        public string SpriteName { get; }
    }
}