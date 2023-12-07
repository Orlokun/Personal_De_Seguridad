using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CameraManagement;
using DialogueSystem.Interfaces;
using DialogueSystem.Sound;
using DialogueSystem.Units;
using InputManagement;
using TMPro;
using UI;
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
        FinishedTypingBaseDialogue,
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
        private IDialogueChoicesGameOperator dialogueChoicesOperator => _dialogueChoiceOperator;

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
        private const string DECISION_PREFAB = "DecisionUIObject";
        private const string UI_RESOURCES_PATH = "UI/";
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
        public void LoadSupplierDialogues(IEnumerator coroutine)
        {
            Debug.Log("[DIALOGUE OPERATOR] Starting Co routine for Supplier Dialogues Data");
            StartCoroutine(coroutine);
        }
        public void StartNewDialogue(IDialogueObject newDialogue)
        {
            _currentState = UIDialogueState.TypingText;
            Debug.Log("Starting new Dialogue");
            LoadNewDialogue(newDialogue);
            
            _mUIController.ToggleDialogueObject(true);
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
            GameCameraManager.Instance.ReturnToLastState();
            _mUIController.ToggleDialogueObject(false);
            mDialogueTextObject.text = "";
            //GeneralGamePlayStateManager.Instance.SetGamePlayState(InputGameState.InGame);
            KillDialogue();
        }
        private IEnumerator FinishDialogue()
        {
            while (CurrentDialogueState != UIDialogueState.FinishedTypingBaseDialogue)
            {
                yield return null;
            }
        }

        public void KillDialogue()
        {
            _currentState = UIDialogueState.NotDisplayed;
            _currentDialogue = null;
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
            _soundMachine = GetComponent<DialogueOperatorSoundMachine>();
            OnDialogueCompleted += OnDialogueFinished;
        }
        private void Start()
        {
            _mUIController = UIController.Instance;
        }
        #endregion

        #region PrivateUtils
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
                   && GeneralInputStateManager.Instance.CurrentInputGameState != InputGameState.Pause;
        }

        private bool NextChoiceWaitingConditions()
        {
            return _currentDecisionMade;
        }

        private IEnumerator WriteDialogueLine(IDialogueNode dialogueNode)
        {
            _currentState = UIDialogueState.TypingText;
            var isAddingHtmlTag = false;            //Used to manage tags inside text so they don't appear in the UI.
            
            _soundMachine.StartPlayingSound();      //TODO: Get sound foreach character
            
            nextLineButton.SetActive(false);
            
            CheckDialogueLineCameraBehavior(dialogueNode);
            
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
            CheckDialogueLineEventBehavior(dialogueNode);
            CheckDialogueLineChoiceBehavior(dialogueNode);
            nextLineButton.SetActive(true);
            _soundMachine.PausePlayingSound();
            _currentState = UIDialogueState.FinishedTypingLine;
        }

        private void CheckDialogueLineCameraBehavior(IDialogueNode dialogueNode)
        {
            if (!dialogueNode.HasCameraTarget)
            {
                return;
            }
            //TODO: Move camera towards target;
        }
        private void CheckDialogueLineEventBehavior(IDialogueNode dialogueNode)
        {
            if (!dialogueNode.HasEvent)
            {
                return;
            }
            //TODO: Launch Event
        }
        private void CheckDialogueLineChoiceBehavior(IDialogueNode dialogueNode)
        {
            if (!dialogueNode.HasChoice || dialogueNode.LinkNodes.Length <= 1)
            {
                Debug.Log($"Dialogue node {dialogueNode.DialogueLineIndex} has no Choice");
                _waitingForChoice = false;
                _currentDecisionMade = _waitingForChoice;
                decisionObjectsParent.SetActive(_waitingForChoice);
                return;
            }
            SetDecisionButtons(dialogueNode);
        }

        private void SetDecisionButtons(IDialogueNode dialogueNode)
        {
            Debug.Log($"Dialogue node {dialogueNode.DialogueLineIndex} Initializing Choices");
            _waitingForChoice = true;
            _currentDecisionMade = !_waitingForChoice;
            decisionObjectsParent.SetActive(_waitingForChoice);
            dialogueChoicesOperator.DisplayDialogueChoices(dialogueNode, _currentDialogue, this);
        }
        
        private bool _currentDecisionMade;
        public void MakeDecision(string decisionId)
        {
            _currentDecisionMade = true;
            //TODO: Process decision

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