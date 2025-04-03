using System;
using TMPro;
using UnityEngine;

namespace UI.PopUpManager.InfoPanelPopUp
{
    public class GamePlayActionTimer : PopUpObject, IGamePlayActionTimer
    {
        private bool _mIsTimerActive;
        public bool IsTimerActive => _mIsTimerActive;
        public delegate void TimerFinished();
        public event Action OnTimerEnd;
    
        private float _mTimerSeconds;
    
        [SerializeField] TMP_Text mTimerText;
    
        public void StartTimer(float seconds)
        {
            _mTimerSeconds = seconds;
            _mIsTimerActive = true;
        }

        private void Update()
        {
            if (_mIsTimerActive)
            {
                _mTimerSeconds -= Time.deltaTime;
                mTimerText.text = _mTimerSeconds.ToString("F0");
                if (_mTimerSeconds <= 0)
                {
                    _mIsTimerActive = false;
                    _mTimerSeconds = 0;
                    OnTimerEnd?.Invoke();
                }
            }
        }
    }

    public interface IGamePlayActionTimer : IPopUpObject
    {
        public event Action OnTimerEnd;
        public void StartTimer(float seconds);
    }
}