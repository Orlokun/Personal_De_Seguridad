using System;
using System.Collections;
using System.Collections.Generic;
using DataUnits.GameCatalogues.JsonCatalogueLoaders;
using DialogueSystem.Interfaces;
using DialogueSystem.Units;
using GameDirection;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Utils;

namespace DialogueSystem
{
    public enum OmniIntroDialogues
    {
        NoJobIntro1 = 1,
        NoJobIntro2,
        NoJobIntro3,
        JobIntro1,
        JobIntro2,
        FirstJob1
    }

    public interface IModularDialogueDataController : IInitialize
    {
        
    }
    
    public class ModularDialogueDataController : IModularDialogueDataController
    {
        private Dictionary<OmniIntroDialogues, IDialogueObject> _modularIntroDialogues = new Dictionary<OmniIntroDialogues, IDialogueObject>();
        private IntroModularDialoguesData _modularDialoguesData;
        
        private bool _mIsInitialized;
        public bool IsInitialized => _mIsInitialized;
        
        public void Initialize()
        {
            if (_mIsInitialized)
            {
                return;
            }
            StartModularIntroDialoguesLoadData();
            _mIsInitialized = true;
        }
        private void StartModularIntroDialoguesLoadData()
        {
            var modularDialoguesUrl = DataSheetUrls.ModularDialoguesDataUrl;
            GameDirector.Instance.ActCoroutine(DownloadDialogueData(modularDialoguesUrl));
        }
        private IEnumerator DownloadDialogueData(string url)
        {
            //
            var webRequest = UnityWebRequest.Get(url);
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(
                    $"Modular dialogues data must be reachable. Error: {webRequest.result}. {webRequest.error}");
            }
            else
            {
                var sourceJson = webRequest.downloadHandler.text;
                LoadModularDialoguesData(sourceJson);
            }
        }
        private void LoadModularDialoguesData(string sourceJson)
        {
            Debug.Log($"[ModularDialogueDataController.LoadModularDialoguesData] Begin request");
            _modularDialoguesData = JsonConvert.DeserializeObject<IntroModularDialoguesData>(sourceJson);
            //Debug.Log($"Finished parsing. Is Job Supplier Dialogue null?: {_mDialogueData == null}. {_mDialogueData}");
            _modularIntroDialogues = new Dictionary<OmniIntroDialogues, IDialogueObject>();

            OmniIntroDialogues lastDialogueObjectIndex = 0;
            
            IDialogueObject currentDialogueObject;
            for (var i = 1; i < _modularDialoguesData.values.Count; i++)
            {
                var hasDialogueModuleId = Enum.TryParse<OmniIntroDialogues>(_modularDialoguesData.values[i][0], out var currentDialogueModuleId);
                if (currentDialogueModuleId == 0 || !hasDialogueModuleId)
                {
                    Debug.LogWarning($"[LoadModularDialoguesData] Modular Dialogue must have an Enum Id");
                    return;
                }
                if (lastDialogueObjectIndex != currentDialogueModuleId || i == 1)
                {
                    lastDialogueObjectIndex = currentDialogueModuleId;
                    currentDialogueObject = ScriptableObject.CreateInstance<DialogueObject>();
                    _modularIntroDialogues.Add(currentDialogueModuleId, currentDialogueObject);
                }
                
                var hasDialogueNodeId = int.TryParse(_modularDialoguesData.values[i][1], out var dialogueNodeId);
                if (!hasDialogueNodeId)
                {
                    Debug.LogWarning($"[LoadModularDialoguesData] Modular Dialogue node must have a node Id");
                    return;
                }
                var isSpeakerId = int.TryParse(_modularDialoguesData.values[i][2], out var speakerId);
                if (!isSpeakerId)
                {
                    Debug.LogWarning($"[LoadModularDialoguesData] Modular Dialogue node must have a Speaker Id");
                    return;
                }

                var dialogueLineText = _modularDialoguesData.values[i][3];
                var cameraTargetName = _modularDialoguesData.values[i][4];
                var hasCameraTarget = cameraTargetName != "0";
                var eventNameId = _modularDialoguesData.values[i][5];
                var hasEventId = eventNameId != "0";
                
                var linksToString = _modularDialoguesData.values[i][6].Split(',');
                var linksToInts = DialogueProcessor.ProcessLinksStrings(linksToString);
                var linksToFinish = linksToInts[0] == 0;
                var hasChoices = linksToInts.Length > 1;

                var dialogueNode = new DialogueNodeData(currentDialogueModuleId, dialogueNodeId, speakerId, dialogueLineText,
                    hasCameraTarget, cameraTargetName, hasChoices, hasEventId, eventNameId, linksToInts);
                _modularIntroDialogues[currentDialogueModuleId].DialogueNodes.Add(dialogueNode);
            }
            Debug.Log($"[ModularDialogueDataController.LoadModularDialoguesData] Finish request");
        }
    }
}