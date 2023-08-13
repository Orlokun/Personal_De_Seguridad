namespace GameDirection
{
    public interface IGameDirector : IHighLvlGameStateManager, IPlayerProfileManager
    {
        public void StartNewGame();
    }
}