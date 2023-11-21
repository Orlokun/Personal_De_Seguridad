using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using DialogueSystem.Interfaces;
using DialogueSystem.Units;
using InputManagement;
using Players_NPC;
using UI.PopUpManager;
using UnityEngine;
using Utils;

namespace UI
{
    public enum CanvasBitId
    {
        BaseCanvas = 1,
        GamePlayCanvas = 2,
        Office = 4,
        MainMenu = 8
    }

    public interface IUIController
    {
        void DeactivateAllObjects();
        void StartMainMenuUI();
        event UIController.ReturnToBaseCanvasState OnResetCanvas;
        void ToggleDialogueObject(bool isActive);
        void ReturnToBaseGamePlayCanvasState();
        void UpdateOfficeUIElement(int cameraState);
        void ActivateObject(CanvasBitId canvasBitId, int panel);
        void DeactivateObject(CanvasBitId canvasBitId, int panel);
        void ToggleBackground(bool toggleValue);
        IDialogueOperator DialogueOperator { get; }
    }

    [RequireComponent(typeof(DialogueOperator))]
    public class UIController : MonoBehaviour, IUIController
    {
        //Singleton Management to make sure only one UI controller is in scene
        private static UIController _instance;
        public static UIController Instance => _instance;
        public delegate void ReturnToBaseCanvasState();
        public event ReturnToBaseCanvasState OnResetCanvas;
        
        //Manage the activation or deactivation of the Dialogue UI Object and logic
        private IDialogueOperator _mDialogueOperator;
        public IDialogueOperator DialogueOperator => _mDialogueOperator;

        //Tracking of canvas objects active objects        
        private Dictionary<int, ICanvasOperator> _mActiveCanvasDict;
        private int _activeCanvas = 0;
        
        [SerializeField] private DialogueWithCameraTarget testCameraDialogue;

        #region PublicFunction
        public void ToggleDialogueObject(bool isActive)
        {
            if (!_mActiveCanvasDict.ContainsKey((int) CanvasBitId.GamePlayCanvas))
            {
                Debug.LogWarning("[UIController.ToggleDialogueObject] active canvas must contain GamePlay canvas id.");
                return;
            }
            var objectsToggled = new List<int>(){GameplayPanelsBitStates.BOTTOM_DIALOGUE};
            if (isActive)
            {
                _mActiveCanvasDict[(int) CanvasBitId.GamePlayCanvas].ActivateUIElements(objectsToggled);
            }
            else
            {
                _mActiveCanvasDict[(int) CanvasBitId.GamePlayCanvas].DeactivateUIElements(objectsToggled);
            }
        }
        public void ReturnToBaseGamePlayCanvasState()
        {
            foreach (var canvasOperator in _mActiveCanvasDict)
            {
                if (canvasOperator.Key == (int) CanvasBitId.BaseCanvas)
                {
                    var baseObjects = new List<int>() {BasePanelsBitStates.BASE_INFO,BasePanelsBitStates.IN_GAME_CLOCK,
                        BasePanelsBitStates.IN_GAME_HELP_BUTTON};
                    canvasOperator.Value.ActivateThisElementsOnly(baseObjects);
                    continue;
                }
                canvasOperator.Value.DeactivateAllElements();
            }
            
            //Item Sidebar and clock
            var panelObjects = new List<int>() {GameplayPanelsBitStates.ITEM_SIDEBAR};
            _mActiveCanvasDict[(int)CanvasBitId.GamePlayCanvas].ActivateThisElementsOnly(panelObjects);
            OnResetCanvas?.Invoke();
        }
        public void UpdateOfficeUIElement(int cameraState)
        {
            var indexBitValue = BitOperator.TurnIndexToBitIndexValue(cameraState);
            if (!_mActiveCanvasDict.ContainsKey((int) CanvasBitId.Office))
            {
                Debug.LogWarning("[UIController.ToggleDialogueObject] active canvas must contain Office canvas id.");
                return;
            }
            var bitList = new List<int>() {indexBitValue};
            _mActiveCanvasDict[(int)CanvasBitId.Office].ActivateThisElementsOnly(bitList);
            PopUpOperator.Instance.RemoveAllPopUpsExceptOne(BitPopUpId.LARGE_HORIZONTAL_BANNER);
        }
        public void ActivateObject(CanvasBitId canvasBitId, int panel)
        {
            if (!_mActiveCanvasDict.ContainsKey((int) canvasBitId))
            {
                return;
            }
            _mActiveCanvasDict[(int)canvasBitId].ActivateUIElement(panel);
        }
        public void DeactivateObject(CanvasBitId canvasBitId, int panel)
        {
            if (!_mActiveCanvasDict.ContainsKey((int) canvasBitId))
            {
                return;
            }
            _mActiveCanvasDict[(int)canvasBitId].DeactivateUIElement(panel);
        }
        public void DeactivateAllObjects()
        {
            foreach (var canvasOperator in _mActiveCanvasDict)
            {
                canvasOperator.Value.DeactivateAllElements();
            }
        }
        public void StartMainMenuUI()
        {
            foreach (var canvasOperator in _mActiveCanvasDict)
            {
                if (canvasOperator.Key != (int)CanvasBitId.MainMenu)
                {
                    canvasOperator.Value.DeactivateAllElements();
                    continue;
                }
                var menuPanels = new List<int>()
                    {MainMenuPanelsBitStates.BG_PANEL, MainMenuPanelsBitStates.MAIN_OPTIONS};
                canvasOperator.Value.ActivateThisElementsOnly(menuPanels);
            }
        }
        public void ToggleBackground(bool toggleValue)
        {
            var canvasOperator = _mActiveCanvasDict[(int) CanvasBitId.GamePlayCanvas];
            if (!toggleValue)
            {
                canvasOperator.DeactivateUIElement(GameplayPanelsBitStates.TEXT_BACKGROUND);
            }
            else
            {
                canvasOperator.ActivateUIElement(GameplayPanelsBitStates.TEXT_BACKGROUND);
            }

        }
        #endregion
        
