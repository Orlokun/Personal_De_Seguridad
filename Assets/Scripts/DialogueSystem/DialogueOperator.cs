using System;
using System.Collections;
using CameraManagement;
using DialogueSystem.Interfaces;
using DialogueSystem.Units;
using InputManagement;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Utils;
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
        /// <summary>
        /// Reference of UI Editor Components
        /// </summary>
        #region Serialized Members
        [SerializeField] private Image mSpeakerImageDisplay;
        [SerializeField] private TMP_Text mDialogueTextObject;
        [SerializeField] private TMP_Text mSpeakerName;
        [SerializeField] private GameObject nextLineButton;
        [SerializeField] private Transform decisionObjectsParent;
        
        //Parameters to simply randomize the writing speed a Bit. 
        [SerializeField] private float maxWritingSpeed;
        [SerializeField] private float minWritingSpeed;
        #endregion

        #region Members
        private delegate void FinishedDialogueReading();
        private event FinishedDialogueReading OnDialogueCompleted;
        /// <summary>
        /// Private Memebers and dependencies
        /// </summary>

        private UIDialogueState _currentState;
        public UIDialogueState CurrentDialogueState => _currentState;

        private IDialogueObject _currentDialogue;
        private IDialogueCameraMan _dialogueCameraMan;
        private Coroutine _typingMachineCoroutine;
        private IUIController _mUIController;
        private const string DECISION_PREFAB = "DecisionUIObject";
        private const string UI_RESOURCES_PATH = "UI/";
        #endregion

        #region Public Interface
        public void StartNewDialogue(IDialogueObject newDialogue)
        {
            Debug.Log("Starting new Dialogue");
            LoadNewDialogue(newDialogue);
            
            _mUIController.ToggleDialogueObject(true);
            StartCoroutine(WriteDialogue());
        }

        private void OnDialogueFinished()
        {
            if(_currentDialogue.ContainsBehavior(DialogueBehaviors.DialogueWithChoice))
            {
                DisplayDialogueChoice();
            }
            GameCameraManager.Instance.ReturnToLastState();
            _mUIController.ToggleDialogueObject(false);
            GeneralGamePlayStateManager.Instance.SetGamePlayState(InputGameState.InGame);
            KillDialogue();
        }

        private IEnumerator FinishDialogue()
        {
            while (CurrentDialogueState != UIDialogueState.FinishedTypingBaseDialogue)
            {
                yield return null;
            }
            
        }

        private void DisplayDialogueChoice()
        {
            var iDecision = (IDialogueDecision) _currentDialogue;
            foreach (var decisionText in iDecision.DecisionPossibilities)
            {
                var decisionObject = (GameObject)Instantiate(Resources.Load(UI_RESOURCES_PATH + DECISION_PREFAB), decisionObjectsParent);
                TMP_Text decisionTextObject;
                for (int i = 0; i<decisionObject.transform.childCount;i++)
                {
                    if (decisionObject.transform.GetChild(i).TryGetComponent(out decisionTextObject))
                    {
                        decisionTextObject.text = decisionText;
                    }
                }
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
            _dialogueCameraMan = Factory.CreateCameraMan();
            OnDialogueCompleted += OnDialogueFinished;
        }
        private void Start()
        {
            _mUIController = UIController.Instance;
        }
        #endregion

        #region PrivateUtils
        private IEnumerator WriteDialogue()
        {
            //Go through each of the dialogue lines
            for(int i = 0; i < _currentDialogue.DialogueLines.Count; i++)
            {
                //Manages Camera movement for line
                _dialogueCameraMan.ManageDialogueCameraBehavior(_currentDialogue, i);
                
                //Make Sure no other line is being written at the moment
                if (_typingMachineCoroutine != null)
                {
                    StopCoroutine(_typingMachineCoroutine);
                }

                //Start Typewriter effect of the current line    
                mDialogueTextObject.text = "";
                _typingMachineCoroutine = StartCoroutine(WriteDialogueLine(_currentDialogue.DialogueLines[i]));
                
                //Wait for the conditions to be met in order to continue the loop
                yield return new WaitUntil(NextLineWritingConditions);
                //The following line of code makes the coroutine wait for a frame so as the next WaitUntil is not skipped
                yield return null;
            }
            OnDialogueCompleted?.Invoke();
        }

        private bool NextLineWritingConditions()
        {
            return Input.GetKeyDown(KeyCode.Space) 
                   && _currentState == UIDialogueState.FinishedTypingLine 
                   && GeneralGamePlayStateManager.Instance.CurrentInputGameState != InputGameState.Pause;
        }
        private IEnumerator WriteDialogueLine(string dialogueLine)
        {
            _currentState = UIDialogueState.TypingText;
            var isAddingHtmlTag = false;
            nextLineButton.SetActive(false);

            foreach (var letter in dialogueLine)
            {
                //Check if player is hurrying Dialogue and we have more than 3 characters.
                if (Input.GetKey(KeyCode.Space) && mDialogueTextObject.text.Length > 3)
                {
                    mDialogueTextObject.text = dialogueLine;
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
                    var typingSpeed = Random.Range(minWritingSpeed, maxWritingSpeed);
                    yield return new WaitForSeconds(typingSpeed);
                }

            }
            nextLineButton.SetActive(true);
            _currentState = UIDialogueState.FinishedTypingLine;
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
                return;
            }
            mSpeakerImageDisplay.sprite = _currentDialogue.ActorImage;
            mSpeakerName.text = _currentDialogue.SpeakerName;

            if (_currentDialogue.ContainsBehavior(DialogueBehaviors.DialogueWithChoice))
            {
                
            }
            
            if (_currentDialogue.ContainsBehavior(DialogueBehaviors.DialogueWithCamera))
            {
                GeneralGamePlayStateManager.Instance.SetGamePlayState(InputGameState.InDialogue);
            }
        }
        #endregion
    }
}