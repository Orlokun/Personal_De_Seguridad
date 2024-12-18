using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataUnits.GameCatalogues.JsonCatalogueLoaders;
using DialogueSystem;
using DialogueSystem.Interfaces;
using DialogueSystem.Units;
using GameDirection;
using GameDirection.TimeOfDayManagement;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Utils;

namespace DataUnits.JobSources
{
    public interface IJobSupplierDialogueModule
    {
        void LoadInitialDeflectionDialogues();
        void StartUnlockDialogueData();
        public Dictionary<int, IDialogueObject> DeflectionDialogues { get; }
        public Dictionary<int, IDialogueObject> ImportantDialogues { get; }
        public Dictionary<int, IDialogueObject> InsistenceDialogues { get; }
        public Dictionary<int, IDialogueObject> SupplierCallDialogues{ get; }
        public Dictionary<int, ISupplierCallDialogueDataObject> SupplierCallDialoguesDataDictionary { get; }
    }

    public class JobSupplierDialogueModule : IJobSupplierDialogueModule
    {
        private JobSupplierObject _supplierObject;
        public JobSupplierDialogueModule(JobSupplierObject supplierObject)
        {
            _supplierObject = supplierObject;
        }
        private Dictionary<int, IDialogueObject> _mDeflectionDialogues = new Dictionary<int, IDialogueObject>();
        private Dictionary<int, IDialogueObject> _mImportantDialogues = new Dictionary<int, IDialogueObject>();
        private Dictionary<int, IDialogueObject> _mInsistenceDialogues = new Dictionary<int, IDialogueObject>();
        private Dictionary<int, IDialogueObject> _mSupplierCallDialogues = new Dictionary<int, IDialogueObject>();
        private Dictionary<int, ISupplierCallDialogueDataObject> _mSupplierCallData = new Dictionary<int, ISupplierCallDialogueDataObject>();
    
        public Dictionary<int, IDialogueObject> DeflectionDialogues =>_mDeflectionDialogues;
        public Dictionary<int, IDialogueObject> ImportantDialogues =>_mImportantDialogues;
        public Dictionary<int, IDialogueObject> InsistenceDialogues =>_mInsistenceDialogues;
        public Dictionary<int, IDialogueObject> SupplierCallDialogues =>_mSupplierCallDialogues;
        public Dictionary<int, ISupplierCallDialogueDataObject> SupplierCallDialoguesDataDictionary =>_mSupplierCallData;
    

        #region JsonPhoneDialogueManagement
        private SupplierDialoguesData _mImportantDialoguesData; 
        private SupplierDialoguesData _mDeflectionDialoguesData; 
        private SupplierDialoguesData _mInsistenceDialoguesData;
        private SupplierDialoguesData _mSupplierCallDialoguesData; 
        private SupplierDialoguesData _mSuppliersCallDialoguesDataString; 
    
