using UnityEngine;
namespace InputManagement
{
    public class IGeneralGameGameInputManager : IGeneralGameInputManager
    {
        #region SingletonInstance
        private static IGeneralGameGameInputManager mInstance;
        public static IGeneralGameInputManager Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new IGeneralGameGameInputManager();
                }
                return mInstance;
            }
        }
        #endregion

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