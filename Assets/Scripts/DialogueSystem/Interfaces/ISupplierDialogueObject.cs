namespace DialogueSystem.Interfaces
{
    public interface ISupplierDialogueObject : IDialogueObject
    {
        public int GetDialogueAssignedStatus { get; }
        public void SetDialogueStatus(int status);
    }
}