using System;
using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using DialogueSystem.Interfaces;
using DialogueSystem.Units;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement.BitDescriptions.Suppliers;
using GamePlayManagement.LevelManagement;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.CustomerInterfaces;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Utils;

namespace GameDirection.DayLevelSceneManagers
{

    public class DayZeroIntroScene : IIntroSceneOperator
    {
        public bool IsInitialized => mInitialized;
        private bool mInitialized;
        private IGameDirector _mGameDirector;

        
        public void Initialize(IGameDirector injectionClass)
        {
            if (IsInitialized)
            {
                return;
            }
            if (injectionClass == null)
            {
                Debug.Log("[IntroSceneManager.InitializeWithArg] Injection must not be null");
                return;
            }
            _mGameDirector = injectionClass;
            mInitialized = true;
        }

        public IEnumerator StartIntroScene()
        {
            throw new NotImplementedException();
        }
    }
    
    
    
    public abstract class DayLevelSceneManagement : ILevelDayManager
    {
        public delegate void FinishCurrentDialogue();
        public event FinishCurrentDialogue OnFinishCurrentDialogue;
        protected DayBitId DayId;
        
        protected IGameDirector MGameDirector;
        protected IDialogueObject ModularDialogue;
        protected ICustomersInSceneManager _customerSpawner;
        protected DialogueObjectsFromData DialoguesBaseDataString;
        protected Dictionary<int, IDialogueObject> DayBaseDialogues = new Dictionary<int, IDialogueObject>();

        protected bool mInitialized;
        protected int IntroDialogueIndex;

        
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
                int.TryParse(DialoguesBaseDataString.values[i][2], out var speakerId);

                var dialogueLineText = DialoguesBaseDataString.values[i][3];
                var cameraArgs = DialoguesBaseDataString.values[i][4].Split(',');
                var hasCameraTarget = cameraArgs.Length >1;
                var eventNameId = DialoguesBaseDataString.values[i][5];
                var hasEventId = eventNameId != "0";
                
                var linksToString = DialoguesBaseDataString.values[i][6].Split(',');
                var linksToInts = DialogueProcessor.ProcessLinksStrings(linksToString);
                var linksToFinish = linksToInts[0] == 0;
                var hasChoices = linksToInts.Length > 1;
                
                //Highlight event
                var hasHighlightEvent = DialoguesBaseDataString.values[i][7] != "0";
                var emptyString = new string[1] {"0"};
                var highlightEvent = hasHighlightEvent ? DialoguesBaseDataString.values[i][7].Split(',') : emptyString;

                var dialogueNode = new DialogueNodeData(currentDialogueObjectIndex, dialogueLineId, speakerId, dialogueLineText,
                    hasCameraTarget, cameraArgs, hasChoices, hasEventId, eventNameId, linksToInts, hasHighlightEvent, highlightEvent);
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

        protected virtual void ManageLoadJobSupplierLevel(JobSupplierBitId supplierId)
        {
            if (supplierId == 0)
            {
                return;
            }
            var sceneToLoad = TurnJobSupplierIntoScene(supplierId);
            MGameDirector.GetLevelManager.ActivateScene(sceneToLoad);
        }

        private LevelIndexId TurnJobSupplierIntoScene(JobSupplierBitId supplierId)
        {
            switch (supplierId)
            {
                case JobSupplierBitId.COPY_OF_EDEN:
                    return LevelIndexId.EdenLvl;
                default:
                    return 0;
            }
        }
        
        protected virtual void FinishIntroductionText()
        {
            throw new NotImplementedException();
        }

        protected virtual IEnumerator StartIntroductionReading()
        {
            return null;
        }
        protected virtual IEnumerator StartModularDialoguePreparations()
        {
            return null;
        }
        protected virtual void ReleaseFromInitialDialogueAndStartClock()
        {
            Debug.Log("[DayLevelSceneManagement.ReleaseFromDialogueStateAndStartClock] Start Day Game");
            MGameDirector.ReleaseFromDialogueStateToGame();
            MGameDirector.GetUIController.ReturnToBaseGamePlayCanvasState();
            MGameDirector.GetClockInDayManagement.SetClockAtDaytime(PartOfDay.EarlyMorning);
            MGameDirector.GetClockInDayManagement.PlayPauseClock(true);
            MGameDirector.GetDialogueOperator.OnDialogueCompleted -= ReleaseFromInitialDialogueAndStartClock;
        }
    }
}