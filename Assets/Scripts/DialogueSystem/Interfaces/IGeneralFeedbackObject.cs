using DialogueSystem.Units;

namespace DialogueSystem.Interfaces
{
    public interface IGeneralFeedbackObject
    {
        public GeneralFeedbackId FeedbackId { get; }
        public string FeedbackName { get; }
        public string FeedbackText { get; }
        public int FeedbackReadTime { get; }
    }
}