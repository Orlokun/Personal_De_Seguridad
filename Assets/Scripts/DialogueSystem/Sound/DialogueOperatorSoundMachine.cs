using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Sound
{
    public class DialogueVoices
    {
        
    }
    
    [RequireComponent(typeof(AudioSource))]
    public class DialogueOperatorSoundMachine : MonoBehaviour, IDialogueOperatorSoundMachine
    {
        //Sound
        private AudioSource _mAudioSource;
        [SerializeField] private AudioClip typeWriterSound;
        [SerializeField] private List<AudioClip> characterVoices;

        private Dictionary<int, AudioClip> SpeakersInGame;
        private void Awake()
        {
            _mAudioSource = GetComponent<AudioSource>();
            if (!typeWriterSound)
            {
                Debug.LogError("Dialogue Operator Sound machine must have sound loaded");
                return;
            }
            SetAudioClip(typeWriterSound);
        }

        /*private AudioClip GetDialogueSpeakerAudioClip()
        {

        }*/
        private void SetAudioClip(AudioClip newClip)
        {
            _mAudioSource.clip = newClip;
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

    public enum SpeakerId
    {
              
    }
}