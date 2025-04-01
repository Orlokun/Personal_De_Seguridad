using System.Collections.Generic;
using DialogueSystem.Interfaces;
using UnityEngine;

namespace DialogueSystem.Units
{
    [CreateAssetMenu(menuName = "Dialogue/DialogueObject")]
    public class DialogueObject : ScriptableObject, IDialogueObject
    {
        [SerializeField] protected List<IDialogueNode> dialogueLines = new List<IDialogueNode>();
        [SerializeField] protected Sprite actorImage;
        [SerializeField] protected string speakerName;
        
        protected int _mTimesActivated = 0;

        public List<IDialogueNode> DialogueNodes
        {
            get => dialogueLines;
            set => dialogueLines = value;
        }

        public virtual List<IDialogueNode> GetDialogueNodes()
        {
            throw new System.NotImplementedException();
        }

        public int TimesActivatedCount => _mTimesActivated;
        public void AddDialogueCount()
        {
             _mTimesActivated++;
        }
        public DialogueSpeakerId GetSpeakerId(int dialogueLine)
        {
            return dialogueLines[dialogueLine].SpeakerId;
        }
    }
}