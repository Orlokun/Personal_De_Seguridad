using DialogueSystem.Units;

namespace DialogueSystem.Interfaces
{
    public interface IDialogueObject : IDialogueObjectBaseData
    {
        public void AddDialogueCount();
        public DialogueSpeakerId GetSpeakerId(int dialogueLine);
    }
}