        #region Init
        private void Awake()
        {
            SingletonAwake();
            LoadInitialVariables();
            DontDestroyOnLoad(this);
        }
        private IEnumerator ManageTestDialogue()
        {
            yield return new WaitForSeconds(4);
            //ActivateUIElements(new List<int>(){BaseCanvasBitStates.BASE_INFO});
            var guardObject = FindObjectOfType<TestGuardNavigation>().gameObject.transform;
            testCameraDialogue.MyTargetsInDialogues[0].Value = guardObject;
            _mDialogueOperator.StartNewDialogue(testCameraDialogue);
        }
        private void Start()
        {
            //Makes sure no Dialogue is active
            //ReturnToBaseGamePlayCanvasState();
            
            //GeneralFadeOutPanelAnim.gameObject.SetActive(true);
            
            //Test Dialogue. TODO: Remove
            //StartCoroutine(ManageTestDialogue());
        }
        private void SingletonAwake()
        {
            if (_instance != null)
            {
                Debug.LogWarning("Only one UIController must be present in scene");
                Destroy(this);
                return;
            }
            _instance = this;
        }
        private void LoadInitialVariables()
        {
            GeneralInputStateManager.Instance.OnGameStateChange += UpdateInputState;
            SaveCanvasOperatorsDictionaries();
            _mDialogueOperator = GetComponent<DialogueOperator>();
        }
        private void SaveCanvasOperatorsDictionaries()
        {
            var activeCanvas = FindObjectsOfType<CanvasOperator>();
            _mActiveCanvasDict = new Dictionary<int, ICanvasOperator>();
            foreach (var canvas in activeCanvas)
            {
                _mActiveCanvasDict.Add((int)canvas.GetCanvasID, canvas);
            }
            foreach (var canvasOperator in _mActiveCanvasDict)
            {
                canvasOperator.Value.SavePanelsDictData();
            }
        }
        #endregion
        
        #region Disable
        private void OnDestroy()
        {
            GeneralInputStateManager.Instance.OnGameStateChange -= UpdateInputState;
        }

        #endregion
        
        #region Private Utilities


        private void UpdateInputState(InputGameState newGameState)
        {
            if (newGameState == InputGameState.Pause ||
                GeneralInputStateManager.Instance.CurrentInputGameState == InputGameState.Pause)
            {
                TogglePausePanel();
            }
        }
        private void TogglePausePanel()
        {
            _mActiveCanvasDict[(int)CanvasBitId.BaseCanvas].ToggleUIElement(BasePanelsBitStates.PAUSE_MENU);
        }
        #endregion
    }
}