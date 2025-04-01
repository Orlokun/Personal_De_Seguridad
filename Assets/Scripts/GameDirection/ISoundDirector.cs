using UnityEngine;

namespace GameDirection
{
    public interface ISoundDirector
    {
        void PlayAmbientSound();
        void StopRadio();
        void SetRadioSource(AudioSource radioSource);
        void LowerMusicVolume();
        public void RaiseMusicVolume();

    }
}