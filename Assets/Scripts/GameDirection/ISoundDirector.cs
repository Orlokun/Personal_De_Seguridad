using UnityEngine;

namespace GameDirection
{
    public interface ISoundDirector
    {
        void PlayRegularDayAmbientSound();
        void StopRadio();
        void SetRadioSource(AudioSource radioSource);
        void LowerMusicVolume();
        public void RaiseMusicVolume();
        public void ToggleIntroSceneAlarmSound(bool state);
        public void ToggleWarZoneSound(bool state);
        public void StartIntroSceneMusic();
        public void StartInterviewMusic();
        public void PlayShotSound();
    }
}