        public void LoadInitialDeflectionDialogues()
        {
            if (_supplierObject.SpeakerIndex == 0)
            {
                Debug.LogWarning("Speaker Index must be set before loading data");
                return;
            }
            Debug.Log($"START: COLLECTING SPEAKER DATA FOR {_supplierObject.StoreName}");
            var deflectionDialoguesUrl = DataSheetUrls.SuppliersDialogueGameData(_supplierObject.SpeakerIndex, DialogueType.Deflections);
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
                Debug.LogError($"Jobs Catalogue Data for {_supplierObject.StoreName} must be reachable. Error: {webRequest.result}. {webRequest.error}");
            }
            else
            {
                var sourceJson = webRequest.downloadHandler.text;
                switch (dialogueType)
                {
                    case DialogueType.ImportantDialogue:
                        Debug.Log($"[DownloadDialogueData.ImportantDialogue] Loading {_supplierObject.StoreName} Important dialogues");
                        LoadImportantDialoguesFromJson(sourceJson);
                        break;
                    case DialogueType.Deflections:
                        Debug.Log($"[DownloadDialogueData.Deflections]  Loading {_supplierObject.StoreName} Deflections dialogues");
                        LoadDeflectionDialoguesFromJson(sourceJson);
                        break;
                    case DialogueType.InsistenceDialogue:
                        Debug.Log($"[DownloadDialogueData.Deflections]  Loading {_supplierObject.StoreName} InsistenceDialogue");
                        LoadInsistenceDialogues(sourceJson);
                        break;
                    case DialogueType.CallingDialogues:
                        Debug.Log($"[DownloadDialogueData.Deflections]  Loading {_supplierObject.StoreName} CallingDialogues");
                        LoadCallDialoguesData(sourceJson);
                        break;
                    case DialogueType.CallingDialoguesData:
                        Debug.Log($"[DownloadDialogueData.Deflections]  Loading {_supplierObject.StoreName} CallingDialogues");
                        LoadCallDialoguesDataData(sourceJson);
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
            
            IDialogueObject currentDialogueObject;
            for (var i = 1; i < _mImportantDialoguesData.values.Count; i++)
            {
                var isDialogueNodeIndex = int.TryParse(_mImportantDialoguesData.values[i][0], out var currentDialogueObjectIndex);
                if (currentDialogueObjectIndex == 0 || !isDialogueNodeIndex)
                {
                    Debug.LogWarning($"[JobSupplierObject.LoadDialoguesFromJson] Dialogues for {_supplierObject.StoreName} must have node Index greater than zero");
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
                    Debug.LogWarning($"[JobSupplierObject.LoadDialoguesFromJson] Dialogues for {_supplierObject.StoreName} must have Index greater than zero");
                    return;
                }
                var isSpeakerId = int.TryParse(_mImportantDialoguesData.values[i][2], out var speakerId);
                if (!isSpeakerId)
                {
                    Debug.LogWarning($"[JobSupplierObject.LoadDialoguesFromJson] Dialogues for {_supplierObject.StoreName} must have Index greater than zero");
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
            Debug.Log($"[JobSupplier.LoadImportantDialoguesFromJson] Begin Random deflection dialogues request for {_supplierObject.StoreName}");
            _mDeflectionDialoguesData = JsonConvert.DeserializeObject<SupplierDialoguesData>(sourceJson);
            _mDeflectionDialogues = new Dictionary<int, IDialogueObject>();

            var lastDialogueObjectIndex = 0;
            if (_mDeflectionDialoguesData.values == null)
            {
                Debug.LogError($"[JobSupplier.LoadImportantDialoguesFromJson] Values were not properly loaded into {_supplierObject.StoreName} dialogues");
            }
            IDialogueObject currentDialogueObject;
            for (var i = 1; i < _mDeflectionDialoguesData.values.Count; i++)
            {
                var isDialogueNodeIndex = int.TryParse(_mDeflectionDialoguesData.values[i][0], out var currentDialogueObjectIndex);
                if (currentDialogueObjectIndex == 0 || !isDialogueNodeIndex)
                {
                    Debug.LogWarning($"[JobSupplierObject.LoadDialoguesFromJson] Dialogues for {_supplierObject.StoreName} must have node Index greater than zero");
                    return;
                }
                if (lastDialogueObjectIndex != currentDialogueObjectIndex || i == 1)
                {
                    lastDialogueObjectIndex = currentDialogueObjectIndex;
                    currentDialogueObject = ScriptableObject.CreateInstance<DialogueObject>();
                    _mDeflectionDialogues.Add(currentDialogueObjectIndex, currentDialogueObject);
                }
                
                var hasDialogueNodeId = int.TryParse(_mDeflectionDialoguesData.values[i][1], out var dialogueLineId);
                if (!hasDialogueNodeId)
                {
                    Debug.LogWarning($"[JobSupplierObject.LoadDialoguesFromJson] Dialogues for {_supplierObject.StoreName} must have Index greater than zero");
                    return;
                }
                var isSpeakerId = int.TryParse(_mDeflectionDialoguesData.values[i][2], out var speakerId);
                if (speakerId == 0 || !isSpeakerId)
                {
                    Debug.LogWarning($"[JobSupplierObject.LoadDialoguesFromJson] Dialogues for {_supplierObject.StoreName} must have Index greater than zero");
                    return;
                }

                var dialogueLineText = _mDeflectionDialoguesData.values[i][3];
                var cameraTargetName = _mDeflectionDialoguesData.values[i][4];
                var hasCameraTarget = cameraTargetName != "0";
                var eventNameId = _mDeflectionDialoguesData.values[i][5];
                var hasEventId = eventNameId != "0";
                
                var linksToString = _mDeflectionDialoguesData.values[i][6].Split(',');
                var linksToInts = DialogueProcessor.ProcessLinksStrings(linksToString);
                var linksToFinish = linksToInts[0] == 0;
                var hasChoices = linksToInts.Length > 1;

                var dialogueNode = new DialogueNodeData(currentDialogueObjectIndex, dialogueLineId, speakerId, dialogueLineText,
                    hasCameraTarget, cameraTargetName, hasChoices, hasEventId, eventNameId, linksToInts);
                _mDeflectionDialogues[currentDialogueObjectIndex].DialogueNodes.Add(dialogueNode);
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
                    Debug.LogWarning($"[JobSupplierObject.LoadDialoguesFromJson] Dialogues for {_supplierObject.StoreName} must have node Index greater than zero");
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
                    Debug.LogWarning($"[JobSupplierObject.LoadDialoguesFromJson] Dialogues for {_supplierObject.StoreName} must have Index greater than zero");
                    return;
                }
                var isSpeakerId = int.TryParse(_mInsistenceDialoguesData.values[i][2], out var speakerId);
                if (speakerId == 0 || !isSpeakerId)
                {
                    Debug.LogWarning($"[JobSupplierObject.LoadDialoguesFromJson] Dialogues for {_supplierObject.StoreName} must have Index greater than zero");
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
        private void LoadCallDialoguesData(string sourceJson)
        {
            //Debug.Log($"[JobSupplier.LoadImportantDialoguesFromJson] Begin request");
            _mSupplierCallDialoguesData = JsonConvert.DeserializeObject<SupplierDialoguesData>(sourceJson);
            //Debug.Log($"Finished parsing. Is Job Supplier Dialogue null?: {_mInsistenceDialoguesData == null}. {_mInsistenceDialoguesData}");
            _mSupplierCallDialogues = new Dictionary<int, IDialogueObject>();

            var lastDialogueObjectIndex = 0;
            
            IDialogueObject currentDialogueObject;
            for (var i = 1; i < _mSupplierCallDialoguesData.values.Count; i++)
            {
                var isDialogueNodeIndex = int.TryParse(_mSupplierCallDialoguesData.values[i][0], out var currentDialogueObjectIndex);
                if (currentDialogueObjectIndex == 0 || !isDialogueNodeIndex)
                {
                    Debug.LogWarning($"[JobSupplierObject.LoadDialoguesFromJson] Dialogues for {_supplierObject.StoreName} must have node Index greater than zero");
                    return;
                }
                if (lastDialogueObjectIndex != currentDialogueObjectIndex || i == 1)
                {
                    lastDialogueObjectIndex = currentDialogueObjectIndex;
                    currentDialogueObject = ScriptableObject.CreateInstance<DialogueObject>();
                    _mSupplierCallDialogues.Add(currentDialogueObjectIndex, currentDialogueObject);
                }
                
                var hasDialogueNodeId = int.TryParse(_mSupplierCallDialoguesData.values[i][1], out var dialogueLineId);
                if (!hasDialogueNodeId)
                {
                    Debug.LogWarning($"[JobSupplierObject.LoadDialoguesFromJson] Dialogues for {_supplierObject.StoreName} must have Index available");
                }
                var isSpeakerId = int.TryParse(_mSupplierCallDialoguesData.values[i][2], out var speakerId);
                if (!isSpeakerId)
                {
                    Debug.LogWarning($"[JobSupplierObject.LoadDialoguesFromJson] Dialogues for {_supplierObject.StoreName} must have Index greater than zero");
                }

                var dialogueLineText = _mSupplierCallDialoguesData.values[i][3];
                var cameraTargetName = _mSupplierCallDialoguesData.values[i][4];
                var hasCameraTarget = cameraTargetName != "0";
                var eventNameId = _mSupplierCallDialoguesData.values[i][5];
                var hasEventId = eventNameId != "0";
                
                var linksToString = _mSupplierCallDialoguesData.values[i][6].Split(',');
                var linksToInts = DialogueProcessor.ProcessLinksStrings(linksToString);
                var linksToFinish = linksToInts[0] == 0;
                var hasChoices = linksToInts.Length > 1;

                var dialogueNode = new DialogueNodeData(currentDialogueObjectIndex, dialogueLineId, speakerId, dialogueLineText,
                    hasCameraTarget, cameraTargetName, hasChoices, hasEventId, eventNameId, linksToInts);
                _mSupplierCallDialogues[currentDialogueObjectIndex].DialogueNodes.Add(dialogueNode);
            }
        }
        private void LoadCallDialoguesDataData(string sourceJson)
        {
            Debug.Log($"[JobSupplier.LoadCallDialoguesDataData] Begin request for {_supplierObject.StoreName} Dialogues Data");
            _mSuppliersCallDialoguesDataString = JsonConvert.DeserializeObject<SupplierDialoguesData>(sourceJson);
            _mSupplierCallData = new Dictionary<int, ISupplierCallDialogueDataObject>();

            var lastDialogueObjectIndex = 0;
            
            for (var i = 1; i < _mSuppliersCallDialoguesDataString.values.Count; i++)
            {
                var isDialogueGroupId = int.TryParse(_mSuppliersCallDialoguesDataString.values[i][0], out var dialogueGroupIndex);
                if (dialogueGroupIndex == 0 || !isDialogueGroupId)
                {
                    Debug.LogWarning($"[JobSupplierObject.LoadCallDialoguesDataData] Dialogues data for {_supplierObject.StoreName} reference to dialogue Index greater than zero");
                    return;
                }

                var hasJobDayCount = int.TryParse(_mSuppliersCallDialoguesDataString.values[i][1], out var jobDayCount);
                if (jobDayCount == 0 || !hasJobDayCount)
                {
                    Debug.LogWarning($"[JobSupplierObject.LoadCallDialoguesDataData] Dialogues for {_supplierObject.StoreName} must have Job Day Count greater than zero");
                    return;
                }
                
                var hasTimeOfDay = Enum.TryParse<PartOfDay>(_mSuppliersCallDialoguesDataString.values[i][2], out var partOfDay);
                if (partOfDay == 0 || !hasTimeOfDay)
                {
                    Debug.LogWarning($"[JobSupplierObject.LoadCallDialoguesDataData] Dialogues for {_supplierObject.StoreName} must have Time of day greater than zero");
                    return;
                }
                
                var callTime = _mSuppliersCallDialoguesDataString.values[i][3].Split(',');
                var hasCallHour = int.TryParse(callTime[0], out var callHour);
                if (callHour == 0 || !hasCallHour)
                {
                    Debug.LogWarning($"[JobSupplierObject.LoadCallDialoguesDataData] Dialogues for {_supplierObject.StoreName} must have call hour greater than zero");
                    return;
                }
                var hasCallMinute = int.TryParse(callTime[1], out var callMinute);
                if (callMinute == 0 || !hasCallMinute)
                {
                    Debug.LogWarning($"[JobSupplierObject.LoadCallDialoguesDataData] Dialogues for {_supplierObject.StoreName} must have call hour greater than zero");
                    return;
                }

                var currentDialogueObject = Factory.CreateSupplierCallDialogueDataObject(dialogueGroupIndex, jobDayCount, callHour,
                    callMinute, partOfDay);
                _mSupplierCallData.Add(dialogueGroupIndex, currentDialogueObject);
            }
            Debug.Log($"[JobSupplier.LoadCallDialoguesDataData] Finis request for {_supplierObject.StoreName} Dialogues Data");
        }
        #endregion
        
   
        public void StartUnlockDialogueData()
        {
            GetUnlockedDialogues();
            GameDirector.Instance.GetClockInDayManagement.OnPassMinute += _supplierObject.CheckCallingTime;
        }
        private async void GetUnlockedDialogues()
        {
            var importantDialoguesUrl = DataSheetUrls.SuppliersDialogueGameData(_supplierObject.SpeakerIndex, DialogueType.ImportantDialogue);
            var insistenceDialoguesUrl = DataSheetUrls.SuppliersDialogueGameData(_supplierObject.SpeakerIndex, DialogueType.InsistenceDialogue);
            var callDialoguesUrl = DataSheetUrls.SuppliersDialogueGameData(_supplierObject.SpeakerIndex, DialogueType.CallingDialogues);
            var callDialoguesDataUrl = DataSheetUrls.SuppliersDialogueGameData(_supplierObject.SpeakerIndex, DialogueType.CallingDialoguesData);
            await Task.Delay(300);
            GameDirector.Instance.ActCoroutine(DownloadDialogueData(DialogueType.ImportantDialogue, importantDialoguesUrl));
            await Task.Delay(500);
            GameDirector.Instance.ActCoroutine(DownloadDialogueData(DialogueType.InsistenceDialogue, insistenceDialoguesUrl));
            await Task.Delay(500);
            GameDirector.Instance.ActCoroutine(DownloadDialogueData(DialogueType.CallingDialogues, callDialoguesUrl));
            await Task.Delay(500);
            GameDirector.Instance.ActCoroutine(DownloadDialogueData(DialogueType.CallingDialoguesData, callDialoguesDataUrl));
        }
    }
}