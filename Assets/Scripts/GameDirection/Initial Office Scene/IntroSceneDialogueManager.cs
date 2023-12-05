using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using DialogueSystem;
using DialogueSystem.Interfaces;
using DialogueSystem.Units;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement.LevelManagement;
using InputManagement;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Utils;

namespace GameDirection.Initial_Office_Scene
{
    public interface ISceneManager
    {
        
    }
    public class IntroSceneDialogueManager : MonoBehaviour, ISceneManager, IInitializeWithArg1<IGameDirector>
    {
        private DialogueObjectsFromData _introDialogues;
        public delegate void FinishCurrentDialogue();
        public event FinishCurrentDialogue OnFinishCurrentDialogue;
        private IGameDirector _mGameDirector;
        private Dictionary<int, IDialogueObject> _introDialoguesDict = new Dictionary<int, IDialogueObject>();
        private bool _isInitialized;

        private int _dialogueIndex;

        public bool IsInitialized => _isInitialized;
        public void Initialize(IGameDirector injectionClass1)
        {
            if (IsInitialized)
            {
                return;
            }
            if (injectionClass1 == null)
            {
                Debug.Log("[IntroSceneManager.InitializeWithArg] Injection must not be null");
                return;
            }
            _mGameDirector = injectionClass1;
            LoadInitDialoguesFromJson();
            _isInitialized = true;
        }
        private void LoadInitDialoguesFromJson()
        {
            StartCoroutine(GetIntroDialogueData(DataSheetUrls.IntroDialoguesGameData));
        }
        private IEnumerator GetIntroDialogueData(string url)
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

            _introDialogues = JsonConvert.DeserializeObject<DialogueObjectsFromData>(sourceJson);
            Debug.Log($"Finished parsing Dialogue Data null?: {_introDialogues == null}");
            _introDialoguesDict = new Dictionary<int, IDialogueObject>();
            var lastDialogueIndex = 0;
            IDialogueObject currentDialogueObject;
            if (_introDialogues == null)
            {
                return;
            }
            
            for (var i = 1; i < _introDialogues.values.Count;i++)
            {
                var isDialogueNodeIndex = int.TryParse(_introDialogues.values[i][0], out var currentDialogueObjectIndex);
                if (!isDialogueNodeIndex)
                {
                    Debug.LogWarning($"[JobSupplierObject.LoadDialoguesFromJson] Dialogues for Intro must have node Index greater than zero");
                    return;
                }
                if (lastDialogueIndex != currentDialogueObjectIndex || i == 1)
                {
                    lastDialogueIndex = currentDialogueObjectIndex;
                    currentDialogueObject = ScriptableObject.CreateInstance<DialogueObject>();
                    _introDialoguesDict.Add(currentDialogueObjectIndex, currentDialogueObject);
                }
                
                var hasDialogueNodeId = int.TryParse(_introDialogues.values[i][1], out var dialogueLineId);
                if (!hasDialogueNodeId)
                {
                    Debug.LogWarning($"[IntroSceneManagement.LoadFromJson] Dialogues for Intro must have dialoge node id");
                    return;
                }
                var isSpeakerId = int.TryParse(_introDialogues.values[i][2], out var speakerId);
                if (speakerId == 0 || !isSpeakerId)
                {
                    Debug.LogWarning($"[IntroSceneManagement.LoadFromJson] Dialogues for Intro must have speaker Index greater than zero");
                    //return;
                }

                var dialogueLineText = _introDialogues.values[i][3];
                var cameraTargetName = _introDialogues.values[i][4];
                var hasCameraTarget = cameraTargetName != "0";
                var eventNameId = _introDialogues.values[i][5];
                var hasEventId = eventNameId != "0";
                
                var linksToString = _introDialogues.values[i][6].Split(',');
                var linksToInts = DialogueProcessor.ProcessLinksStrings(linksToString);
                var linksToFinish = linksToInts[0] == 0;
                var hasChoices = linksToInts.Length > 1;

                var dialogueNode = new DialogueNodeData(currentDialogueObjectIndex, dialogueLineId, speakerId, dialogueLineText,
                    hasCameraTarget, cameraTargetName, hasChoices, hasEventId, eventNameId, linksToInts);
                _introDialoguesDict[currentDialogueObjectIndex].DialogueNodes.Add(dialogueNode);
            }
        }
        
        #region Introduction Region
        public IEnumerator PrepareIntroductionReading()
        {
            _mGameDirector.ChangeHighLvlGameState(HighLevelGameStates.InCutScene);
            _mGameDirector.GetInputStateManager.SetGamePlayState(InputGameState.InDialogue);
            
            _mGameDirector.GetSoundDirector.PlayAmbientMusic();
            _mGameDirector.GetUIController.DeactivateAllObjects();
            yield return new WaitForSeconds(2f);
            _mGameDirector.GetUIController.ToggleBackground(true);
            _mGameDirector.GetGeneralBackgroundFader.GeneralCurtainDisappear();
            _mGameDirector.GetDialogueOperator.OnDialogueCompleted += FinishIntroductionText;
            StartCoroutine(StartIntroductionReading());
        }
        private IEnumerator StartIntroductionReading()
        {
            yield return new WaitForSeconds(2f);
            _mGameDirector.GetDialogueOperator.StartNewDialogue(_introDialoguesDict[_dialogueIndex]);
            _dialogueIndex++;
        }
        private void FinishIntroductionText()
        {
            _mGameDirector.GetGeneralBackgroundFader.GeneralCurtainAppear();
            StartCoroutine(FinishIntroductionReading());
        }
        private IEnumerator FinishIntroductionReading()
        {
            yield return new WaitForSeconds(1f);
            _mGameDirector.GetLevelManager.LoadAdditiveLevel(LevelIndexId.OfficeLvl);
            _mGameDirector.GetLevelManager.UnloadScene(LevelIndexId.InitScene);
            yield return new WaitForSeconds(2f);
            _mGameDirector.GetUIController.ToggleBackground(false);
            _mGameDirector.GetGeneralBackgroundFader.GeneralCurtainDisappear();
            _mGameDirector.GetDialogueOperator.OnDialogueCompleted -= FinishIntroductionText;
            StartCoroutine(SecondDialogue());
        }

        private IEnumerator SecondDialogue()
        {
            _mGameDirector.GetDialogueOperator.OnDialogueCompleted += ReleaseFromDialogueStateAndStartClock;
            yield return new WaitForSeconds(2.5f);
            _mGameDirector.ChangeHighLvlGameState(HighLevelGameStates.OfficeMidScene);
            _mGameDirector.GetDialogueOperator.StartNewDialogue(_introDialoguesDict[_dialogueIndex]);
            _dialogueIndex++;         
            OnFinishCurrentDialogue?.Invoke();
        }

        private void ReleaseFromDialogueStateAndStartClock()
        {
            _mGameDirector.ReleaseFromDialogueStateToGame();
            _mGameDirector.GetUIController.ReturnToBaseGamePlayCanvasState();
            _mGameDirector.GetClockInDayManagement.SetClockAtDaytime(PartOfDay.EarlyMorning);
            _mGameDirector.GetClockInDayManagement.PlayPauseClock(true);
            _mGameDirector.GetDialogueOperator.OnDialogueCompleted -= ReleaseFromDialogueStateAndStartClock;
            StartCoroutine(PrepareFirstFeedback());
        }

        private IEnumerator PrepareFirstFeedback()
        {
            yield return new WaitForSeconds(6);
            FeedbackManager.Instance.StartReadingFeedback(GeneralFeedbackId.QE_MOVEMENT);
        }
        

        #endregion

    }
}