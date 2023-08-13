namespace DialogueSystem.Interfaces
{
    public interface IDialogueOperator
    {
        UIDialogueState CurrentDialogueState { get; }
        void StartNewDialogue(IDialogueObject newDialogue);
        void KillDialogue();
    }
}