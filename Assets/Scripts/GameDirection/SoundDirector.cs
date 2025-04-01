using System.Collections;
using GamePlayManagement;
using UnityEngine;
namespace GameDirection
{
    public class SoundDirector : MonoBehaviour, ISoundDirector
    {
        private static ISoundDirector mInstance;
        public static ISoundDirector Instance => mInstance;

        [SerializeField] private AudioSource _mMainAmbientSource1;
        [SerializeField] private AudioSource _mMainAmbientSource2;
        
        [SerializeField] private AudioSource _mMusicSource1;
        [SerializeField] private AudioSource _mMusicSource2;
        
        private AudioSource _mRadioSource;
        [SerializeField] private AudioClip ambientWindClip;
        [SerializeField] private AudioClip beastieBoysSong;
        [SerializeField] private AudioClip alarmSound;

        private void Awake()
        {
            if (mInstance != null && mInstance != this)
            {
                Destroy(this);
            }

            DontDestroyOnLoad(this);
            mInstance = this;
            CheckInitialComponents();
        }

        private void CheckInitialComponents()
        {

        }

        public void PlayRegularDayAmbientSound()
        {
            if (_mMainAmbientSource1.clip != ambientWindClip)
            {
                _mMainAmbientSource1.clip = ambientWindClip;
            }

            _mMainAmbientSource1.volume = .5f;
            _mMainAmbientSource1.Play();
        }

        public void StopRadio()
        {
            if (FindFirstObjectByType<RadioSwitchOfficeObject>() != null)
            {
                IRadioOperator radio = FindFirstObjectByType<RadioSwitchOfficeObject>();
                if (radio != null)
                {
                    radio.TurnRadioPower(false);
                }
            }
        }

        public void SetRadioSource(AudioSource radioSource)
        {
            if (_mRadioSource == null)
            {
                _mRadioSource = radioSource;
                RaiseMusicVolume();
            }
        }

        public void LowerMusicVolume()
        {
            if (_mRadioSource != null)
            {
                if (_mRadioSource.isPlaying)
                {
                    _mRadioSource.volume = .35f;
                }
            }
        }

        public void RaiseMusicVolume()
        {
            if (_mRadioSource != null)
            {
                _mRadioSource.volume = .75f;
            }
        }

        private float currentRadioVolume = 0f;

        public void StartIntroSceneAlarmSound()
        {
            if (_mMainAmbientSource1.clip != alarmSound)
            {
                _mMainAmbientSource1.clip = alarmSound;
            }
            FadeIn(4f, .08f, _mMainAmbientSource1);
        }
        
        public void StartIntroSceneMusic()
        {
            if (_mMusicSource1.clip != beastieBoysSong)
            {
                _mMusicSource1.clip = beastieBoysSong;
            }
            FadeIn(8f, .1f, _mMusicSource1);
        }

        private void FadeIn(float time, float targetVolume, AudioSource audioSource)
        {
            GameDirector.Instance.ActCoroutine(FadeInCoroutine(time, targetVolume, audioSource));
        }

        private IEnumerator FadeInCoroutine(float time, float targetVolume,  AudioSource audioSource)
        {
            float startVolume = 0f;
            float elapsedTime = 0f;
            audioSource.Play();
            while (elapsedTime < time)
            {
                elapsedTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / time);
                yield return null;
            }
            audioSource.volume = targetVolume;
        }

}
}