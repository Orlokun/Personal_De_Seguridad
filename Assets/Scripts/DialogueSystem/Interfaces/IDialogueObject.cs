using System.Collections.Generic;
using DialogueSystem.Units;

namespace DialogueSystem.Interfaces
{
    public interface IDialogueObject : IDialogueObjectBaseData
    {
        public void AddDialogueCount();
        public DialogueSpeakerId GetSpeakerId(int dialogueLine);
    }

    public interface IDialogueObjectBaseData
    {
        public List<IDialogueNode> DialogueNodes { get; set; }

        public int TimesActivatedCount { get; }
    }
}