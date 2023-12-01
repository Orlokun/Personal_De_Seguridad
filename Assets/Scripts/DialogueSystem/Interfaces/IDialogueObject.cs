using System.Collections.Generic;
using DialogueSystem.Units;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem.Interfaces
{
    public interface IDialogueObject
    {
        public List<string> DialogueLines { get; set; }
        public Sprite ActorImage{ get; set; }
        public string SpeakerName{ get; set; }
    }

    public interface IGeneralFeedbackObject
    {
        public GeneralFeedbackId FeedbackId { get; }
        public string FeedbackName { get; }
        public string FeedbackText { get; }
        public int FeedbackReadTime { get; }
    }

    public class GeneralFeedbackObject : IGeneralFeedbackObject
    {
        private GeneralFeedbackId _mFeedbackId;
        private string _mFeedbackName;  
        private string _mFeedbackText;
        private int _mFeedbackReadTime;

        public GeneralFeedbackId FeedbackId => _mFeedbackId;
        public string FeedbackName => _mFeedbackName;
        public string FeedbackText => _mFeedbackText;
        public int FeedbackReadTime => _mFeedbackReadTime;

        public GeneralFeedbackObject(GeneralFeedbackId id, string name, string text, int readTime)
        {
            _mFeedbackId = id;
            _mFeedbackName = name;
            _mFeedbackText = text;
            _mFeedbackReadTime = readTime;
        }
    }

    public enum GeneralFeedbackId
    {
        QE_MOVEMENT = 1,
        TAB_MOVEMENT = 2,
        MOUSE_OBJECTS = 3
    }
}