using System.Collections;
using DialogueSystem.Interfaces;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement.BitDescriptions.Suppliers;
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
            MGameDirector.GetDialogueOperator.StartNewDialogue(DayBaseDialogues[DialogueIndex]);
        }
        protected override void FinishIntroductionText()
        {
            MGameDirector.GetGeneralBackgroundFader.GeneralCurtainAppear();
            MGameDirector.ActCoroutine(StartModularDialoguePreparations());
        }
    }

    public class DayOneLevelSceneManagement : DayLevelSceneManagement
    {
        #region Introduction Region
        public override IEnumerator StartDayManagement()
        {
            MGameDirector.ChangeHighLvlGameState(HighLevelGameStates.InCutScene);
            MGameDirector.GetGameInputManager.SetGamePlayState(InputGameState.InDialogue);
            MGameDirector.GetNarrativeNewsDirector.LoadDayNews(DayBitId.Day_01);
            MGameDirector.SubscribeCurrentWorkDayToCustomerManagement();

            
            //TODO: Remove TEST ADDITION
            //MGameDirector.GetDialogueOperator.GetDialogueEventsOperator.LaunchHireEvent(JobSupplierBitId.COPY_OF_EDEN); 
            //End remove
            
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
            MGameDirector.GetDialogueOperator.StartNewDialogue(DayBaseDialogues[DialogueIndex]);
            DialogueIndex++;
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

            /*
            //TODO: Remove EDEN LVL Load. TEST ADDITION
            MGameDirector.GetLevelManager.LoadAdditiveLevel(LevelIndexId.EdenLvl);
            var scene = SceneManager.GetSceneAt((int)LevelIndexId.EdenLvl);
            SceneManager.MoveGameObjectToScene(MGameDirector.GetCustomerInstantiationManager.MyGameObject, scene);
            MGameDirector.GetCustomerInstantiationManager.LoadInstantiationProperties(JobSupplierBitId.COPY_OF_EDEN);
            //End Remove
            */
            
            yield return new WaitForSeconds(2f);
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
            MGameDirector.GetUIController.InitializeBaseInfoCanvas(MGameDirector.GetActiveGameProfile);
            MGameDirector.GetDialogueOperator.StartNewDialogue(DayBaseDialogues[DialogueIndex]);
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