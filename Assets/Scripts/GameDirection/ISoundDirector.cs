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
        public void StartIntroSceneAlarmSound();
        public void StartIntroSceneMusic();

    }
}