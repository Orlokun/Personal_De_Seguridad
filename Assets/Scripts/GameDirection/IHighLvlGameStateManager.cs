namespace GameDirection
{
    public interface IHighLvlGameStateManager
    {
        public HighLevelGameStates GetCurrentHighLvlGameState { get; }
        public void ChangeHighLvlGameState(HighLevelGameStates newState);
    }
}