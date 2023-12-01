using System.Collections.Generic;
using DialogueSystem.Interfaces;
using UnityEngine;

namespace DialogueSystem.Units
{
    public enum DialogueBehaviors
    {
        SimpleDialogue = 1,
        DialogueWithChoice = 2,
        DialogueWithCamera = 4,
    }

    public interface IDialogueLineObject
    {
    
    }
    public class DialogueLineObjectData : ScriptableObject, IDialogueLineObject
    {
        public string dialogueLine;
        
        public bool hasCameraTarget;
        public string targetId;

        public bool hasDecision;
        public bool decisionId;
    }

    [CreateAssetMenu(menuName = "Dialogue/DialogueObject")]
    public class BaseDialogueObject : ScriptableObject, IDialogueObject
    {
        [SerializeField] protected List<string> dialogueLines = new List<string>();
        [SerializeField] protected Sprite actorImage;
        [SerializeField] protected string speakerName;

        public List<string> DialogueLines
        {
            get => dialogueLines;
            set => dialogueLines = value;
        }

        public Sprite ActorImage
        {
            get => actorImage;
            set => actorImage = value;
        }

        public string SpeakerName
        {
            get => speakerName;
            set => speakerName = value;
        }
    }
}