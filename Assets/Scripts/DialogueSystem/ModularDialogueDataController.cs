using System;
using System.Collections;
using System.Collections.Generic;
using DataUnits.GameCatalogues.JsonCatalogueLoaders;
using DialogueSystem.Interfaces;
using DialogueSystem.Units;
using GameDirection;
using GamePlayManagement;
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
        FirstJob1,
        PayNoRent1
    }

    public interface IModularDialogueDataController : IInitialize
    {
        public IDialogueObject CreateInitialDayIntro(IPlayerGameProfile currentPlayer);
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

        public IDialogueObject CreateInitialDayIntro(IPlayerGameProfile currentPlayer)
        {
            var modularDialogues = new List<OmniIntroDialogues>();
            ProcessJobModularDialogues(modularDialogues,currentPlayer);
            var modularInitDialogue = ConvertModuleDialoguesIntoOne(modularDialogues);
            return modularInitDialogue;
        }

        private IDialogueObject ConvertModuleDialoguesIntoOne(List<OmniIntroDialogues> dialogues)
        {
            IDialogueObject modularInitDialogue = ScriptableObject.CreateInstance<DialogueObject>();
            foreach (var dialogue in dialogues)
            {
                if (!_modularIntroDialogues.ContainsKey(dialogue))
                {
                    continue;
                }
                var dialogueList = _modularIntroDialogues[dialogue].DialogueNodes;
                foreach (var dialogueNode in dialogueList)
                {
                    modularInitDialogue.DialogueNodes.Add(dialogueNode);
                }
            }
            //Makes sure all the dialogues are correctly linked. LIMITATION: Do not add choices in intro modular dialogues
            for (var i = 0; i < modularInitDialogue.DialogueNodes.Count; i++)
            {
                if (i != modularInitDialogue.DialogueNodes.Count - 1)
                {
                    modularInitDialogue.DialogueNodes[i].LinkNodes = new[] {i + 1};
                    continue;
                }
                modularInitDialogue.DialogueNodes[i].LinkNodes = new[] {0};
            }
            return modularInitDialogue;
        }
        private void ProcessJobModularDialogues(List<OmniIntroDialogues> modularDialogues, IPlayerGameProfile currentPlayer)
        {
            var playerJobModule = currentPlayer.GetActiveJobsModule();
            var isPlayerEmployed = playerJobModule.CurrentEmployer != 0;
            
         
            int employmentStreak;
            int totalDaysEmployed;

            int unemployedStreak;
            int unemployedTotalDays;
            
            if (isPlayerEmployed)
            {
                employmentStreak = playerJobModule.DaysEmployedStreak;
                totalDaysEmployed = playerJobModule.TotalDaysEmployed;
                TurnEmploymentDataToDialogueEnum(modularDialogues,true, employmentStreak, totalDaysEmployed);
            }
            else
            {
                unemployedStreak = playerJobModule.DaysUnemployedStreak;
                unemployedTotalDays = playerJobModule.TotalDaysUnemployed;
                TurnEmploymentDataToDialogueEnum(modularDialogues,false, unemployedStreak, unemployedTotalDays);
            }
        }

        private void TurnEmploymentDataToDialogueEnum(List<OmniIntroDialogues> modularDialogues, bool isPlayerEmployed, int employmentStreak, int employmentTotal)
        {
            var baseDialogueName = "";
            var streakString = employmentStreak.ToString();
            var jobDialogueName = "";
            
            if (isPlayerEmployed)
            {
                baseDialogueName = "JobIntro";
                jobDialogueName = baseDialogueName + streakString;
            }
            else
            {
                baseDialogueName = "NoJobIntro";
                jobDialogueName = baseDialogueName + streakString;
            }
            var isJobEnum = Enum.TryParse<OmniIntroDialogues>(jobDialogueName, out var jobIntroDialogueEnum);
            if (!isJobEnum)
            {
                Debug.LogWarning("[TurnEmploymentDataToDialogueEnum] Current Data must be available as OmniModular dialogue enum");
                return;
            }
            modularDialogues.Add(jobIntroDialogueEnum);
            if (isPlayerEmployed && employmentTotal == 1)
            {
                modularDialogues.Add(OmniIntroDialogues.FirstJob1);
            }
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