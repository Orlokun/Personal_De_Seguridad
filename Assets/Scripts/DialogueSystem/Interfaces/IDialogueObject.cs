using System.Collections.Generic;
using DialogueSystem.Units;
using UnityEngine;

namespace DialogueSystem.Interfaces
{
    public interface IDialogueObject
    {
        public List<IDialogueNode> DialogueNodes { get; set; }
        public Sprite ActorImage{ get; set; }
        public string SpeakerName{ get; set; }
    }

    public enum GeneralFeedbackId
    {
        QE_MOVEMENT = 1,
        TAB_MOVEMENT = 2,
        MOUSE_OBJECTS = 3,
        STOREVIEW = 4,
    }
}