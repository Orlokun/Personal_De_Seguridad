using System;
using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using DialogueSystem.Interfaces;
using DialogueSystem.Units;
using GameDirection.TimeOfDayManagement;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Utils;

namespace GameDirection.DayLevelSceneManagers
{

    
    public abstract class DayLevelSceneManagement : ILevelDayManager
    {
        public delegate void FinishCurrentDialogue();
        public event FinishCurrentDialogue OnFinishCurrentDialogue;
        protected DayBitId DayId;
        protected bool mInitialized;
        protected IGameDirector MGameDirector;
        protected Dictionary<int, IDialogueObject> DayBaseDialogues = new Dictionary<int, IDialogueObject>();
        protected IDialogueObject ModularDialogue;
        protected int DialogueIndex;
    
        
        protected DialogueObjectsFromData DialoguesBaseDataString;

        public bool MInitialized => mInitialized;
        public void Initialize(IGameDirector injectionClass1, DayBitId injectionClass2)
        {
            if (MInitialized)
            {
                return;
            }
            if (injectionClass1 == null || injectionClass2 == 0)
            {
                Debug.Log("[IntroSceneManager.InitializeWithArg] Injection must not be null");
                return;
            }
            MGameDirector = injectionClass1;
            DayId = injectionClass2;
            LoadDayData();
            mInitialized = true;
        }
        private void LoadDayData()
        {
            var dayDataUrl = DataSheetUrls.GetDaySceneDialogueDataUrl(DayId);    
            MGameDirector.ActCoroutine(GetDaySceneDialogueData(dayDataUrl));
        }
        private IEnumerator GetDaySceneDialogueData(string url)
        {
            var webRequest = UnityWebRequest.Get(url);
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("GetItems Catalogue Data must be reachable");
            }
            else
            {
                var sourceJson = webRequest.downloadHandler.text;
                LoadFromJson(sourceJson);
            }
        }
        private void LoadFromJson(string sourceJson)
        {
            Debug.Log($"DialoguesInSceneDataManager.LoadFromJson");
            Debug.Log($"StartParsing Dialogue Objects: {sourceJson}");

            DialoguesBaseDataString = JsonConvert.DeserializeObject<DialogueObjectsFromData>(sourceJson);
            Debug.Log($"Finished parsing Dialogue Data null?: {DialoguesBaseDataString == null}");
            DayBaseDialogues = new Dictionary<int, IDialogueObject>();
            var lastDialogueIndex = 0;
            if (DialoguesBaseDataString == null)
            {
                return;
            }
            
            IDialogueObject currentDialogueObject;
            for (var i = 1; i < DialoguesBaseDataString.values.Count;i++)
            {
                var isDialogueNodeIndex = int.TryParse(DialoguesBaseDataString.values[i][0], out var currentDialogueObjectIndex);
                if (!isDialogueNodeIndex)
                {
                    Debug.LogWarning($"[JobSupplierObject.LoadDialoguesFromJson] Dialogues for Intro must have node Index greater than zero");
                    return;
                }
                if (lastDialogueIndex != currentDialogueObjectIndex || i == 1)
                {
                    lastDialogueIndex = currentDialogueObjectIndex;
                    currentDialogueObject = ScriptableObject.CreateInstance<DialogueObject>();
                    DayBaseDialogues.Add(currentDialogueObjectIndex, currentDialogueObject);
                }
                
                var hasDialogueNodeId = int.TryParse(DialoguesBaseDataString.values[i][1], out var dialogueLineId);
                if (!hasDialogueNodeId)
                {
                    Debug.LogWarning($"[IntroSceneManagement.LoadFromJson] Dialogues for Intro must have dialoge node id");
                    return;
                }
                var isSpeakerId = int.TryParse(DialoguesBaseDataString.values[i][2], out var speakerId);
                if (speakerId == 0 || !isSpeakerId)
                {
                    Debug.LogWarning($"[IntroSceneManagement.LoadFromJson] Dialogues for Intro must have speaker Index greater than zero");
                    //return;
                }

                var dialogueLineText = DialoguesBaseDataString.values[i][3];
                var cameraTargetName = DialoguesBaseDataString.values[i][4];
                var hasCameraTarget = cameraTargetName != "0";
                var eventNameId = DialoguesBaseDataString.values[i][5];
                var hasEventId = eventNameId != "0";
                
                var linksToString = DialoguesBaseDataString.values[i][6].Split(',');
                var linksToInts = DialogueProcessor.ProcessLinksStrings(linksToString);
                var linksToFinish = linksToInts[0] == 0;
                var hasChoices = linksToInts.Length > 1;

                var dialogueNode = new DialogueNodeData(currentDialogueObjectIndex, dialogueLineId, speakerId, dialogueLineText,
                    hasCameraTarget, cameraTargetName, hasChoices, hasEventId, eventNameId, linksToInts);
                DayBaseDialogues[currentDialogueObjectIndex].DialogueNodes.Add(dialogueNode);
            }
        }

        public virtual IEnumerator StartDayManagement()
        {
            throw new NotImplementedException();
        }

        protected virtual void OnFinishCurrentDialogueEvent()
        {
            OnFinishCurrentDialogue?.Invoke();
        }

        protected virtual void FinishIntroductionText()
        {
            throw new NotImplementedException();
        }

        protected virtual IEnumerator StartIntroductionReading()
        {
            return null;
        }
        protected virtual IEnumerator StartModularDialogueReading()
        {
            return null;
        }
        protected virtual void ReleaseFromDialogueStateAndStartClock()
        {
            MGameDirector.ReleaseFromDialogueStateToGame();
            MGameDirector.GetUIController.ReturnToBaseGamePlayCanvasState();
            MGameDirector.GetClockInDayManagement.SetClockAtDaytime(PartOfDay.EarlyMorning);
            MGameDirector.GetClockInDayManagement.PlayPauseClock(true);
            MGameDirector.GetDialogueOperator.OnDialogueCompleted -= ReleaseFromDialogueStateAndStartClock;
        }
    }
}