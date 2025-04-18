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
        [SerializeField] private AudioSource _mMainAmbientSource3;
        [SerializeField] private AudioSource _mMainAmbientSource4;
        
        [SerializeField] private AudioSource _mMusicSource1;
        [SerializeField] private AudioSource _mMusicSource2;
        
        private AudioSource _mRadioSource;
        [SerializeField] private AudioClip ambientWindClip;
        [SerializeField] private AudioClip beastieBoysSong;
        [SerializeField] private AudioClip alarmSound;
        [SerializeField] private AudioClip warZoneEnvironmentSound;
        [SerializeField] private AudioClip sambaVeraoSong;
        [SerializeField] private AudioClip shotGunSound;

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
            _mMainAmbientSource1.loop = true;
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

        public void ToggleIntroSceneAlarmSound(bool state)
        {
            if (state)
            {
                if (_mMainAmbientSource1.clip != alarmSound)
                {
                    _mMainAmbientSource1.clip = alarmSound;
                }

                FadeIn(8f, .0225f, _mMainAmbientSource1);
            }
            else
            {
                _mMainAmbientSource1.Stop();
                _mMainAmbientSource1.loop = false;
            }
        }

        public void ToggleWarZoneSound(bool state)
        {
            if (state)
            {
                if (_mMainAmbientSource2.clip != warZoneEnvironmentSound)
                {
                    _mMainAmbientSource2.clip = warZoneEnvironmentSound;
                    _mMainAmbientSource2.loop = true;
                }
                FadeIn(4f, .08f, _mMainAmbientSource2);
                return;
            }
            _mMainAmbientSource2.Stop();
            _mMainAmbientSource2.loop = false;
        }

        public void StartIntroSceneMusic()
        {
            if (_mMusicSource1.clip != beastieBoysSong)
            {
                _mMusicSource1.clip = beastieBoysSong;
            }
            FadeIn(.01f, .06f, _mMusicSource1);
        }
        public void StartInterviewMusic()
        {
            if (_mMusicSource1.clip != sambaVeraoSong)
            {
                _mMusicSource1.clip = sambaVeraoSong;
            }
            FadeIn(.01f, .06f, _mMusicSource1);
        }

        public void PlayShotSound()
        {
            if (_mMainAmbientSource1.isPlaying)
            {
                _mMainAmbientSource1.Stop();
            }
            if (_mMainAmbientSource1.clip != shotGunSound)
            {
                _mMainAmbientSource1.clip = shotGunSound;
                _mMainAmbientSource1.volume = 0.3f;
            }
            _mMainAmbientSource1.loop = false;
            _mMainAmbientSource1.Play();            
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