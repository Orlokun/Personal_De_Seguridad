using System.Collections.Generic;
using DialogueSystem.Interfaces;
using GamePlayManagement.TutorialManagement;
using UnityEngine;

namespace DialogueSystem.Units
{
    public enum DialogueBehaviors
    {
        SimpleDialogue = 1,
        DialogueWithChoice = 2,
        DialogueWithCamera = 4,
    }

    public interface ITutorialDialogueNode : IDialogueNode
    {
        public FeedbackObjects FeedbackObject { get; }
    }
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
    }
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
    }

    [CreateAssetMenu(menuName = "Dialogue/DialogueObject")]
    public class DialogueObject : ScriptableObject, IDialogueObject
    {
        [SerializeField] protected List<IDialogueNode> dialogueLines = new List<IDialogueNode>();
        [SerializeField] protected Sprite actorImage;
        [SerializeField] protected string speakerName;
        
        protected int _mTimesActivated = 0;

        public List<IDialogueNode> DialogueNodes
        {
            get => dialogueLines;
            set => dialogueLines = value;
        }

        public virtual List<IDialogueNode> GetDialogueNodes()
        {
            throw new System.NotImplementedException();
        }

        public int TimesActivatedCount => _mTimesActivated;
        public void AddDialogueCount()
        {
             _mTimesActivated++;
        }
        public DialogueSpeakerId GetSpeakerId(int dialogueLine)
        {
            return dialogueLines[dialogueLine].SpeakerId;
        }
    }
}