using GamePlayManagement.ProfileDataModules;

namespace GameDirection.TimeOfDayManagement
{
    public interface IPlayerGameStatusModuleData : IProfileModule
    {
        public int PlayerKnownEndings { get; }
        public int PlayerLastEnding { get; }
        public int PlayerOmniCredits { get; }
        public int MPlayerSeniority { get; }
        public int PlayerHealth { get; }
        public int PlayerStress { get; }
        public int GameDifficulty { get; }
    }
}