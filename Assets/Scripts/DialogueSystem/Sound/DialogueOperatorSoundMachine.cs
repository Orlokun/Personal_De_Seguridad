using System;
using UnityEngine;

namespace DialogueSystem.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class DialogueOperatorSoundMachine : MonoBehaviour, IDialogueOperatorSoundMachine
    {
        //Sound
        private AudioSource _mAudioSource;
        [SerializeField] private AudioClip typeWriterSound;

        private void Awake()
        {
            _mAudioSource = GetComponent<AudioSource>();
            if (!typeWriterSound)
            {
                Debug.LogError("Dialogue Operator Sound machine must have sound loaded");
                return;
            }
            SetAudioClip();
        }

        private void SetAudioClip()
        {
            _mAudioSource.clip = typeWriterSound;
        }

        public void StartPlayingSound()
        {
            _mAudioSource.Play();
        }

        public void StopPlayingSound()
        {
            _mAudioSource.Stop();
        }

        public void PausePlayingSound()
        {
            _mAudioSource.Pause();
        }
    }
}