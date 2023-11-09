using UnityEngine;

namespace DialogueSystem.Sound
{
    public interface IDialogueOperatorSoundMachine
    {
        void StartPlayingSound();
        void StopPlayingSound();
        void PausePlayingSound();
        void SetNewAudioClip(AudioClip voiceTextSound);
    }
}