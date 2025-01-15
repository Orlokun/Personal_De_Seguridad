using GamePlayManagement;
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

    [RequireComponent(typeof(AudioSource))]
    public class SoundDirector : MonoBehaviour, ISoundDirector
    {
        private static ISoundDirector mInstance;
        public static ISoundDirector Instance => mInstance;
        
        private AudioSource _mMainAudioSource;
        private AudioSource _mRadioSource;
        [SerializeField] private AudioClip ambientWindClip;

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
            _mMainAudioSource = GetComponent<AudioSource>();
            if (!_mMainAudioSource)
            {
                Debug.LogError("Audio source must be a component of Sound director game object");
                return;
            }
            if (!ambientWindClip)
            {
                Debug.LogError("Ambient sound must be set from editor");
                return;
            }
            _mMainAudioSource.clip = ambientWindClip;
        }

        public void PlayAmbientSound()
        {
            if (_mMainAudioSource.clip != ambientWindClip)
            {
                _mMainAudioSource.clip = ambientWindClip;
            }
            _mMainAudioSource.volume = .5f;
            _mMainAudioSource.Play();
        }

        public void StopRadio()
        {
            if(FindFirstObjectByType<RadioSwitchOfficeObject>() != null)
            {
                IRadioOperator radio = FindFirstObjectByType<RadioSwitchOfficeObject>();
                if(radio != null)
                {
                    radio.TurnRadioPower(false);
                }
            }
        }

        public void SetRadioSource(AudioSource radioSource)
        {
            if(_mRadioSource == null)
            {
                _mRadioSource = radioSource;
                RaiseMusicVolume();
            }
        }

        public void LowerMusicVolume()
        {
            if(_mRadioSource != null)
            {
                if(_mRadioSource.isPlaying)
                {
                    _mRadioSource.volume = .35f;
                }
            }
        }

        public void RaiseMusicVolume()
        {
            if(_mRadioSource != null)
            {
                _mRadioSource.volume = .75f;
            }
        }
    }
}