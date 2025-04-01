namespace InputManagement
{
    public interface IGeneralGameInputManager
    {
        InputGameState CurrentInputGameState { get; }
        InputGameState LastInputGameState { get; }
        void SetGamePlayState(InputGameState newGameState);
        event IGeneralGameGameInputManager.GameStateChangeHandler OnGameStateChange;
    }
}