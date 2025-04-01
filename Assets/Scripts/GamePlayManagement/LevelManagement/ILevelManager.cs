namespace GamePlayManagement.LevelManagement
{
    public interface ILevelManager
    {
        public void ActivateScene(LevelIndexId lvl);
        public void DeactivateScene(LevelIndexId lvl);
        public void ReturnToMainScreen();
    }
}