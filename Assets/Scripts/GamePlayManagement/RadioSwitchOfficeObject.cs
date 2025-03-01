using System.Collections;
using DialogueSystem.Interfaces;
using DialogueSystem.Units;
using GameDirection;
using GameDirection.ComplianceDataManagement;
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
        
        
        private bool _mWasStoppedManually;
        private int clickCount = 0;

        private Coroutine _mRadioCoroutine;
        
        private void Start()
        {
            _mAudioSource = GetComponent<AudioSource>();
            AttemptInitialize();
        }

        private void AttemptInitialize()
        {
            SoundDirector.Instance.SetRadioSource(_mAudioSource);
        }

        #endregion

        public void ReceiveActionClickedEvent(RaycastHit hitInfo)
        {
            if (hitInfo.collider.gameObject.GetComponent<RadioSwitchOfficeObject>())
            {
                if (_mAudioSource.isPlaying)
                {
                    _mWasStoppedManually = true;
                    StopMusic();
                    return;
                }
                StartPlayingMusic();
            }
        }

        public void ReceiveDeselectObjectEvent()
        {
            
        }
        
        private void StartPlayingMusic()
        {
            _mWasStoppedManually = false;
            _mAudioSource.Play();
            _mRadioCoroutine = StartCoroutine(CheckIfAudioFinished());
        }

        private void StopMusic()
        {
            _mAudioSource.Stop();
            if (_mRadioCoroutine != null && _mWasStoppedManually)
            {
                StopCoroutine(_mRadioCoroutine);
                _mRadioCoroutine = null;
            }
        }
        private IEnumerator CheckIfAudioFinished()
        {
            yield return new WaitUntil(() => !_mAudioSource.isPlaying);
            if (!_mWasStoppedManually)
            {
                CompleteFinishRadioClip();
                _mRadioCoroutine = null;
                yield break;
            }
            Debug.Log("Radio clip stopped Manually");
        }
        private void CompleteFinishRadioClip()
        {
            StopMusic();
            var complianceManager = (IComplianceEvaluationEvents)GameDirector.Instance.GetActiveGameProfile.GetComplianceManager;
            complianceManager.CheckRadioCompleted();
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
                _mWasStoppedManually = true;
                StopMusic();
                return;
            }
            StartPlayingMusic();
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