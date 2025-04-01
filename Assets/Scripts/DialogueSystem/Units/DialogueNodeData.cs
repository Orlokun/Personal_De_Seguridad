namespace DialogueSystem.Units
{
    public class DialogueNodeData : IDialogueNode
    {
        private int _dialogObjectIndex;
        private int _dialogueLineIndex;
        private DialogueSpeakerId _speakerId;
        private string _dialogueLine;
        private bool _hasCameraTarget;
        private string[] _cameraEvent;
        private bool _hasChoice;
        private bool _hasEvent;
        private string _eventNameId;
        private int[] _linkNodes;
        
        private bool _mHasHighlightEvent;
        private string[] _mHighlightEvent;
        

        public DialogueNodeData(OmniIntroDialogues dialogObjectIndex, int dialogueLineIndex, int speakerId, string dialogueLine, bool hasCameraTarget, 
            string[] cameraEvent, bool hasChoice, bool hasEvent, string eventNameId, int[] linkNodes)
        {
            _dialogObjectIndex = (int)dialogObjectIndex;
            _dialogueLineIndex = dialogueLineIndex;
            _speakerId = (DialogueSpeakerId)speakerId;
            _dialogueLine = dialogueLine;
            _hasCameraTarget = hasCameraTarget;
            _cameraEvent = cameraEvent;
            _hasChoice = hasChoice;
            _hasEvent = hasEvent;
            _eventNameId = eventNameId;
            _linkNodes = linkNodes;
        }

        public DialogueNodeData(int currentDialogueObjectIndex, int dialogueLineIndex, int speakerId, string dialogueLineText, bool hasCameraTarget, string[] cameraEvent, bool hasChoices, bool hasEventId, string eventNameId, int[] nodesLined, bool hasHighlightEvent, string[] highlightEvent)
        {
            _dialogObjectIndex = currentDialogueObjectIndex;
            _dialogueLineIndex = dialogueLineIndex;
            _speakerId = (DialogueSpeakerId)speakerId;
            _dialogueLine = dialogueLineText;
            _hasCameraTarget = hasCameraTarget;
            _cameraEvent = cameraEvent;
            _hasChoice = hasChoices;
            _hasEvent = hasEventId;
            _eventNameId = eventNameId;
            _linkNodes = nodesLined;
            
            _mHasHighlightEvent = hasHighlightEvent;
            _mHighlightEvent = highlightEvent;
        }

        public int DialogObjectIndex => _dialogObjectIndex;
        public int DialogueLineIndex => _dialogueLineIndex;
        public DialogueSpeakerId SpeakerId => _speakerId;
        public string DialogueLine => _dialogueLine;
        public bool HasCameraTarget => _hasCameraTarget;
        public string[] CameraEvent => _cameraEvent;
        public bool HasChoice => _hasChoice;
        public bool HasEvent => _hasEvent;
        public string EventCodes => _eventNameId;
        public int[] LinkNodes
        {
            get => _linkNodes;
            set => _linkNodes = value;
        }
        public bool HasHighlightEvent => _mHasHighlightEvent;
        public string[] HighlightEvent => _mHighlightEvent;
    }
}