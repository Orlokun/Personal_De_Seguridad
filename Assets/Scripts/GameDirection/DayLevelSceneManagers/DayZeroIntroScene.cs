using System.Collections;
using InputManagement;
using UnityEngine;

namespace GameDirection.DayLevelSceneManagers
{
    public class DayZeroIntroScene : IIntroSceneOperator
    {
        public bool IsInitialized => mInitialized;
        private bool mInitialized;
        private IGameDirector _mGameDirector;
        private IIntroSceneInGameManager _mSceneManager;

        
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
            //_mGameDirector.GetSoundDirector.StartIntroSceneMusic();
            /*
            _mGameDirector.GetActiveGameProfile.GetComplianceManager.UpdateComplianceDay(_mDayId);
            _mGameDirector.GetDialogueOperator.OnDialogueCompleted += FinishIntroductionText;
            _mGameDirector.ActCoroutine(StartIntroductionReading());
            */
        }
    }
}