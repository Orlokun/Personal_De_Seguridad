using GamePlayManagement.TutorialManagement;
using UnityEngine;

namespace DialogueSystem.Units
{
    [CreateAssetMenu(menuName = "Dialogue/DialogueObject")]
    public class TutorialDialogueObject : DialogueObject, ITutorialDialogueObject
    {
        public FeedbackObjects GetFeedbackObject(int nodeIndex)
        {
            throw new System.NotImplementedException();
        }
    }
}