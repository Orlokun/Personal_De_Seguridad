using GamePlayManagement.TutorialManagement;

namespace DialogueSystem.Units
{
    public interface ITutorialDialogueObject
    {
        FeedbackObjects GetFeedbackObject(int nodeIndex);
    }
}