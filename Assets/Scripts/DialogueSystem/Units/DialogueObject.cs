using System.Collections.Generic;
using DialogueSystem.Interfaces;
using DialogueSystem.Sound;
using UnityEngine;

namespace DialogueSystem.Units
{
    public enum DialogueBehaviors
    {
        SimpleDialogue = 1,
        DialogueWithChoice = 2,
        DialogueWithCamera = 4,
    }
    
    public interface IDialogueNode
    {
        public int DialogObjectIndex { get ; }
        public int DialogueLineIndex { get ; }
        public DialogueSpeakerId SpeakerId { get ; }
        public string DialogueLine { get ; }
        public bool HasCameraTarget { get ; }
        public string TargetCameraId { get ; }
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
        private string _targetCameraId;
        private bool _hasChoice;
        private bool _hasEvent;
        private string _eventNameId;
        private int[] _linkNodes;
        public DialogueNodeData(int dialogObjectIndex, int dialogueLineIndex, int speakerId, string dialogueLine, bool hasCameraTarget, 
            string targetCameraId, bool hasChoice, bool hasEvent, string eventNameId, int[] linkNodes)
        {
            _dialogObjectIndex = dialogObjectIndex;
            _dialogueLineIndex = dialogueLineIndex;
            _speakerId = (DialogueSpeakerId)speakerId;
            _dialogueLine = dialogueLine;
            _hasCameraTarget = hasCameraTarget;
            _targetCameraId = targetCameraId;
            _hasChoice = hasChoice;
            _hasEvent = hasEvent;
            _eventNameId = eventNameId;
            _linkNodes = linkNodes;
        }
        public DialogueNodeData(OmniIntroDialogues dialogObjectIndex, int dialogueLineIndex, int speakerId, string dialogueLine, bool hasCameraTarget, 
            string targetCameraId, bool hasChoice, bool hasEvent, string eventNameId, int[] linkNodes)
        {
            _dialogObjectIndex = (int)dialogObjectIndex;
            _dialogueLineIndex = dialogueLineIndex;
            _speakerId = (DialogueSpeakerId)speakerId;
            _dialogueLine = dialogueLine;
            _hasCameraTarget = hasCameraTarget;
            _targetCameraId = targetCameraId;
            _hasChoice = hasChoice;
            _hasEvent = hasEvent;
            _eventNameId = eventNameId;
            _linkNodes = linkNodes;
        }
        
        public int DialogObjectIndex => _dialogObjectIndex;
        public int DialogueLineIndex => _dialogueLineIndex;
        public DialogueSpeakerId SpeakerId => _speakerId;
        public string DialogueLine => _dialogueLine;
        public bool HasCameraTarget => _hasCameraTarget;
        public string TargetCameraId => _targetCameraId;
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
        private int _mTimesActivated;

        public List<IDialogueNode> DialogueNodes
        {
            get => dialogueLines;
            set => dialogueLines = value;
        }
        public int TimesActivatedCount => _mTimesActivated;
        public void DialogueRead()
        {
            _mTimesActivated++;
        }

        public DialogueSpeakerId GetSpeakerId(int dialogueLine)
        {
            return (DialogueSpeakerId)dialogueLines[dialogueLine].SpeakerId;
        }
    }
}