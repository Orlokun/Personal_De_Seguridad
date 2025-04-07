using System.Collections;
using DialogueSystem.Units;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement.LevelManagement;
using InputManagement;
using UnityEditor;
using UnityEngine;

namespace GameDirection.DayLevelSceneManagers
{
    public class DayOneLevelSceneManagement : DayLevelSceneManagement
    {
        private const DayBitId _mDayId = DayBitId.Day_01;
        #region Introduction Region
        public override IEnumerator StartDayManagement()
        {
            MGameDirector.ChangeHighLvlGameState(HighLevelGameStates.InCutScene);
            MGameDirector.GetGameInputManager.SetGamePlayState(InputGameState.InDialogue);
            MGameDirector.GetNarrativeNewsDirector.LoadDayNews(_mDayId);
            MGameDirector.SubscribeCurrentWorkDayToCustomerManagement();
            
            MGameDirector.GetSoundDirector.PlayRegularDayAmbientSound();
            MGameDirector.GetUIController.DeactivateAllObjects();
            MGameDirector.GetUIController.ToggleBackground(true);
            yield return new WaitForSeconds(2f);
            MGameDirector.GetActiveGameProfile.GetComplianceManager.UpdateComplianceDay(_mDayId);
            MGameDirector.GetUIController.ToggleBackground(true);
            MGameDirector.GetGeneralBackgroundFader.GeneralCurtainDisappear();
            MGameDirector.GetDialogueOperator.OnDialogueCompleted += FinishIntroductionText;
            MGameDirector.ActCoroutine(StartIntroductionReading());
        }
        protected override IEnumerator StartIntroductionReading()
        {
            yield return new WaitForSeconds(2f);
            MGameDirector.GetDialogueOperator.StartNewDialogue(DayBaseDialogues[IntroDialogueIndex]);
            IntroDialogueIndex++;
        }
        protected override void FinishIntroductionText()
        {
            MGameDirector.GetGeneralBackgroundFader.GeneralCurtainAppear();
            MGameDirector.ActCoroutine(StartModularDialoguePreparations());
        }
        
        protected override IEnumerator StartModularDialoguePreparations()
        {
            yield return new WaitForSeconds(1f);
            MGameDirector.GetLevelManager.ActivateScene(LevelIndexId.OfficeLvl);
            MGameDirector.GetLevelManager.DeactivateScene(LevelIndexId.InitScene);
            yield return new WaitForSeconds(2f);
            MGameDirector.GetGameCameraManager.LoadMainOfficeCameras();
            MGameDirector.GetUIController.ToggleBackground(false);
            MGameDirector.GetGeneralBackgroundFader.GeneralCurtainDisappear();
            MGameDirector.GetDialogueOperator.OnDialogueCompleted -= FinishIntroductionText;
            MGameDirector.ActCoroutine(ReadModularDialogue());
        }

        private IEnumerator ReadModularDialogue()
        {
            MGameDirector.GetDialogueOperator.OnDialogueCompleted += ReleaseFromInitialDialogueAndStartClock;
            //MGameDirector.GetCustomerInstantiationManager.LoadCustomerLevelStartTransforms();
            yield return new WaitForSeconds(2.5f);
            MGameDirector.ChangeHighLvlGameState(HighLevelGameStates.OfficeMidScene);
            MGameDirector.GetDialogueOperator.StartNewDialogue(DayBaseDialogues[IntroDialogueIndex]);
            //OnFinishCurrentDialogueEvent();
        }
        protected override void ReleaseFromInitialDialogueAndStartClock()
        {
            base.ReleaseFromInitialDialogueAndStartClock();
            MGameDirector.ActCoroutine(PrepareFirstFeedback());
        }

        private IEnumerator PrepareFirstFeedback()
        {
            yield return new WaitForSeconds(6);
            FeedbackManager.Instance.StartReadingFeedback(GeneralFeedbackId.QE_MOVEMENT);
        }
        #endregion

    }
}