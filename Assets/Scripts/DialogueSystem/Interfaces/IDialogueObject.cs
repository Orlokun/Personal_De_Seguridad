using System.Collections.Generic;
using DialogueSystem.Units;
using UnityEngine;

namespace DialogueSystem.Interfaces
{
    public interface IDialogueObject : IDialogueObjectBaseData
    {
        public void DialogueRead();
    }

    public interface IDialogueObjectBaseData
    {
        public List<IDialogueNode> DialogueNodes { get; set; }
        public Sprite ActorImage{ get; set; }
        public string SpeakerName{ get; set; }
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