using System.Collections;
using System.Collections.Generic;
using DialogueSystem.Interfaces;
using InputManagement;
using UnityEngine;
using Utils;

namespace GameDirection.Initial_Office_Scene
{
    public interface ISceneManager
    {
        
    }
    public class IntroSceneManager : MonoBehaviour, ISceneManager, IInitializeWithArg2<IGameDirector, List<IDialogueObject>>
    {
        private IGameDirector _mGameDirector;
        private List<IDialogueObject> _introDialogues;
        private bool _isInitialized;

        private int _dialogueIndex = 0;

        public bool IsInitialized => _isInitialized;

        public void Initialize(IGameDirector injectionClass1, List<IDialogueObject> injectionClass2)
        {
            if (IsInitialized)
            {
                return;
            }
            if (injectionClass2 == null || injectionClass2.Count == 0)
            {
                Debug.Log("Initial Dialogues List is empty");
                return;
            }
            
            _mGameDirector = injectionClass1;
            _introDialogues = injectionClass2;
            _isInitialized = true;
        }
        
        #region Introduction Region
        public IEnumerator PrepareIntroductionReading()
        {
            yield return new WaitForSeconds(2f);
            _mGameDirector.GetSoundDirector.PlayAmbientMusic();
            _mGameDirector.GetUIController.DeactivateAllObjects();
            _mGameDirector.GetUIController.ToggleBackground(true);
            _mGameDirector.GetGeneralFader.GeneralCameraFadeIn();
            _mGameDirector.GetDialogueOperator.OnDialogueCompleted += FinishIntroductionText;
            StartCoroutine(StartIntroductionReading());
        }
        private IEnumerator StartIntroductionReading()
        {
            yield return new WaitForSeconds(2f);
            _mGameDirector.GetDialogueOperator.StartNewDialogue(_introDialogues[_dialogueIndex]);
            _dialogueIndex++;
        }
        private void FinishIntroductionText()
        {
            _mGameDirector.GetGeneralFader.GeneralCameraFadeOut();
            StartCoroutine(FinishIntroductionReading());
        }
        private IEnumerator FinishIntroductionReading()
        {
            yield return new WaitForSeconds(2f);
            _mGameDirector.GetLevelManager.LoadOfficeLevel();
            _mGameDirector.GetUIController.ToggleBackground(false);
            _mGameDirector.GetGeneralFader.GeneralCameraFadeIn();
            _mGameDirector.GetDialogueOperator.OnDialogueCompleted -= FinishIntroductionText;
            StartCoroutine(SecondDialogue());

        }

        private IEnumerator SecondDialogue()
        {
            yield return new WaitForSeconds(2.5f);
            _mGameDirector.ChangeHighLvlGameState(HighLevelGameStates.OfficeMidScene);
            GeneralGamePlayStateManager.Instance.SetGamePlayState(InputGameState.InGame);
            _mGameDirector.GetDialogueOperator.StartNewDialogue(_introDialogues[_dialogueIndex]);
            _dialogueIndex++;
        }

        #endregion
    }
}