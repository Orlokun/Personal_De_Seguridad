using UnityEngine;
namespace InputManagement
{
    public interface IGeneralGameStateManager
    {
        InputGameState CurrentInputGameState { get; }
        InputGameState LastInputGameState { get; }
        void SetGamePlayState(InputGameState newGameState);
        event GeneralInputStateManager.GameStateChangeHandler OnGameStateChange;
    }

    public class GeneralInputStateManager : IGeneralGameStateManager
    {
        private static GeneralInputStateManager _mInstance;
        public static GeneralInputStateManager Instance
        {
            get
            {
                if (_mInstance == null)
                {
                    _mInstance = new GeneralInputStateManager();
                }
                return _mInstance;
            }
        }
        
        public InputGameState CurrentInputGameState
        {
            get;
            private set;
        }
        public InputGameState LastInputGameState
        {
            get;
            private set;
        }
        
        public void SetGamePlayState(InputGameState newGameState)
        {
            if (CurrentInputGameState == newGameState)
            {
                return;
            }
            if (newGameState != InputGameState.Pause)
            {
                LastInputGameState = newGameState;
            }
            OnGameStateChange?.Invoke(newGameState);
            CurrentInputGameState = newGameState;
            
            var pauseTimeScale = CurrentInputGameState == InputGameState.Pause;
            Time.timeScale = pauseTimeScale ? 0 : 1;
        }
        
        public delegate void GameStateChangeHandler(InputGameState newGameState);
        public event GameStateChangeHandler OnGameStateChange;
    }
}