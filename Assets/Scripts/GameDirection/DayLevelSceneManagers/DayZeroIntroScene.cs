using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using DialogueSystem.Interfaces;
using DialogueSystem.Units;
using InputManagement;
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
        private IIntroSceneInGameManager _mSceneManager;

        protected DialogueObjectsFromData DialoguesBaseDataString;

        private Dictionary<int, IDialogueObject> _mIntroSceneDialogues;      
        public void Initialize(IGameDirector injectionClass, IIntroSceneInGameManager injectionClass2)
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
            _mSceneManager = injectionClass2;
            _mGameDirector.ActCoroutine(GetIntroSceneDialogueData());
            mInitialized = true;
        }

        public IEnumerator StartIntroScene()
        {
            _mGameDirector.ChangeHighLvlGameState(HighLevelGameStates.InCutScene);
            _mGameDirector.GetGameInputManager.SetGamePlayState(InputGameState.InDialogue);
            _mGameDirector.GetSoundDirector.StartIntroSceneAlarmSound();
            _mGameDirector.GetUIController.DeactivateAllObjects();
            yield return new WaitForSeconds(4f);
            _mSceneManager.ToggleIntroSceneLevelObjects(true);
            _mSceneManager.ToggleBeacon(true);
            yield return new WaitForSeconds(2f);
            _mGameDirector.GetGeneralBackgroundFader.GeneralCurtainDisappear();

            yield return new WaitForSeconds(4f);
            _mSceneManager.ToggleCeoCameras(true);
            _mGameDirector.GetDialogueOperator.StartNewDialogue(_mIntroSceneDialogues[0]);
            //_mGameDirector.GetSoundDirector.StartIntroSceneMusic();
            /*
            _mGameDirector.GetActiveGameProfile.GetComplianceManager.UpdateComplianceDay(_mDayId);
            _mGameDirector.GetDialogueOperator.OnDialogueCompleted += FinishIntroductionText;
            _mGameDirector.ActCoroutine(StartIntroductionReading());
            */
        }

        private IEnumerator GetIntroSceneDialogueData()
        {
            var introDialoguesUrl = DataSheetUrls.GetIntroSceneDialogues;    
            var webRequest = UnityWebRequest.Get(introDialoguesUrl);
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
            _mIntroSceneDialogues = new Dictionary<int, IDialogueObject>();
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
                    _mIntroSceneDialogues.Add(currentDialogueObjectIndex, currentDialogueObject);
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
                _mIntroSceneDialogues[currentDialogueObjectIndex].DialogueNodes.Add(dialogueNode);
            }
        }
    }
}