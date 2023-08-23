using CameraManagement;
using DataUnits.GameCatalogues;
using DialogueSystem.Interfaces;
using GameManagement;
using GamePlayManagement;
using GamePlayManagement.LevelManagement;
using InputManagement;
using UI;
namespace GameDirection
{
    public interface IGameDirector : IHighLvlGameStateManager, IPlayerProfileManager
    {
        public void StartNewGame();
        public ILevelManager GetLevelManager { get; }
        public IUIController GetUIController { get; }
        public IGeneralUIFader GetGeneralFader { get; }
        public IGameCameraManager GetGameCameraManager { get; }
        public IGeneralGameStateManager GetGameStateManager { get; }
        public IDialogueOperator GetDialogueOperator { get; }
        public ISoundDirector GetSoundDirector { get; }
        public IBaseItemDataCatalogue GetBaseItemDataCatalogue { get; }
    }
}