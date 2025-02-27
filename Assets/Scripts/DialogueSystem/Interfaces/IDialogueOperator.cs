using System.Collections.Generic;
using DialogueSystem.Units;
using UnityEngine.Events;

namespace DialogueSystem.Interfaces
{
    public interface IDialogueOperator
    {
        public IDialogueEventsOperator GetDialogueEventsOperator { get; }
        UIDialogueState CurrentDialogueState { get; }
        public UnityAction WriteNextDialogueNode(IDialogueObject dObject, int newDialogueIndex);
        void StartNewDialogue(IDialogueObject newDialogue);
        void KillDialogue();
        event DialogueOperator.FinishedDialogueReading OnDialogueCompleted;
        public List<IDialogueObject> GetDialogueObjectInterfaces(List<DialogueObject> dialogueObjects);
        public void GetSpeakerDataWithSpeakerId(DialogueSpeakerId speakerId);
    }
}