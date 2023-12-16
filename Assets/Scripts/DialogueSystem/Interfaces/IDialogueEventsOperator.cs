using GamePlayManagement.BitDescriptions.Suppliers;

namespace DialogueSystem.Interfaces
{
    public interface IDialogueEventsOperator
    {
        public void HandleDialogueEvent(string eventCompleteCode);
        public void LaunchHireEvent(JobSupplierBitId jobSupplierId);
    }
}