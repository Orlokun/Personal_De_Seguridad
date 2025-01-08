using DialogueSystem.Interfaces;
using GameDirection;
using InputManagement.MouseInput;
using UnityEngine;

namespace GamePlayManagement
{
    public class RadioSwitchOfficeObject : MonoBehaviour, IInteractiveClickableObject
    {
        #region Interactive Object Interface
        private AudioSource _mAudioSource;
        public AudioSource GetAudioSource => _mAudioSource;
        
        private int clickCount = 0;

        private void Start()
        {
            _mAudioSource = GetComponent<AudioSource>();
        }

        #endregion
        public void ReceiveActionClickedEvent()
        {
            throw new System.NotImplementedException();
        }

        public void ReceiveActionClickedEvent(RaycastHit hitInfo)
        {
            //Nothing to do, just deselect
        }

        public void ReceiveDeselectObjectEvent()
        {
            throw new System.NotImplementedException();
        }

        public void ReceiveClickEvent()
        {
            clickCount++;
            if (clickCount == 1)
            {
                FeedbackManager.Instance.StartReadingFeedback(GeneralFeedbackId.RADIO_FIRST_USE);
            }
            if (_mAudioSource.isPlaying)
            {
                _mAudioSource.Stop();
                return;
            }
            _mAudioSource.Play();
        }

        private bool _mHasSnippet = false;
        public string GetSnippetText { get; }
        public bool HasSnippet => _mHasSnippet;
        public void DisplaySnippet()
        {
            throw new System.NotImplementedException();
        }
    }
}