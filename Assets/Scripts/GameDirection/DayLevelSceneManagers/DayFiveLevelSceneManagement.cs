using System.Collections;
using GameDirection.TimeOfDayManagement;
using InputManagement;
using UnityEngine;

namespace GameDirection.DayLevelSceneManagers
{
    public class DayFiveLevelSceneManagement : DayLevelSceneManagement    
    {
        public override IEnumerator StartDayManagement()
        {
            MGameDirector.ChangeHighLvlGameState(HighLevelGameStates.InCutScene);
            MGameDirector.GetGameInputManager.SetGamePlayState(InputGameState.InDialogue);
            MGameDirector.GetNarrativeNewsDirector.LoadDayNews(DayBitId.Day_01);

            MGameDirector.GetSoundDirector.PlayAmbientSound();
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
            MGameDirector.GetDialogueOperator.StartNewDialogue(DayBaseDialogues[IntroDialogueIndex]);
        }
        protected override void FinishIntroductionText()
        {
            MGameDirector.GetGeneralBackgroundFader.GeneralCurtainAppear();
            MGameDirector.ActCoroutine(StartModularDialoguePreparations());
        }
    }
}