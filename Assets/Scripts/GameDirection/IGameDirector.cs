using System.Collections;
using System.Threading.Tasks;
using CameraManagement;
using DataUnits;
using DataUnits.GameCatalogues;
using DialogueSystem;
using DialogueSystem.Interfaces;
using DialogueSystem.Units;
using GameDirection.NewsManagement;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement.BitDescriptions.Suppliers;
using GamePlayManagement.LevelManagement;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.CustomerInterfaces;
using InputManagement;
using UI;
using UnityEngine;

namespace GameDirection
{
    public interface IGameDirector : IHighLvlGameStateManager, IPlayerProfileManager
    {
        public void StartNewGame();
        public void ContinueGame();
        public void SubscribeCurrentWorkDayToCustomerManagement();
        public ILevelManager GetLevelManager { get; }
        public IClockManagement GetClockInDayManagement { get; }
        public IUIController GetUIController { get; }
        public IGeneralUIFader GetGeneralBackgroundFader { get; }
        public IGameCameraOperator GetGameCameraManager { get; }
        public IGeneralGameInputManager GetGameInputManager { get; }
        public IDialogueOperator GetDialogueOperator { get; }
        public ISoundDirector GetSoundDirector { get; }
        public IRentValuesCatalogue GetRentCatalogueData { get; }
        public IItemsDataController GetItemsDataController { get; }
        public IFeedbackManager GetFeedbackManager { get; }
        public IModularDialogueDataController GetModularDialogueManager { get; }
        public ICustomersInSceneManager GetCustomerInstantiationManager { get; }
        public INewsNarrativeDirector GetNarrativeNewsDirector { get; }
        public void LaunchTutorial();
        public void ReleaseFromDialogueStateToGame();
        public void FinishWorkday();
        public void ActCoroutine(IEnumerator coroutine);
        public void ManageNewJobHiredEvent(JobSupplierBitId newJobSupplier);
        Task ManageNewItemSupplierUnlockedEvent(BitItemSupplier itemsupplier);

        public void BeginNewDayProcess();

        ICallableSupplier GetSpeakerData(DialogueSpeakerId dialogueNodeSpeakerId);
        void PlayerLost(EndingTypes organSale);
        void StartTutorialProcess(int tutorialIndex);
        public GameObject GetPlacementManager();
        void StartIntroTimerEvent();
        public void StartNewDayManagement();

        void CleanGameProfile();
    }
}