using System.Collections;
using System.Collections.Generic;
using CameraManagement;
using DataUnits.ItemScriptableObjects;
using DialogueSystem;
using DialogueSystem.Interfaces;
using DialogueSystem.Units;
using GamePlayManagement.ItemPlacement.PlacementManagement;
using InputManagement;
using Newtonsoft.Json;
using UI.PopUpManager;
using UI.PopUpManager.InfoPanelPopUp;
using UnityEngine;
using UnityEngine.Networking;
using Utils;

namespace GameDirection.DayLevelSceneManagers
{
    public enum IntroSceneTimerStates
    {
        AwaitGuard,
        AwaitCamera
    }
    public class DayZeroIntroScene : IIntroSceneOperator
    {
        public bool IsInitialized => mInitialized;
        private bool mInitialized;
        private IGameDirector _mGameDirector;
        private IIntroSceneInGameManager _mSceneManager;
        private IntroSceneTimerStates _mIntroSceneState;
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

            _mIntroSceneState = IntroSceneTimerStates.AwaitGuard;
            _mGameDirector = injectionClass;
            _mSceneManager = injectionClass2;
            _mGameDirector.ActCoroutine(GetIntroSceneDialogueData());
            mInitialized = true;
            
            _mGameDirector.GetGameCameraManager.SetLevelCamerasParent(_mSceneManager.GetCamerasInLevelParent);
            _mGameDirector.GetGameCameraManager.SetOfficeCamerasParent(_mSceneManager.GetCamerasInOfficeParent);
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
            ActivatePlacementManager();
            
            FloorPlacementManager.Instance.OnItemPlaced += OnGuardPlaced;
            //_mGameDirector.GetSoundDirector.StartIntroSceneMusic();
            /*
            _mGameDirector.GetActiveGameProfile.GetComplianceManager.UpdateComplianceDay(_mDayId);
            _mGameDirector.GetDialogueOperator.OnDialogueCompleted += FinishIntroductionText;
            _mGameDirector.ActCoroutine(StartIntroductionReading());
            */
        }
        
        private void ActivatePlacementManager()
        {
            var placementManager = _mGameDirector.GetPlacementManager();
            if (placementManager == null)
            {
                return;
            }
            placementManager.SetActive(true);
        }


        private void OnCameraPlacementFailed()
        {
            
        }

        private void OnGuardPlacementFailed()
        {
            Debug.LogWarning("Guard was NOT placed!");
            _mGameDirector.GetDialogueOperator.StartNewDialogue(_mIntroSceneDialogues[3]);

        }
        
        private void OnGuardPlaced(IItemObject itemPlaced)
        {
            Debug.LogWarning("Guard was placed!");
            StopTimer();
            _mIntroSceneState = IntroSceneTimerStates.AwaitCamera;
            _mGameDirector.GetDialogueOperator.StartNewDialogue(_mIntroSceneDialogues[1]);
            _mGameDirector.GetDialogueOperator.OnDialogueCompleted += GuardDiesDIalogue;
        }

        private void GuardDiesDIalogue()
        {
            
        }
        private IEnumerator WaitAndAccessCharacterFirstZone()
        {
            Debug.Log("[Waiting for character to come]");


            //Maybe play special sounds
            yield return new WaitForSeconds(3f);
            Debug.Log("[Character instantiated]");
            Debug.Log("[Waiting for character to shoot guard]");
            yield return new WaitForSeconds(2f);
            _mGameDirector.GetDialogueOperator.StartNewDialogue(_mIntroSceneDialogues[2]);
            _mGameDirector.GetDialogueOperator.OnDialogueCompleted += OnCameraPlacementFailed;
            _mGameDirector.GetSoundDirector.StartIntroSceneAlarmSound();
            _mSceneManager.ToggleCeoCameras(false);
            StartTimer();
            
        }
        private void StopTimer()
        {
            var timerPopUp = (IGamePlayActionTimer)PopUpOperator.Instance.GetActivePopUp(BitPopUpId.ACTION_TIMER_POPUP);
            if (timerPopUp == null)
            {
                return;
            }   
            timerPopUp.OnTimerEnd -= TimerEnd;
            PopUpOperator.Instance.RemovePopUp(BitPopUpId.ACTION_TIMER_POPUP);
        }

        public void StartTimer()
        {
            var timerPopUp = (IGamePlayActionTimer)PopUpOperator.Instance.ActivatePopUp(BitPopUpId.ACTION_TIMER_POPUP);
            
            timerPopUp.StartTimer(45);
            timerPopUp.OnTimerEnd += TimerEnd;
        }

        private void TimerEnd()
        {
            switch (_mIntroSceneState)
            {
                case IntroSceneTimerStates.AwaitGuard:
                    //Launch Failure Dialogue
                    var timerPopUp = (IGamePlayActionTimer)PopUpOperator.Instance.GetActivePopUp(BitPopUpId.ACTION_TIMER_POPUP);
                    timerPopUp.OnTimerEnd -= TimerEnd;
                    PopUpOperator.Instance.RemovePopUp(BitPopUpId.ACTION_TIMER_POPUP);
                    _mIntroSceneState = IntroSceneTimerStates.AwaitCamera;
                    OnGuardPlacementFailed();
                    break;
                case IntroSceneTimerStates.AwaitCamera:
                    //Activate FailureDialogueCamera
                    var cameraTimerPopUp = (IGamePlayActionTimer)PopUpOperator.Instance.GetActivePopUp(BitPopUpId.ACTION_TIMER_POPUP);
                    cameraTimerPopUp.OnTimerEnd -= TimerEnd;
                    PopUpOperator.Instance.RemovePopUp(BitPopUpId.ACTION_TIMER_POPUP);
                    OnCameraPlacementFailed();
                    break;
            }
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