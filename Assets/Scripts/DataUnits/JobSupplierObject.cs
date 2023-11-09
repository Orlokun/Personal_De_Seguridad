using System.Collections;
using System.Collections.Generic;
using DataUnits.GameCatalogues.JsonCatalogueLoaders;
using DialogueSystem;
using DialogueSystem.Interfaces;
using DialogueSystem.Units;
using GamePlayManagement.BitDescriptions.Suppliers;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Utils;

namespace DataUnits
{
    public interface ISupplierBaseObject
    {
        public int StorePhoneNumber { get; set; }
        public string StoreName { get; set; }
    }
    public interface IJobSupplierObject : ISupplierBaseObject, ICallableSupplier
    {
        public BitGameJobSuppliers BitId { get; set; }
        public string StoreType{ get; set; }
        public string StoreOwnerName{ get; set; }
        public int StoreUnlockPoints{ get; set; }
        public string StoreDescription{ get; set; }
        public void LoadDialogueData();
    }
    [System.Serializable]
    [CreateAssetMenu(menuName = "Jobs/JobSource")]
    public class JobSupplierObject : ScriptableObject, IJobSupplierObject
    {
        public BitGameJobSuppliers BitId { get; set; }
        public string StoreType{ get; set; }
        public string StoreName{ get; set; }
        public string StoreOwnerName{ get; set; }
        public int StoreUnlockPoints{ get; set; }
        public string StoreDescription{ get; set; }
        public int StorePhoneNumber{ get; set; }
        public DialogueSpeakerId SpeakerIndex { get; set; }

        private int _callAttempts = 0;

        private Dictionary<int, IDialogueObject> _mUnderThresholdDialogues = new Dictionary<int, IDialogueObject>();
        private Dictionary<int, IDialogueObject> _mOverThresholdDialogues = new Dictionary<int, IDialogueObject>();
        
        private SupplierDialoguesData _mDialogueData; 

        public void LoadDialogueData()
        {
            if (SpeakerIndex == 0)
            {
                Debug.LogWarning("Speaker Index must be set before loading data");
                return;
            }
            Debug.Log($"START: COLLECTING SPEAKER DATA FOR {StoreName}");
            var url = DataSheetUrls.JobSupplierDialogueGameData(SpeakerIndex);
            
            DialogueOperator.Instance.LoadJobSupplierDialogues(LoadDialogueDataFromServer(url));
        }
        
        
        
        private IEnumerator LoadDialogueDataFromServer(string url)
        {
            //
            var webRequest = UnityWebRequest.Get(url);
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Jobs Catalogue Data must be reachable. Error: {webRequest.result}. {webRequest.error}");
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

                    if (StoreUnlockPoints >= dialogueIndex)
                    {
                        _mUnderThresholdDialogues.Add(dialogueIndex, baseDialogueObject);
                    }
                    else
                    {
                        _mOverThresholdDialogues.Add(dialogueIndex, baseDialogueObject);
                    }
                }

                var dialogueObject = StoreUnlockPoints >= dialogueIndex
                    ? _mUnderThresholdDialogues
                    : _mOverThresholdDialogues;
                
                var dialogueLine = _mDialogueData.values[i][1];
                dialogueObject[dialogueIndex].DialogueLines.Add(dialogueLine);
            }
        }
        
        public void ReceiveCall(int playerLevel)
        {
            if (playerLevel < StoreUnlockPoints)
            {
                RandomDeflection();
            }
        }
        
        private void RandomDeflection()
        {
        }
    }

    public class SupplierDialoguesData : CatalogueFromDataGeneric
    {
        
    }
}