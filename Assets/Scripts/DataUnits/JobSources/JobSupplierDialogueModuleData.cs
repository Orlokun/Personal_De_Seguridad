using System;
using System.Collections;
using System.Collections.Generic;
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
    public class JobSupplierDialogueModuleData : IJobSupplierDialogueModuleData
    {
        private IJobSupplierObject _supplierObject;
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
        
        public int NpcRequirementStatus { get; }
        private int CurrentRequirementsStatus = 0;
        
        #region JsonPhoneDialogueManagement
        private SupplierDialoguesData _mImportantDialoguesData; 
        private SupplierDialoguesData _mDeflectionDialoguesData; 
        private SupplierDialoguesData _mInsistenceDialoguesData;
        private SupplierDialoguesData _mSupplierCallDialoguesData; 
        private SupplierDialoguesData _mSuppliersCallDialoguesDataString;

        public JobSupplierDialogueModuleData(IJobSupplierObject supplierObject)
        {
            _supplierObject = supplierObject;
        }
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
        public IEnumerator DownloadDialogueData(DialogueType dialogueType, string url)
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
                        LoadPhoneCallsDialogues(sourceJson);
                        break;
                    case DialogueType.CallingDialoguesData:
                        Debug.Log($"[DownloadDialogueData.Deflections]  Loading {_supplierObject.StoreName} CallingDialogues");
                        LoadPhoneCallsDialogueData(sourceJson);
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
            var lastStatusIndex = 0;
            var pureDialogueIndex = 0;
            
            IDialogueObject currentDialogueObject;
            for (var i = 1; i < _mImportantDialoguesData.values.Count; i++)
            {
                var isStatusAssigned = int.TryParse(_mImportantDialoguesData.values[i][0], out var currentDialogueStatusIndex);

                var isDialogueNodeIndex = int.TryParse(_mImportantDialoguesData.values[i][1], out var currentDialogueObjectIndex);
                if (currentDialogueObjectIndex == 0 || !isDialogueNodeIndex)
                {
                    Debug.LogWarning($"[JobSupplierObject.LoadDialoguesFromJson] Dialogues for {_supplierObject.StoreName} must have node Index greater than zero");
                    return;
                }
                
                
                if (lastDialogueObjectIndex != currentDialogueObjectIndex || i == 1 || currentDialogueStatusIndex != lastStatusIndex)
                {
                    pureDialogueIndex++;
                    lastDialogueObjectIndex = currentDialogueObjectIndex;
                    lastStatusIndex = currentDialogueStatusIndex;
                    currentDialogueObject = ScriptableObject.CreateInstance<DialogueObject>();
                    if (currentDialogueStatusIndex != 0)
                    {
                        currentDialogueObject.SetDialogueStatus(currentDialogueStatusIndex);
                    }
                    _mImportantDialogues.Add(pureDialogueIndex, currentDialogueObject);
                }
                
                var hasDialogueNodeId = int.TryParse(_mImportantDialoguesData.values[i][2], out var dialogueLineId);
                if (!hasDialogueNodeId)
                {
                    Debug.LogWarning($"[JobSupplierObject.LoadDialoguesFromJson] Dialogues for {_supplierObject.StoreName} must have Index greater than zero");
                    return;
                }
                var isSpeakerId = int.TryParse(_mImportantDialoguesData.values[i][3], out var speakerId);
                if (!isSpeakerId)
                {
                    Debug.LogWarning($"[JobSupplierObject.LoadDialoguesFromJson] Dialogues for {_supplierObject.StoreName} must have Index greater than zero");
                    return;
                }

                var dialogueLineText = _mImportantDialoguesData.values[i][4];
                var cameraTargetName = _mImportantDialoguesData.values[i][5];
                var hasCameraTarget = cameraTargetName != "0";
                var eventNameId = _mImportantDialoguesData.values[i][6];
                var hasEventId = eventNameId != "0";
                
                var linksToString = _mImportantDialoguesData.values[i][7].Split(',');
                var linksToInts = DialogueProcessor.ProcessLinksStrings(linksToString);
                var linksToFinish = linksToInts[0] == 0;
                var hasChoices = linksToInts.Length > 1;

                var dialogueNode = new DialogueNodeData(currentDialogueObjectIndex, dialogueLineId, speakerId, dialogueLineText,
                    hasCameraTarget, cameraTargetName, hasChoices, hasEventId, eventNameId, linksToInts);
                _mImportantDialogues[pureDialogueIndex].DialogueNodes.Add(dialogueNode);
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
                
                int.TryParse(_mDeflectionDialoguesData.values[i][2], out var speakerId);

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
                var isDialogueStatusSet = int.TryParse(_mInsistenceDialoguesData.values[i][0], out var currentDialogueStatusIndex);
                
                var isDialogueGroupId = int.TryParse(_mInsistenceDialoguesData.values[i][1], out var currentDialogueObjectIndex);
                
                if (currentDialogueObjectIndex == 0 || !isDialogueGroupId)
                {
                    Debug.LogWarning($"[JobSupplierObject.LoadDialoguesFromJson] Dialogues for {_supplierObject.StoreName} must have node Index greater than zero");
                    return;
                }
                if (lastDialogueObjectIndex != currentDialogueObjectIndex || i == 1)
                {
                    lastDialogueObjectIndex = currentDialogueObjectIndex;
                    currentDialogueObject = ScriptableObject.CreateInstance<DialogueObject>();
                    currentDialogueObject.SetDialogueStatus(currentDialogueStatusIndex);
                    _mInsistenceDialogues.Add(currentDialogueObjectIndex, currentDialogueObject);
                }
                
                var hasDialogueNodeId = int.TryParse(_mInsistenceDialoguesData.values[i][2], out var dialogueLineId);
                if (!hasDialogueNodeId)
                {
                    Debug.LogWarning($"[JobSupplierObject.LoadDialoguesFromJson] Dialogues for {_supplierObject.StoreName} must have Index greater than zero");
                    return;
                }
                
                int.TryParse(_mInsistenceDialoguesData.values[i][3], out var speakerId);

                var dialogueLineText = _mInsistenceDialoguesData.values[i][4];
                var cameraTargetName = _mInsistenceDialoguesData.values[i][5];
                var hasCameraTarget = cameraTargetName != "0";
                var eventNameId = _mInsistenceDialoguesData.values[i][6];
                var hasEventId = eventNameId != "0";
                
                var linksToString = _mInsistenceDialoguesData.values[i][7].Split(',');
                var linksToInts = DialogueProcessor.ProcessLinksStrings(linksToString);
                var linksToFinish = linksToInts[0] == 0;
                var hasChoices = linksToInts.Length > 1;

                var dialogueNode = new DialogueNodeData(currentDialogueObjectIndex, dialogueLineId, speakerId, dialogueLineText,
                    hasCameraTarget, cameraTargetName, hasChoices, hasEventId, eventNameId, linksToInts);
                _mInsistenceDialogues[currentDialogueObjectIndex].DialogueNodes.Add(dialogueNode);
            }
        }
        private void LoadPhoneCallsDialogues(string sourceJson)
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
        private void LoadPhoneCallsDialogueData(string sourceJson)
        {
            Debug.Log($"[JobSupplier.LoadPhoneCallsDialogueData] Begin request for {_supplierObject.StoreName} Dialogues Data");
            _mSuppliersCallDialoguesDataString = JsonConvert.DeserializeObject<SupplierDialoguesData>(sourceJson);
            _mSupplierCallData = new Dictionary<int, ISupplierCallDialogueDataObject>();

            var lastDialogueObjectIndex = 0;
            
            for (var i = 1; i < _mSuppliersCallDialoguesDataString.values.Count; i++)
            {
                var isDialogueGroupId = int.TryParse(_mSuppliersCallDialoguesDataString.values[i][0], out var dialogueGroupIndex);
                if (dialogueGroupIndex == 0 || !isDialogueGroupId)
                {
                    Debug.LogWarning($"[JobSupplierObject.LoadPhoneCallsDialogueData] Dialogues data for {_supplierObject.StoreName} reference to dialogue Index greater than zero");
                    return;
                }

                var hasJobDayCount = int.TryParse(_mSuppliersCallDialoguesDataString.values[i][1], out var jobDayCount);
                if (jobDayCount == 0 || !hasJobDayCount)
                {
                    Debug.LogWarning($"[JobSupplierObject.LoadPhoneCallsDialogueData] Dialogues for {_supplierObject.StoreName} must have Job Day Count greater than zero");
                    return;
                }
                
                var hasTimeOfDay = Enum.TryParse<PartOfDay>(_mSuppliersCallDialoguesDataString.values[i][2], out var partOfDay);
                if (partOfDay == 0 || !hasTimeOfDay)
                {
                    Debug.LogWarning($"[JobSupplierObject.LoadPhoneCallsDialogueData] Dialogues for {_supplierObject.StoreName} must have Time of day greater than zero");
                    return;
                }
                
                var callTime = _mSuppliersCallDialoguesDataString.values[i][3].Split(',');
                var hasCallHour = int.TryParse(callTime[0], out var callHour);
                if (callHour == 0 || !hasCallHour)
                {
                    Debug.LogWarning($"[JobSupplierObject.LoadPhoneCallsDialogueData] Dialogues for {_supplierObject.StoreName} must have call hour greater than zero");
                    return;
                }
                var hasCallMinute = int.TryParse(callTime[1], out var callMinute);
                if (callMinute == 0 || !hasCallMinute)
                {
                    Debug.LogWarning($"[JobSupplierObject.LoadPhoneCallsDialogueData] Dialogues for {_supplierObject.StoreName} must have call hour greater than zero");
                    return;
                }

                var currentDialogueObject = Factory.CreateSupplierCallDialogueDataObject(dialogueGroupIndex, jobDayCount, callHour,
                    callMinute, partOfDay);
                _mSupplierCallData.Add(dialogueGroupIndex, currentDialogueObject);
            }
            Debug.Log($"[JobSupplier.LoadPhoneCallsDialogueData] Finished request for {_supplierObject.StoreName} Dialogues Data");
        }
        #endregion
    }
}