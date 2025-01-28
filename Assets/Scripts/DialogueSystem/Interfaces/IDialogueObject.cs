using System.Collections.Generic;
using DialogueSystem.Units;

namespace DialogueSystem.Interfaces
{
    public interface IDialogueObject : IDialogueObjectBaseData<IDialogueNode>
    {
        public void AddDialogueCount();
        public DialogueSpeakerId GetSpeakerId(int dialogueLine);
    }

    public interface IDialogueObjectBaseData<T> where T : IDialogueNode
    {
        public List<T> DialogueNodes { get; set; }

        public int TimesActivatedCount { get; }
    }

    public interface IImportantDialogueObject : IDialogueObject
    {
        public bool HasCondition { get; }
        public bool IsConditionMet { get; }
    }

    public enum GeneralFeedbackId
    {
        QE_MOVEMENT = 1,
        TAB_MOVEMENT = 2,
        MOUSE_OBJECTS = 3,
        STOREVIEW = 4,
        RADIO_FIRST_USE = 5,
    }
}