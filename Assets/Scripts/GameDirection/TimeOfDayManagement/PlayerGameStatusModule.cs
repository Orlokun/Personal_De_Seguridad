using System;
using DialogueSystem;
using GamePlayManagement;
using UnityEngine;
using Utils;

namespace GameDirection.TimeOfDayManagement
{
    public class PlayerGameStatusModule : IPlayerGameStatusModule
    {
        private int _totalOmniCredits;
        [Range(-100,100)]
        private int _mPlayerSeniority;
        [Range(-100,100)]
        private int _health;
        private int _stress;
        private int _mGameDifficulty;

        private int _mLastPlayedEnding = 0;
        private int _mAllPlayedEndings = 0;
        
        private IPlayerGameProfile _mPlayerProfile;
        public void SetProfile(IPlayerGameProfile currentPlayerProfile)
        {
            _mPlayerProfile = currentPlayerProfile;
            _totalOmniCredits = 35;
            _mPlayerSeniority = 1;
            _health = 20;
            _mGameDifficulty = 1;
        }

        public void PlayerLostResetData()
        {
            ResetBaseData();
        }

        public void PlayerLostGame(EndingTypes endingType)
        {
            var ending = (int)endingType;
            _mLastPlayedEnding = ending;
            if (BitOperator.IsActive(_mAllPlayedEndings, ending))
            {
                return;
            }
            _mAllPlayedEndings += ending;
        }

        //Must change depending oon meta game directions
        private void ResetBaseData()
        {
            _totalOmniCredits = 35;
            _mPlayerSeniority = 1;
            _health = 20;
            _mGameDifficulty = 1;
        }
        
        public int PlayerKnownEndings { get; }
        public int PlayerLastEnding { get; }

        public int PlayerOmniCredits
        {
            get => _totalOmniCredits;
            set => _totalOmniCredits = value;
        }

        public int MPlayerSeniority => _mPlayerSeniority;
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

        public void ReceiveOmniCredits(int amount)
        {
            _totalOmniCredits += amount;
        }

        public void ReceiveSeniority(int amount)
        {
            _mPlayerSeniority += amount;
        }
    }
}