using System.Collections;
using DialogueSystem.Interfaces;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement.LevelManagement;
using InputManagement;
using UnityEngine;

namespace GameDirection.DayLevelSceneManagers
{
    public class DayFifteenLevelSceneManagement : DayLevelSceneManagement
    {
        
    }
    public class DayFourteenLevelSceneManagement : DayLevelSceneManagement
    {
        
    }
    public class DayThirteenLevelSceneManagement : DayLevelSceneManagement
    {
        
    }
    public class DayTwelveLevelSceneManagement : DayLevelSceneManagement
    {
        
    }
    public class DayElevenLevelSceneManagement : DayLevelSceneManagement
    {
        
    }
    public class DayTenLevelSceneManagement : DayLevelSceneManagement
    {
        
    }
    public class DayNineLevelSceneManagement : DayLevelSceneManagement
    {
        
    }
    public class DayEightLevelSceneManagement : DayLevelSceneManagement
    {
        
    }
    public class DaySevenLevelSceneManagement : DayLevelSceneManagement
    {
        
    }
    public class DaySixLevelSceneManagement : DayLevelSceneManagement    
    {
        
    }
    public class DayFiveLevelSceneManagement : DayLevelSceneManagement    
    {
        
    }
    public class DayFourLevelSceneManagement : DayLevelSceneManagement    
    {
        
    }
    public class DayThreeLevelSceneManagement : DayLevelSceneManagement    
    {
        
    }
    public class DayTwoLevelSceneManagement : DayLevelSceneManagement
    {
        private IDialogueObject dayIntroObject;
        
        
        public override IEnumerator StartDayManagement()
        {
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

        protected void GetIntroDialogue()
        {
            
        }
        protected override IEnumerator StartIntroductionReading()
        {
            return null;
        }
        protected override void FinishIntroductionText()
        {
            MGameDirector.GetGeneralBackgroundFader.GeneralCurtainAppear();
            MGameDirector.ActCoroutine(FinishIntroductionReading());
        }
        protected override IEnumerator FinishIntroductionReading()
        {
            yield return new WaitForSeconds(1f);
            MGameDirector.GetLevelManager.LoadAdditiveLevel(LevelIndexId.OfficeLvl);
            MGameDirector.GetLevelManager.UnloadScene(LevelIndexId.InitScene);
            yield return new WaitForSeconds(2f);
            MGameDirector.GetUIController.ToggleBackground(false);
            MGameDirector.GetGeneralBackgroundFader.GeneralCurtainDisappear();
            MGameDirector.GetDialogueOperator.OnDialogueCompleted -= FinishIntroductionText;
            //MGameDirector.ActCoroutine(SecondDialogue());
        }
    }

    public class DayOneLevelSceneManagement : DayLevelSceneManagement
    {

        private int _dialogueIndex;
        
        #region Introduction Region
        public override IEnumerator StartDayManagement()
        {
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
            MGameDirector.GetDialogueOperator.StartNewDialogue(DayDialogueObjects[_dialogueIndex]);
            _dialogueIndex++;
        }
        protected override void FinishIntroductionText()
        {
            MGameDirector.GetGeneralBackgroundFader.GeneralCurtainAppear();
            MGameDirector.ActCoroutine(FinishIntroductionReading());
        }
        
        protected override IEnumerator FinishIntroductionReading()
        {
            yield return new WaitForSeconds(1f);
            MGameDirector.GetLevelManager.LoadAdditiveLevel(LevelIndexId.OfficeLvl);
            MGameDirector.GetLevelManager.UnloadScene(LevelIndexId.InitScene);
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
            MGameDirector.GetDialogueOperator.StartNewDialogue(DayDialogueObjects[_dialogueIndex]);
            _dialogueIndex++;
            OnFinishCurrentDialogueEvent();
        }
        

        private void ReleaseFromDialogueStateAndStartClock()
        {
            MGameDirector.ReleaseFromDialogueStateToGame();
            MGameDirector.GetUIController.ReturnToBaseGamePlayCanvasState();
            MGameDirector.GetClockInDayManagement.SetClockAtDaytime(PartOfDay.EarlyMorning);
            MGameDirector.GetClockInDayManagement.PlayPauseClock(true);
            MGameDirector.GetDialogueOperator.OnDialogueCompleted -= ReleaseFromDialogueStateAndStartClock;
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