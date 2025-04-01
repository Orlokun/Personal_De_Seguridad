using DataUnits;
using DialogueSystem.Units;

namespace GameDirection
{
    public interface IFeedbackManager
    {
        public void StartReadingFeedback(GeneralFeedbackId feedbackType);

        public void ActivatePhoneCallReceivedButton(ICallableSupplier caller);
        public void DeactivatePhoneCallReceivedButton();
        
        void StartTrustFeedback(ICallableSupplier supplierTrust, int trustValue);
    }
}