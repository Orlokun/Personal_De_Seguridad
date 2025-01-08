using System;
using GamePlayManagement.ProfileDataModules;

namespace GameDirection.TimeOfDayManagement
{
    public interface IPlayerGameStatusModule : IPlayerGameStatusModuleData
    {
        public void DoCigarAction(Tuple<int, int> cigarCost);
    }

    public interface IPlayerGameStatusModuleData : IProfileModule
    {
        public int PlayerOmniCredits { get; }
        public int PlayerXp { get; }
        public int PlayerHealth { get; }
        public int PlayerStress { get; }
        public int GameDifficulty { get; }
    }
}