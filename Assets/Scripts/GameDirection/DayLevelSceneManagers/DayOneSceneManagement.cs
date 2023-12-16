using System.Collections;
using DialogueSystem.Interfaces;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement.BitDescriptions.Suppliers;
using GamePlayManagement.LevelManagement;
using InputManagement;
using Players_NPC.NPC_Management.Customer_Management;
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

    public class DayOneLevelSceneManagement : DayLevelSceneManagement
    {

        
        #region Introduction Region
        public override IEnumerator StartDayManagement()
        {
            MGameDirector.ChangeHighLvlGameState(HighLevelGameStates.InCutScene);
            MGameDirector.GetInputStateManager.SetGamePlayState(InputGameState.InDialogue);
            
            //TODO: Remove TEST ADDITION
            MGameDirector.GetDialogueOperator.GetDialogueEventsOperator.LaunchHireEvent(JobSupplierBitId.COPY_OF_EDEN); 

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
            DialogueIndex++;
        }
        protected override void FinishIntroductionText()
        {
            MGameDirector.GetGeneralBackgroundFader.GeneralCurtainAppear();
            MGameDirector.ActCoroutine(StartModularDialogueReading());
        }
        
        protected override IEnumerator StartModularDialogueReading()
        {
            yield return new WaitForSeconds(1f);
            MGameDirector.GetLevelManager.LoadAdditiveLevel(LevelIndexId.OfficeLvl);
            //TODO: Remove EDEN LVL Load. TEST ADDITION
            MGameDirector.GetLevelManager.LoadAdditiveLevel(LevelIndexId.EdenLvl);
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
            MGameDirector.GetDialogueOperator.StartNewDialogue(DayBaseDialogues[DialogueIndex]);
            //OnFinishCurrentDialogueEvent();
        }
        protected override void ReleaseFromDialogueStateAndStartClock()
        {
            base.ReleaseFromDialogueStateAndStartClock();
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