﻿using System.Collections;
using System.Collections.Generic;
using CameraManagement;
using DataUnits.ItemScriptableObjects;
using DialogueSystem;
using DialogueSystem.Interfaces;
using DialogueSystem.Units;
using GamePlayManagement.BitDescriptions.Suppliers;
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

        #region Init
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
            _mGameDirector.GetSoundDirector.ToggleIntroSceneAlarmSound(true);
            _mGameDirector.GetUIController.DeactivateAllObjects();
            yield return new WaitForSeconds(1.5f);
            _mGameDirector.GetSoundDirector.ToggleWarZoneSound(true);
            yield return new WaitForSeconds(9f);
            _mGameDirector.GetSoundDirector.StartIntroSceneMusic();
            yield return new WaitForSeconds(7f);
            _mSceneManager.ToggleIntroSceneLevelObjects(true);
            _mSceneManager.ToggleBeacon(true);
            _mSceneManager.ToggleIntroSceneLights(false);
            yield return new WaitForSeconds(2f);
            _mGameDirector.GetGeneralBackgroundFader.GeneralCurtainDisappear();
            yield return new WaitForSeconds(1.5f);
            _mSceneManager.ToggleCeoCameras(true);
            _mGameDirector.GetDialogueOperator.StartNewDialogue(_mIntroSceneDialogues[0]);
            ActivatePlacementManager();
            
            FloorPlacementManager.Instance.OnItemPlaced += OnGuardPlaced;
        }
        #endregion

        #region First Section (Guard Zone)

        private void OnGuardPlacementFailed()
        {
            Debug.LogWarning("Guard was NOT placed!");
            FloorPlacementManager.Instance.OnItemPlaced -= OnGuardPlaced;
            _mGameDirector.GetDialogueOperator.StartNewDialogue(_mIntroSceneDialogues[3]);
            CameraPlacementManager.MInstance.OnItemPlaced += OnCameraPlaced;
            _mGameDirector.GetActiveGameProfile.GetInventoryModule().ClearItemFromInventory(BitItemSupplier.D1TV, 1);
            AddItemToInventory(BitItemSupplier.D1TV, 8);
        }
        private void OnGuardPlaced(IItemObject itemPlaced)
        {
            Debug.LogWarning("Guard was placed!");
            StopTimer();
            _mGameDirector.GetActiveGameProfile.GetInventoryModule().ClearItemFromInventory(BitItemSupplier.D1TV, 1);
            FloorPlacementManager.Instance.OnItemPlaced -= OnGuardPlaced;
            _mIntroSceneState = IntroSceneTimerStates.AwaitCamera;
            _mGameDirector.GetDialogueOperator.StartNewDialogue(_mIntroSceneDialogues[1]);
            _mGameDirector.GetDialogueOperator.OnDialogueCompleted += GuardDiesDialogue;
        }
        private void GuardDiesDialogue()
        {
            _mGameDirector.GetDialogueOperator.OnDialogueCompleted -= GuardDiesDialogue;
            _mGameDirector.ActCoroutine(WaitAndAccessCharacterFirstZone());    
        }
        private IEnumerator WaitAndAccessCharacterFirstZone()
        {
            _mSceneManager.ToggleGuardPlacementCameras(true);
            AddItemToInventory(BitItemSupplier.D1TV, 8);
            _mGameDirector.GetSoundDirector.ToggleWarZoneSound(false);
            Debug.Log("[Waiting for character to come]");
            //Maybe play special sounds
            yield return new WaitForSeconds(2f);
            _mSceneManager.InstantiateCharacterFirstSection();
            Debug.Log("[Character instantiated]");
            Debug.Log("[Waiting for character to shoot guard]");
            yield return new WaitForSeconds(2f);
            _mSceneManager.ShootGuard();
            yield return new WaitForSeconds(.3f);
            _mGameDirector.GetDialogueOperator.StartNewDialogue(_mIntroSceneDialogues[2]);
            _mSceneManager.ToggleCeoCameras(true);
            yield return new WaitForSeconds(.3f);
            _mSceneManager.ToggleSceneCharacter(false);
            CameraPlacementManager.MInstance.OnItemPlaced += OnCameraPlaced;
        }
        #endregion

        #region Second Section (Camera Zone)
        private void OnCameraPlaced(IItemObject itemPlaced)
        {
            Debug.LogWarning("Camera was placed!");
            StopTimer();
            FloorPlacementManager.Instance.OnItemPlaced -= OnCameraPlaced;
            _mGameDirector.GetActiveGameProfile.GetInventoryModule().ClearItemFromInventory(BitItemSupplier.D1TV, 8);
            _mIntroSceneState = IntroSceneTimerStates.AwaitCamera;
            _mSceneManager.ToggleCameraPlacementCameras(true);
            _mGameDirector.GetDialogueOperator.StartNewDialogue(_mIntroSceneDialogues[4]);
            _mGameDirector.GetDialogueOperator.OnDialogueCompleted += CharacterAppearsOnCamera;
        }
        private void OnCameraPlacementFailed()
        {
            _mGameDirector.GetActiveGameProfile.GetInventoryModule().ClearItemFromInventory(BitItemSupplier.D1TV, 8);
            _mSceneManager.ToggleCeoCameras(true);
            _mGameDirector.GetDialogueOperator.StartNewDialogue(_mIntroSceneDialogues[6]);
            _mGameDirector.GetDialogueOperator.OnDialogueCompleted += LaunchEndOfIntroScene;
        }

        private void CharacterAppearsOnCamera()
        {
            //InstantiateObject and wait for him to reach a spot
            _mGameDirector.GetDialogueOperator.OnDialogueCompleted -= CharacterAppearsOnCamera;
            _mGameDirector.ActCoroutine(WaitForPlayerInCamera());
        }

        private IEnumerator WaitForPlayerInCamera()
        {
            //Instantiate character in scene
            _mSceneManager.InstantiateCharacterSecondSections();
            yield return new WaitForSeconds(2.5f);
            _mGameDirector.GetDialogueOperator.StartNewDialogue(_mIntroSceneDialogues[5]);
            _mGameDirector.GetDialogueOperator.OnDialogueCompleted += LaunchEndOfIntroScene;
        }
        #endregion

        #region Final Section
        private void LaunchEndOfIntroScene()
        {
            _mGameDirector.ActCoroutine(EndSceneSequence());
            _mGameDirector.GetDialogueOperator.OnDialogueCompleted -= LaunchEndOfIntroScene;
        }

        private IEnumerator EndSceneSequence()
        {
            yield return new WaitForSeconds(.5f);
            //Make destroy door sound and visual effect
            _mSceneManager.ToggleSceneCharacter(false);
            yield return new WaitForSeconds(1f);
            //Make shots in door sound
            Debug.Log("Making door noises");
            _mSceneManager.MoveCharacterToHidePosition();
            _mGameDirector.GetGameCameraManager.ActivateCameraWithIndex(GameCameraState.Office, 1);
            _mSceneManager.ToggleCameraPlacementCameras(false);
            yield return new WaitForSeconds(2f);
            _mSceneManager.InstantiateCharacterThirdSections();
            _mSceneManager.WalkingDestinationReached += FinalDialogueSequence;
            _mGameDirector.GetDialogueOperator.OnDialogueCompleted += FinishSequenceAndStartGame;
        }
        private void FinalDialogueSequence()
        {
            _mSceneManager.WalkingDestinationReached -= FinalDialogueSequence;
            _mGameDirector.GetDialogueOperator.StartNewDialogue(_mIntroSceneDialogues[7]);
        }
        private void FinishSequenceAndStartGame()
        {
            _mSceneManager.ShootTargetCommand();
            _mGameDirector.ActCoroutine(ShootingEndOfScene());
        }

        private IEnumerator ShootingEndOfScene()
        {
            _mGameDirector.GetDialogueOperator.OnDialogueCompleted -= FinishSequenceAndStartGame;
            yield return new WaitForSeconds(0.2f);
            _mGameDirector.GetUIController.DeactivateAllObjects();
            _mGameDirector.GetUIController.ToggleBackground(true);
            _mSceneManager.ToggleSceneCharacter(false);
            _mSceneManager.ToggleCompleteScene(false);
            _mGameDirector.GetSoundDirector.ToggleWarZoneSound(false);
            _mGameDirector.GetSoundDirector.ToggleIntroSceneAlarmSound(false);
            _mGameDirector.GetSoundDirector.PlayShotSound();
            _mGameDirector.GetSoundDirector.StartInterviewMusic();
            yield return new WaitForSeconds(5f);
            _mGameDirector.CleanGameProfile();
            _mGameDirector.StartNewDayManagement();
        }
        #endregion

        #region DataManagement
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
        #endregion

        #region Utils

        private void AddItemToInventory(BitItemSupplier itemSupplier, int itemId)
        {
            _mGameDirector.GetActiveGameProfile.GetActiveItemSuppliersModule().UnlockItemInSupplier(itemSupplier, itemId);
            var cameraItemObject = _mGameDirector.GetActiveGameProfile.GetActiveItemSuppliersModule().GetItemObject(itemSupplier, itemId);
            _mGameDirector.GetActiveGameProfile.GetInventoryModule().AddItemToInventory(cameraItemObject, 1);
            _mGameDirector.GetActiveGameProfile.UpdateProfileData();
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

        public void StartTimer()
        {
            var timerPopUp = (IGamePlayActionTimer)PopUpOperator.Instance.ActivatePopUp(BitPopUpId.ACTION_TIMER_POPUP);
            
            timerPopUp.StartTimer(45);
            timerPopUp.OnTimerEnd += TimerEnd;
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
        
        private void TimerEnd()
        {
            switch (_mIntroSceneState)
            {
                case IntroSceneTimerStates.AwaitGuard:
                    //Launch Failure Dialogue
                    StopTimer();
                    _mIntroSceneState = IntroSceneTimerStates.AwaitCamera;
                    OnGuardPlacementFailed();
                    break;
                case IntroSceneTimerStates.AwaitCamera:
                    //Activate FailureDialogueCamera
                    StopTimer();
                    OnCameraPlacementFailed();
                    break;
            }
        }
        #endregion
    }
}