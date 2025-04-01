using DialogueSystem.Interfaces;

namespace DialogueSystem
{
    public interface IBaseTutorialDialogueData
    {
        IDialogueObject GetTutorialDialogue(int dialogueIndex);
    }
}