using System;
using GamePlayManagement.ProfileDataModules;

namespace GameDirection.TimeOfDayManagement
{
    public interface IProfileGameStatusModule : IProfileModule
    {
        public int PlayerOmniCredits { get; }
        public int PlayerSocialStatus { get; }
        public int PlayerHealth { get; }
        public int PlayerStress { get; }
        public int GameDifficulty { get; }
        public void DoCigarAction(Tuple<int, int> cigarCost);
    }
}