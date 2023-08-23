using System.Collections.Generic;
using DialogueSystem.Units;

namespace DialogueSystem.Interfaces
{
    public interface IDialogueOperator
    {
        UIDialogueState CurrentDialogueState { get; }
        void StartNewDialogue(IDialogueObject newDialogue);
        void KillDialogue();
        event DialogueOperator.FinishedDialogueReading OnDialogueCompleted;
        public List<IDialogueObject> GetDialogueObjects(List<BaseDialogueObject> dialogueObjects);
        
    }
}