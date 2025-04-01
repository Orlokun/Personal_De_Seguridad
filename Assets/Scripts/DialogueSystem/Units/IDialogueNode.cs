namespace DialogueSystem.Units
{
    public interface IDialogueNode
    {
        public int DialogObjectIndex { get ; }
        public int DialogueLineIndex { get ; }
        public DialogueSpeakerId SpeakerId { get ; }
        public string DialogueLine { get ; }
        public bool HasCameraTarget { get ; }
        public string[] CameraEvent { get ; }
        public bool HasChoice { get ; }
        public bool HasEvent { get ; }
        public string EventCodes { get ; }
        public int[] LinkNodes { get ; set; }
        public bool HasHighlightEvent { get; }
        public string[] HighlightEvent { get; }
    }
}