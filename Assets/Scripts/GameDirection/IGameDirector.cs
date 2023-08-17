using CameraManagement;
using DialogueSystem.Interfaces;
using GameManagement;
using GamePlayManagement.LevelManagement;
using InputManagement;
using UI;
namespace GameDirection
{
    public interface IGameDirector : IHighLvlGameStateManager, IPlayerProfileManager
    {
        public void StartNewGame();
        public IPlayerGameProfile GetActiveGameProfile { get; }
        public ILevelManager GetLevelManager { get; }
        public IUIController GetUIController { get; }
        public IGeneralUIFader GetGeneralFader { get; }
        public IGameCameraManager GetGameCameraManager { get; }
        public IGeneralGameStateManager GetGameStateManager { get; }
        public IDialogueOperator GetDialogueOperator { get; }
        public ISoundDirector GetSoundDirector { get; }
    }
}