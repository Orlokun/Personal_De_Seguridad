namespace DialogueSystem.Interfaces
{
    internal interface IDialogueCameraMan
    {
        public void ManageDialogueCameraBehavior(IDialogueObject currentDialogue, int dialogueLineIndex);
    }
}