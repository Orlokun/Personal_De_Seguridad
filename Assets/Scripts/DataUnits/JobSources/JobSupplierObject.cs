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
        

        private int _callAttempts = 0;

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

        #region CallManagement
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