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

    [CreateAssetMenu(menuName = "Dialogue/DialogueObject")]
    public class BaseDialogueObject : ScriptableObject, IDialogueObject
    {
        [SerializeField] protected List<string> dialogueLines = new List<string>();
        [SerializeField] protected Sprite actorImage;
        [SerializeField] protected string speakerName;
        [SerializeField] protected List<DialogueBehaviors> dialogueBehaviorsList;
        protected int MDialogueBehaviors;

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

        public int DialogueBehaviors => MDialogueBehaviors;

        public bool ContainsBehavior(DialogueBehaviors checkedBehavior)
        {
            return (MDialogueBehaviors & (int)checkedBehavior) != 0;
        }

        private void OnEnable()
        {
            LoadDialogueBehaviors();
        }

        private void LoadDialogueBehaviors()
        {
            if (dialogueBehaviorsList == null || dialogueBehaviorsList.Count == 0)
            {
//                Debug.LogWarning("Dialogue Behaviors must be set in editor");
                return;
            }
            foreach (var dialogueBehavior in dialogueBehaviorsList)
            {
                if ((MDialogueBehaviors & (int) dialogueBehavior) != 0)
                {
                    continue;
                }
                MDialogueBehaviors |= (int) dialogueBehavior;
            }
        }
    }
}