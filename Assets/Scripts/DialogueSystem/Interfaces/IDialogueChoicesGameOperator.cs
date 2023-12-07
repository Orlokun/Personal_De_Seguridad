using DialogueSystem.Units;

namespace DialogueSystem.Interfaces
{
    public interface IDialogueChoicesGameOperator
    {
        public void ToggleActive(bool isActive);
        public void DisplayDialogueChoices(IDialogueNode dialogueNode, IDialogueObject completeDialogue, IDialogueOperator dialogueOperator);
    }
}