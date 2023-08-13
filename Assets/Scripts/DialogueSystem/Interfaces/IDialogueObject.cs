using System.Collections.Generic;
using DialogueSystem.Units;
using UnityEngine;

namespace DialogueSystem.Interfaces
{
    public interface IDialogueObject
    {
        public List<string> DialogueLines { get; set; }
        public Sprite ActorImage{ get; set; }
        public string SpeakerName{ get; set; }
        public int DialogueBehaviors { get; }
        public bool ContainsBehavior(DialogueBehaviors checkedBehavior);
    }
}