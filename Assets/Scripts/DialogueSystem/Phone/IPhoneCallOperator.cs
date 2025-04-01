using DataUnits.JobSources;
using DialogueSystem.Interfaces;

namespace DialogueSystem.Phone
{
    public interface IPhoneCallOperator
    {
        void PressCall();
        void GoToCall(ISupplierBaseObject callReceiver);
        void FinishCallImmediately();
        void DialNumber(int number);
        void PlayAnswerSound();
        void StartCallFromSupplier(IDialogueObject receivedCallDialogue);
    }
}