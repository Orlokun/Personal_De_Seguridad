using System.Collections;
using CameraManagement;
using DataUnits.GameCatalogues;
using DialogueSystem;
using DialogueSystem.Interfaces;
using GameDirection.TimeOfDayManagement;
using GameManagement;
using GamePlayManagement;
using GamePlayManagement.BitDescriptions.Suppliers;
using GamePlayManagement.LevelManagement;
using InputManagement;
using UI;
namespace GameDirection
{
    public interface IGameDirector : IHighLvlGameStateManager, IPlayerProfileManager
    {
        public void StartNewGame();
        public ILevelManager GetLevelManager { get; }
        public IClockManagement GetClockInDayManagement { get; }
        public IUIController GetUIController { get; }
        public IGeneralUIFader GetGeneralBackgroundFader { get; }
        public IGameCameraManager GetGameCameraManager { get; }
        public IGeneralInputStateManager GetInputStateManager { get; }
        public IDialogueOperator GetDialogueOperator { get; }
        public ISoundDirector GetSoundDirector { get; }
        public IRentValuesCatalogue GetRentCatalogueData { get; }
        public IItemsDataController GetItemsDataController { get; }
        public IFeedbackManager GetFeedbackManager { get; }
        public IModularDialogueDataController GetModularDialogueManager { get; }
        public void ReleaseFromDialogueStateToGame();
        public void FinishWorkday();
        public void ActCoroutine(IEnumerator coroutine);
        public void ManageNewJobHiredEvent(JobSupplierBitId newJobSupplier);
    }
}