using UnityEngine;
namespace GameDirection
{
    public interface ISoundDirector
    {
        void PlayAmbientMusic();
    }

    [RequireComponent(typeof(AudioSource))]
    public class SoundDirector : MonoBehaviour, ISoundDirector
    {
        private static SoundDirector _mInstance;

        public static SoundDirector Instance => _mInstance;
        
        private AudioSource _mMainAudioSource;
        [SerializeField] private AudioClip ambientWindClip;

        private void Awake()
        {
            if (_mInstance != null && _mInstance != this)
            {
                Destroy(this);
            }
            DontDestroyOnLoad(this);
            _mInstance = this;
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

        public void PlayAmbientMusic()
        {
            if (_mMainAudioSource.clip != ambientWindClip)
            {
                _mMainAudioSource.clip = ambientWindClip;
            }
            _mMainAudioSource.volume = .5f;
            _mMainAudioSource.Play();
        }
    }
}