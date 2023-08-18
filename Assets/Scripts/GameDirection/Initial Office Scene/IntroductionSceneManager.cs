using System.Collections;
using DialogueSystem.Interfaces;
using InputManagement;
using UnityEngine;
using Utils;

namespace GameDirection.Initial_Office_Scene
{
    public interface ISceneManager
    {
        
    }
    public class IntroductionSceneManager : MonoBehaviour, ISceneManager, IInitializeWithArg2<IGameDirector, IDialogueObject>
    {
        private IGameDirector _mGameDirector;
        private IDialogueObject _introDialogue;
        private bool _isInitialized;

        public bool IsInitialized => _isInitialized;

        public void Initialize(IGameDirector injectionClass1, IDialogueObject injectionClass2)
        {
            if (IsInitialized)
            {
                return;
            }
            _mGameDirector = injectionClass1;
            _introDialogue = injectionClass2;
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
            _mGameDirector.GetDialogueOperator.StartNewDialogue(_introDialogue);
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
            yield return new WaitForSeconds(4);
            _mGameDirector.ChangeHighLvlGameState(HighLevelGameStates.OfficeMidScene);
            GeneralGamePlayStateManager.Instance.SetGamePlayState(InputGameState.InGame);
        }

        #endregion
    }
}