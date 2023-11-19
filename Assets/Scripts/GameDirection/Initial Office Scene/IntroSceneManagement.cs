using System.Collections;
using System.Collections.Generic;
using DialogueSystem.Interfaces;
using DialogueSystem.Units;
using GameDirection.TimeOfDayManagement;
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
    public class DialoguesInSceneDataManager : MonoBehaviour, ISceneManager, IInitializeWithArg1<IGameDirector>
    {
        private DialogueObjectsFromData _introDialogues;
        public delegate void FinishCurrentDialogue();
        public event FinishCurrentDialogue OnFinishCurrentDialogue;
        private IGameDirector _mGameDirector;
        private List<IDialogueObject> _introDialoguesList;
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
            _introDialoguesList = new List<IDialogueObject>();
            var lastDialogueIndex = 0;
            BaseDialogueObject currentDialogueObject;
            if (_introDialogues == null)
            {
                return;
            }
            
            for (var i = 1; i < _introDialogues.values.Count;i++)
            {
                int currentGroupIndex;
                var gotIndex = int.TryParse(_introDialogues.values[i][0], out currentGroupIndex);
                if (!gotIndex)
                {
                    Debug.LogWarning("Dialogue line has no index");
                }
                if (currentGroupIndex != lastDialogueIndex || i == 1)
                {
                    lastDialogueIndex = currentGroupIndex;
                    currentDialogueObject = ScriptableObject.CreateInstance<BaseDialogueObject>();
                    var dialogueLine = _introDialogues.values[i][1];
                    currentDialogueObject.DialogueLines.Add(dialogueLine);
                    _introDialoguesList.Add(currentDialogueObject);
                }
                else
                {
                    var dialogueObject = _introDialoguesList[currentGroupIndex];
                    var dialogueLine = _introDialogues.values[i][1];
                    dialogueObject.DialogueLines.Add(dialogueLine);
                }
            }
            Debug.Log($"[IntroSceneManager.LoadFromJson] Loaded {_introDialoguesList.Count} Dialogues //");
        }
        
        #region Introduction Region
        public IEnumerator PrepareIntroductionReading()
        {
            _mGameDirector.ChangeHighLvlGameState(HighLevelGameStates.InCutScene);
            _mGameDirector.GetGameStateManager.SetGamePlayState(InputGameState.InDialogue);
            
            _mGameDirector.GetSoundDirector.PlayAmbientMusic();
            _mGameDirector.GetUIController.DeactivateAllObjects();
            yield return new WaitForSeconds(2f);
            _mGameDirector.GetUIController.ToggleBackground(true);
            _mGameDirector.GetGeneralBackgroundFader.GeneralFadeIn();
            _mGameDirector.GetDialogueOperator.OnDialogueCompleted += FinishIntroductionText;
            StartCoroutine(StartIntroductionReading());
        }
        private IEnumerator StartIntroductionReading()
        {
            yield return new WaitForSeconds(2f);
            _mGameDirector.GetDialogueOperator.StartNewDialogue(_introDialoguesList[_dialogueIndex]);
            _dialogueIndex++;
        }
        private void FinishIntroductionText()
        {
            _mGameDirector.GetGeneralBackgroundFader.GeneralCameraFadeOut();
            StartCoroutine(FinishIntroductionReading());
        }
        private IEnumerator FinishIntroductionReading()
        {
            yield return new WaitForSeconds(2f);
            _mGameDirector.GetLevelManager.LoadOfficeLevel();
            _mGameDirector.GetUIController.ToggleBackground(false);
            _mGameDirector.GetGeneralBackgroundFader.GeneralFadeIn();
            _mGameDirector.GetDialogueOperator.OnDialogueCompleted -= FinishIntroductionText;
            StartCoroutine(SecondDialogue());
        }

        private IEnumerator SecondDialogue()
        {
            _mGameDirector.GetDialogueOperator.OnDialogueCompleted += ReleaseFromDialogueStateAndStartClock;
            yield return new WaitForSeconds(2.5f);
            _mGameDirector.ChangeHighLvlGameState(HighLevelGameStates.OfficeMidScene);
            _mGameDirector.GetDialogueOperator.StartNewDialogue(_introDialoguesList[_dialogueIndex]);
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