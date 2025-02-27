using System.Collections;
using GameDirection.TimeOfDayManagement;
using InputManagement;
using UnityEngine;

namespace GameDirection.DayLevelSceneManagers
{
    public class DayFourLevelSceneManagement : DayLevelSceneManagement    
    {
        private const DayBitId _mDayId = DayBitId.Day_04;

        public override IEnumerator StartDayManagement()
        {
            ModularDialogue = MGameDirector.GetModularDialogueManager.CreateInitialDayIntro(MGameDirector.GetActiveGameProfile);
            MGameDirector.ChangeHighLvlGameState(HighLevelGameStates.InCutScene);
            MGameDirector.GetGameInputManager.SetGamePlayState(InputGameState.InDialogue);
            MGameDirector.GetNarrativeNewsDirector.LoadDayNews(DayBitId.Day_04);
            MGameDirector.SubscribeCurrentWorkDayToCustomerManagement();

            MGameDirector.GetSoundDirector.PlayAmbientSound();
            MGameDirector.GetUIController.DeactivateAllObjects();
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
        }
        protected override void FinishIntroductionText()
        {
            MGameDirector.GetGeneralBackgroundFader.GeneralCurtainAppear();
            MGameDirector.ActCoroutine(StartModularDialoguePreparations());
        }
        protected override IEnumerator StartModularDialoguePreparations()
        {
            yield return new WaitForSeconds(1f);
            var currentEmployer = MGameDirector.GetActiveGameProfile.GetActiveJobsModule().CurrentEmployer;
            ManageLoadJobSupplierLevel(currentEmployer);
            yield return new WaitForSeconds(2f);
            MGameDirector.GetUIController.ToggleBackground(false);
            MGameDirector.GetGeneralBackgroundFader.GeneralCurtainDisappear();
            MGameDirector.GetDialogueOperator.OnDialogueCompleted -= FinishIntroductionText;
            MGameDirector.ActCoroutine(ReadModularDialogue());
        }
        private IEnumerator ReadModularDialogue()
        {
            MGameDirector.GetDialogueOperator.OnDialogueCompleted += ReleaseFromInitialDialogueAndStartClock;
            yield return new WaitForSeconds(2.5f);
            MGameDirector.ChangeHighLvlGameState(HighLevelGameStates.OfficeMidScene);
            MGameDirector.GetDialogueOperator.StartNewDialogue(ModularDialogue);
            OnFinishCurrentDialogueEvent();
        }

        protected override void ReleaseFromInitialDialogueAndStartClock()
        {
            Debug.Log("[DayTwoLevelSceneManagement.ReleaseFromDialogueStateAndStartClock] Start");
            base.ReleaseFromInitialDialogueAndStartClock();
            MGameDirector.ActCoroutine(PrepareFirstFeedback());
            Debug.Log("[DayTwoLevelSceneManagement.ReleaseFromDialogueStateAndStartClock] Done");
        }
        private IEnumerator PrepareFirstFeedback()
        {
            yield return new WaitForSeconds(6);
            //FeedbackManager.Instance.StartReadingFeedback(GeneralFeedbackId.STOREVIEW);
        }
    }
}