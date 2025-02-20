using DialogueSystem.Units;

namespace DialogueSystem.Interfaces
{
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
}