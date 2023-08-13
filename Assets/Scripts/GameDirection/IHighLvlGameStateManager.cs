namespace GameDirection
{
    public interface IHighLvlGameStateManager
    {
        public HighLevelGameStates GetCurrentState { get; }
        public void ChangeHighLvlGameState(HighLevelGameStates newState);
    }
}