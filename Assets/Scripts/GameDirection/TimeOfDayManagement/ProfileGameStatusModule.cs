using System;
using GamePlayManagement;
using UnityEngine;

namespace GameDirection.TimeOfDayManagement
{
    public class ProfileGameStatusModule : IProfileGameStatusModule
    {
        private int _totalOmniCredits;
        [Range(-100,100)]
        private int _socialStatus;
        [Range(-100,100)]
        private int _health;
        private int _stress;
        private int _mGameDifficulty;
        
        private IPlayerGameProfile _mPlayerProfile;
        public void SetProfile(IPlayerGameProfile currentPlayerProfile)
        {
            _mPlayerProfile = currentPlayerProfile;
            _totalOmniCredits = 20000;
            _socialStatus = 10;
            _health = 20;
            _mGameDifficulty = 1;
        }

        public int PlayerOmniCredits
        {
            get => _totalOmniCredits;
            set => _totalOmniCredits = value;
        }

        public int PlayerSocialStatus => _socialStatus;
        public int PlayerHealth => _health;
        public int PlayerStress => _stress;
        public int GameDifficulty => _mGameDifficulty;
        public void DoCigarAction(Tuple<int, int> cigarCost)
        {
            var price = cigarCost.Item1;
            var health = cigarCost.Item2;
            _totalOmniCredits -= price;
            _health -= health;
        }
    }
}