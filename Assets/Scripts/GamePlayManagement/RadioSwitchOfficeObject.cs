using DialogueSystem.Interfaces;
using GameDirection;
using InputManagement.MouseInput;
using UnityEngine;

namespace GamePlayManagement
{
    public interface IRadioOperator
    {
        void TurnRadioPower(bool value);
    }
    
    public class RadioSwitchOfficeObject : MonoBehaviour, IInteractiveClickableObject, IRadioOperator
    {
        #region Interactive Object Interface
        private AudioSource _mAudioSource;
        public AudioSource GetAudioSource => _mAudioSource;
        
        private int clickCount = 0;

        private void Start()
        {
            _mAudioSource = GetComponent<AudioSource>();
            AttepmtToInitialize();
        }

        private void AttepmtToInitialize()
        {
            SoundDirector.Instance.SetRadioSource(_mAudioSource);
        }

        #endregion
        public void ReceiveActionClickedEvent()
        {
            throw new System.NotImplementedException();
        }

        public void ReceiveActionClickedEvent(RaycastHit hitInfo)
        {
            if (hitInfo.collider.gameObject.GetComponent<RadioSwitchOfficeObject>())
            {
                if (_mAudioSource.isPlaying)
                {
                    _mAudioSource.Stop();
                    return;
                }
                _mAudioSource.Play();    
            }
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

        public void TurnRadioPower(bool value)
        {
            if (value)
            {
                _mAudioSource.Play();
                return;
            }
            _mAudioSource.Stop();
        }
    }
}