using DataUnits;
using GameDirection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PopUpManager.OfficeRelatedPopUps
{
    public class PhoneAnswerPopUp : MonoBehaviour, IPhoneAnswerPopUp
    {
        [SerializeField] private TMP_Text mCallerDisplayObject;
        [SerializeField] private Button mAnswerPhone;
        [SerializeField] private Button mRejectCall;

        private void Start()
        {
            mAnswerPhone.onClick.AddListener(AnswerPhone);
            mRejectCall.onClick.AddListener(RejectCall);
        }

        public void SetAnswerData(ICallableSupplier caller)
        {
            var callerName = caller.SpeakerName;
            mCallerDisplayObject.text = callerName;
        }
    
        public void CleanAnswerData()
        {
            mCallerDisplayObject.text = "";
        }

        private void AnswerPhone()
        {
            PhoneCallOperator.Instance.PressCall();     
            FeedbackManager.Instance.DeactivatePhoneCallReceivedButton();
        }
        private void RejectCall()
        {
            PhoneCallOperator.Instance.FinishCallImmediately();     
            FeedbackManager.Instance.DeactivatePhoneCallReceivedButton();
        }
    }

    public interface IPhoneAnswerPopUp
    {
        public void SetAnswerData(ICallableSupplier caller);
    }
}