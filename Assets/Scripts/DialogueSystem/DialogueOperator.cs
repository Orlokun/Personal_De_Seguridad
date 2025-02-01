using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CameraManagement;
using DataUnits;
using DataUnits.JobSources;
using DialogueSystem.Interfaces;
using DialogueSystem.Sound;
using DialogueSystem.Units;
using GameDirection;
using InputManagement;
using TMPro;
using UI;
using UI.PopUpManager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace DialogueSystem
{
    public enum UIDialogueState
    {
        NotDisplayed,
        TypingText,
        FinishedTypingLine,
    }
    public class DialogueOperator : MonoBehaviour, IDialogueOperator
    {
        
        #region Static Props/Members
        private static DialogueOperator _mInstance;
        public static IDialogueOperator Instance => _mInstance;
        #endregion
        /// <summary>
        /// Reference of UI Editor Components
        /// </summary>
        #region Serialized Members
        [SerializeField] private Image mSpeakerImageDisplay;
        [SerializeField] private TMP_Text mDialogueTextObject;
        [SerializeField] private TMP_Text mSpeakerName;
        [SerializeField] private GameObject nextLineButton;
        [SerializeField] private GameObject decisionObjectsParent;
        
        //Parameters to simply randomize the writing speed a Bit. 
        [SerializeField] private float maxWritingSpeed;
        [SerializeField] private float minWritingSpeed;
        [SerializeField] private DialogueChoicesGameOperator _dialogueChoiceOperator;
        private DialogueEventsOperator _dialogueEventsOperator;
        private IDialogueChoicesGameOperator dialogueChoicesOperator => _dialogueChoiceOperator;
        public IDialogueEventsOperator GetDialogueEventsOperator => _dialogueEventsOperator;

        #endregion

        #region Private Members
        
        /// <summary>
        /// Private Members and dependencies
        /// </summary>
        private UIDialogueState _currentState;
        public UIDialogueState CurrentDialogueState => _currentState;

        private bool _waitingForChoice;
        private IDialogueObject _currentDialogue;
        private Coroutine _typingMachineCoroutine;
        private IUIController _mUIController;
        private IDialogueOperatorSoundMachine _soundMachine;
        private ICallableSupplier _mOmnicorpDialogueInfo;
        
        
        #endregion
        
        public delegate void FinishedDialogueReading();
        public event FinishedDialogueReading OnDialogueCompleted;
        
        #region Public Interface
        public List<IDialogueObject> GetDialogueObjectInterfaces(List<DialogueObject> dialogueObjects)
        {
            var dialogueList = new List<IDialogueObject>();
            foreach (var baseDialogueObject in dialogueObjects)
            {
                dialogueList.Add(baseDialogueObject);
            }
            return dialogueList;
        }

        public void GetSpeakerDataWithSpeakerId(DialogueSpeakerId speakerId)
        {
            throw new NotImplementedException();
        }

        public void StartNewDialogue(IDialogueObject newDialogue)
        {
            _currentState = UIDialogueState.TypingText;
            Debug.Log("Starting new Dialogue");
            LoadNewDialogue(newDialogue);
            
            _mUIController.ToggleDialogueObject(true);
            newDialogue.AddDialogueCount();
            WriteNextDialogueNode(_currentDialogue, 0);
        }

        public UnityAction WriteNextDialogueNode(IDialogueObject dObject, int newDialogueIndex)
        {
            if (dObject != _currentDialogue)
            {
                return null;
            }
            var dialogueNode = dObject.DialogueNodes[newDialogueIndex];
            StartCoroutine(WriteDialogueNode(dialogueNode));
            return null;
        }
        
        private void OnDialogueFinished()
        {
            GameCameraOperator.Instance.ReturnToLastState();
            KillDialogue();
        }

        public void KillDialogue()
        {
            _currentState = UIDialogueState.NotDisplayed;
            _currentDialogue = null;
            mDialogueTextObject.text = "";
            mSpeakerName.text = "";
            mSpeakerImageDisplay.color = new Color(1, 1, 1, 0);
            _mUIController.ToggleDialogueObject(false);

        }
        #endregion

        #region Init
        private void Awake()
        {
            if (_mInstance != null)
            {
                Destroy(this);
            }
            _mInstance = this;
            _mOmnicorpDialogueInfo = ScriptableObject.CreateInstance<OmniCorpCallObject>();
            _soundMachine = GetComponent<DialogueOperatorSoundMachine>();
            OnDialogueCompleted += OnDialogueFinished;
        }
        private void Start()
        {
            _mUIController = UIController.Instance;
            _dialogueEventsOperator = new DialogueEventsOperator();
        }
        #endregion

        #region PrivateUtils

        #region Main Writing Loop
        private IEnumerator WriteDialogueNode(IDialogueNode dialogueNode)
        {

            //Make Sure no other line is being written at the moment
            if (_typingMachineCoroutine != null)
            {
                StopCoroutine(_typingMachineCoroutine);
            }

            //Start Typewriter effect of the current line    
            mDialogueTextObject.text = "";
            _typingMachineCoroutine = StartCoroutine(WriteDialogueLine(dialogueNode));
            
            //Wait for the conditions to be met in order to continue the loop
            yield return new WaitUntil(NextLineWritingConditions);

            //The following line of code makes the coroutine wait for a frame so as the next WaitUntil is not skipped
            yield return null;
            CheckDialogueLineEventBehavior(dialogueNode);
            if (dialogueNode.LinkNodes[0] == 0)
            {
                Debug.Log("[DialogueOperator.WriteDialogueNode] Finished going through dialogur");
                _currentState = UIDialogueState.NotDisplayed;
                OnDialogueCompleted?.Invoke();
            }
            else if(dialogueNode.LinkNodes.Length == 1)
            {
                WriteNextDialogueNode(_currentDialogue, dialogueNode.LinkNodes.First());
            }
        }

        private bool NextLineWritingConditions()
        {
            return Input.GetKeyDown(KeyCode.Space)
                   && _currentState == UIDialogueState.FinishedTypingLine
                   && IGeneralGameGameInputManager.Instance.CurrentInputGameState != InputGameState.Pause;
        }

        private IEnumerator WriteDialogueLine(IDialogueNode dialogueNode)
        {
            _currentState = UIDialogueState.TypingText;
            var isAddingHtmlTag = false;            //Used to manage tags inside text so they don't appear in the UI.
            
            _soundMachine.StartPlayingSound();      //TODO: Get sound foreach character
            
            nextLineButton.SetActive(false);
            
            CheckDialogueLineCameraBehavior(dialogueNode);
            //CheckDialogueLineHighlightBehavior(dialogueNode);
            PlaceSpeakerNameAndImage((DialogueSpeakerId)dialogueNode.SpeakerId);
            foreach (var letter in dialogueNode.DialogueLine)
            {
                //Check if player is hurrying Dialogue and we have more than 3 characters.
                if (Input.GetKey(KeyCode.Space) && mDialogueTextObject.text.Length > 3)
                {
                    mDialogueTextObject.text = dialogueNode.DialogueLine;
                    break;
                }
                
                //Manage the use of html tags in text
                if (letter == '<' || isAddingHtmlTag)
                {
                    isAddingHtmlTag = true;
                    mDialogueTextObject.text += letter;
                    if (letter == '>')
                    {
                        isAddingHtmlTag = false;
                    }
                }
                //If is not a tag add characters with Typing effect.
                else
                {
                    //Add characters one by one with a little random component
                    mDialogueTextObject.text += letter;
                    Random.InitState(DateTime.Now.Millisecond);
                    var typingSpeed = Random.Range(minWritingSpeed, maxWritingSpeed);
                    yield return new WaitForSeconds(typingSpeed);
                }
            }
            CheckDialogueLineChoiceBehavior(dialogueNode);
            nextLineButton.SetActive(true);
            _soundMachine.PausePlayingSound();
            _currentState = UIDialogueState.FinishedTypingLine;
        }
        #endregion

        private void PlaceSpeakerNameAndImage(DialogueSpeakerId dialogueNodeSpeakerId)
        {
            var speakerData = dialogueNodeSpeakerId == 0 ? _mOmnicorpDialogueInfo : GameDirector.Instance.GetSpeakerData(dialogueNodeSpeakerId);
            mSpeakerName.text = speakerData.SpeakerName;
        }

        private void CheckDialogueLineCameraBehavior(IDialogueNode dialogueNode)
        {
            if (!dialogueNode.HasCameraTarget)
            {
                return;
            }
            var viewType = dialogueNode.CameraEvent[0];
            Enum.TryParse(viewType, out GameCameraState cameraState);
            
            var cameraIndex = int.Parse(dialogueNode.CameraEvent[1]);
            GameCameraOperator.Instance.ActivateNewCamera(cameraState, cameraIndex);
            _mUIController.SyncUIStatusWithCameraState(cameraState, cameraIndex);
        }
        private void CheckDialogueLineEventBehavior(IDialogueNode dialogueNode)
        {
            if (!dialogueNode.HasEvent)
            {
                return;
            }
            _dialogueEventsOperator.HandleDialogueEvent(dialogueNode.EventCodes);
        }
        private void CheckDialogueLineChoiceBehavior(IDialogueNode dialogueNode)
        {
            if (!dialogueNode.HasChoice || dialogueNode.LinkNodes.Length <= 1)
            {
                Debug.Log($"Dialogue node {dialogueNode.DialogueLineIndex} has no Choice");
                decisionObjectsParent.SetActive(false);
                return;
            }
            SetDecisionButtons(dialogueNode);
        }

        private void CheckDialogueLineHighlightBehavior(IDialogueNode dialogueNode)
        {
            var isFeedbackActive = PopUpOperator.Instance.IsPopupActive(BitPopUpId.FEEDBACK_MASK);
            var newNodeHasHighlight = dialogueNode.HasHighlightEvent;

            if (isFeedbackActive && !newNodeHasHighlight)
            {
                PopUpOperator.Instance.RemovePopUp(BitPopUpId.FEEDBACK_MASK);
                return;
            }
            if(isFeedbackActive && newNodeHasHighlight)
            {
                try
                {
                    var activePopUp = (ITutorialMaskOperator)PopUpOperator.Instance.GetActivePopUp(BitPopUpId.FEEDBACK_MASK);
                    var areArraysEqual = AreArraysEqual(activePopUp.GetLastHighlight, dialogueNode.HighlightEvent);
                    if (areArraysEqual)
                    {
                        return;
                    }
                    activePopUp.SetHighlightState(dialogueNode.HighlightEvent);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                
            }
            if (!isFeedbackActive && newNodeHasHighlight)
            {
                var highlightPopUp = (ITutorialMaskOperator)PopUpOperator.Instance.ActivatePopUp(BitPopUpId.FEEDBACK_MASK);
                highlightPopUp.SetHighlightState(dialogueNode.HighlightEvent);
            }
 
            //1. Check If Highlight Already Active
            //2. Check If new Highlight is needed
            //3. Check if is same as previous
            //3.    If needed: Check if is equal to 
        }

        private bool AreArraysEqual(string[] array1, string[] array2)
        {
            return array1.SequenceEqual(array2);
        }

        private void SetDecisionButtons(IDialogueNode dialogueNode)
        {
            Debug.Log($"Dialogue node {dialogueNode.DialogueLineIndex} Initializing Choices");
            decisionObjectsParent.SetActive(true);
            dialogueChoicesOperator.DisplayDialogueChoices(dialogueNode, _currentDialogue, this);
        }

        private void LoadNewDialogue(IDialogueObject newDialogue)
        {
            if (newDialogue == null)
            {
                return;
            }
            _currentDialogue = newDialogue;
            if (_currentDialogue == null)
            {
                Debug.LogError("Current Dialogue must not be null after loading");
                return;
            }
        }
        #endregion
    }
}