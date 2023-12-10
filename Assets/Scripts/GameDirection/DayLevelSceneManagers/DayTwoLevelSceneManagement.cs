using System.Collections;
using GamePlayManagement.LevelManagement;
using InputManagement;
using UnityEngine;

namespace GameDirection.DayLevelSceneManagers
{
    public class DayTwoLevelSceneManagement : DayLevelSceneManagement
    {
        public override IEnumerator StartDayManagement()
        {
            ModularDialogue =
                MGameDirector.GetModularDialogueManager.CreateInitialDayIntro(MGameDirector.GetActiveGameProfile);
            MGameDirector.ChangeHighLvlGameState(HighLevelGameStates.InCutScene);
            MGameDirector.GetInputStateManager.SetGamePlayState(InputGameState.InDialogue);
            
            MGameDirector.GetSoundDirector.PlayAmbientMusic();
            MGameDirector.GetUIController.DeactivateAllObjects();
            yield return new WaitForSeconds(2f);
            MGameDirector.GetUIController.ToggleBackground(true);
            MGameDirector.GetGeneralBackgroundFader.GeneralCurtainDisappear();
            MGameDirector.GetDialogueOperator.OnDialogueCompleted += FinishIntroductionText;
            MGameDirector.ActCoroutine(StartIntroductionReading());
        }

        protected override IEnumerator StartIntroductionReading()
        {
            yield return new WaitForSeconds(2f);
            MGameDirector.GetDialogueOperator.StartNewDialogue(DayBaseDialogues[DialogueIndex]);
        }
        protected override void FinishIntroductionText()
        {
            MGameDirector.GetGeneralBackgroundFader.GeneralCurtainAppear();
            MGameDirector.ActCoroutine(StartModularDialogueReading());
        }
        protected override IEnumerator StartModularDialogueReading()
        {
            yield return new WaitForSeconds(1f);
            var currentEmployer = MGameDirector.GetActiveGameProfile.GetActiveJobsModule().CurrentEmployer;
            ManageLoadJobSupplierLevel(currentEmployer);
            yield return new WaitForSeconds(2f);
            MGameDirector.GetUIController.ToggleBackground(false);
            MGameDirector.GetGeneralBackgroundFader.GeneralCurtainDisappear();
            MGameDirector.GetDialogueOperator.OnDialogueCompleted -= FinishIntroductionText;
            MGameDirector.ActCoroutine(SecondDialogue());
        }
        private IEnumerator SecondDialogue()
        {
            MGameDirector.GetDialogueOperator.OnDialogueCompleted += ReleaseFromDialogueStateAndStartClock;
            yield return new WaitForSeconds(2.5f);
            MGameDirector.ChangeHighLvlGameState(HighLevelGameStates.OfficeMidScene);
            MGameDirector.GetDialogueOperator.StartNewDialogue(ModularDialogue);
            OnFinishCurrentDialogueEvent();
        }
    }
}