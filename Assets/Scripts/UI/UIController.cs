using System.Collections.Generic;
using CameraManagement;
using DataUnits.GameCatalogues;
using DialogueSystem;
using DialogueSystem.Interfaces;
using GameDirection;
using GamePlayManagement;
using GamePlayManagement.BitDescriptions;
using GamePlayManagement.BitDescriptions.Suppliers;
using InputManagement;
using UI.PopUpManager;
using UnityEngine;
using Utils;

namespace UI
{
    [RequireComponent(typeof(DialogueOperator))]
    public class UIController : MonoBehaviour, IUIController
    {
        //Singleton Management to make sure only one UI controller is in scene
        private static UIController _instance;
        public static IUIController Instance => _instance;
        public delegate void ReturnToBaseCanvasState();
        public event ReturnToBaseCanvasState OnResetCanvas;

        private IInfoCanvasManager _mInfoCanvasManager;
        //Manage the activation or deactivation of the Dialogue UI Object and logic
        private IDialogueOperator _mDialogueOperator;
        public IDialogueOperator DialogueOperator => _mDialogueOperator;



        //This is the dictionary that holds each CANVAS operator.
        private Dictionary<int, ICanvasOperator> _mActiveCanvasDict;
        
        //
        private readonly List<int> _baseObjects = new List<int>() {BasePanelsBitStates.BASE_INFO,BasePanelsBitStates.IN_GAME_CLOCK,
            BasePanelsBitStates.IN_GAME_HELP_BUTTON};

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
                    canvasOperator.Value.ActivateThisElementsOnly(_baseObjects);
                    continue;
                }

                if (canvasOperator.Key == (int)CanvasBitId.GamePlayCanvas)
                {
                    if (canvasOperator.Value.IsElementActive(GameplayPanelsBitStates.BOTTOM_DIALOGUE))
                    {
                        var dialogueCanvas = new List<int>() { GameplayPanelsBitStates.BOTTOM_DIALOGUE };    
                        canvasOperator.Value.ActivateThisElementsOnly(dialogueCanvas);
                        continue;
                    }
                }
                canvasOperator.Value.DeactivateAllElements();
            }
            
            //Item Sidebar
            //var panelObjects = new List<int>() {GameplayPanelsBitStates.ITEM_SIDEBAR};
            //_mActiveCanvasDict[(int)CanvasBitId.GamePlayCanvas].ActivateThisElementsOnly(panelObjects);
            OnResetCanvas?.Invoke();
        }

        
        public void HiredInJobFoundFeedbackEvent(JobSupplierBitId newJobSupplier)
        {
            var jobSupplierName = BaseJobsCatalogue.Instance.GetJobSupplierObject(newJobSupplier).StoreName;
            var bannerText = $"New Job Found in: {jobSupplierName}";
            ShowFeedback(bannerText);
        }
        public void ElementUnlockedFeedback(BitItemSupplier itemSupplier)
        {
            if (GameDirector.Instance.GetCurrentHighLvlGameState == HighLevelGameStates.InCutScene)
            {
                return;
            }
            var jobSupplierName = BaseItemSuppliersCatalogue.Instance.GetItemSupplierData(itemSupplier).StoreName;
            var bannerText = $"Item Supplier Unlocked: {jobSupplierName}";
            ShowFeedback(bannerText);
        }



        private void ShowFeedback(string feedback)
        {
            var bannerObject = (IBannerObjectController)PopUpOperator.Instance.ActivatePopUp(BitPopUpId.LARGE_HORIZONTAL_BANNER);
            bannerObject.ToggleBannerForSeconds(feedback, 4);
        }
        
        #region Base Info Canvas Management
        public void InitializeBaseInfoCanvas(IPlayerGameProfile playerProfile)
        {
            _mInfoCanvasManager.Initialize(playerProfile);
        }

        public void UpdateInfoUI()
        {
            _mInfoCanvasManager.UpdateInfo();
        }
        public void SyncUIStatusWithCameraState(GameCameraState currentCameraState, int indexCamera)
        {
            ReturnToBaseGamePlayCanvasState();
            var indexBitValue = BitOperator.TurnIndexToBitIndexValue(indexCamera);
            switch (currentCameraState)
            {
                case GameCameraState.Office:
                    UpdateOfficeUIElement(indexBitValue);
                    break;
                case GameCameraState.Level:
                    UpdateLevelUIElements();
                    break;
            }
        }

        public bool IsObjectActive(CanvasBitId canvasBitId, int panel)
        {
            if (!_mActiveCanvasDict.ContainsKey((int)canvasBitId))
            {
                return false;
            }
            return _mActiveCanvasDict[(int)canvasBitId].IsElementActive(panel);
        }

        private void UpdateLevelUIElements()
        {
            if (!_mActiveCanvasDict.ContainsKey((int) CanvasBitId.GamePlayCanvas))
            {
                Debug.LogWarning("[UIController.ToggleDialogueObject] active canvas must contain GamePlay canvas id.");
                return;
            }

            var levelViewElements = new List<int>();
            if(_mActiveCanvasDict[(int)CanvasBitId.GamePlayCanvas].IsElementActive(GameplayPanelsBitStates.BOTTOM_DIALOGUE))
            { 
                levelViewElements = new List<int>()
                    {GameplayPanelsBitStates.ITEM_SIDEBAR, GameplayPanelsBitStates.BOTTOM_DIALOGUE};
            }
            else
            {
                levelViewElements = new List<int>() {GameplayPanelsBitStates.ITEM_SIDEBAR};
            }
            _mActiveCanvasDict[(int)CanvasBitId.GamePlayCanvas].ActivateThisElementsOnly(levelViewElements);
            PopUpOperator.Instance.RemoveAllPopUpsExceptList(new List<BitPopUpId>{BitPopUpId.LARGE_HORIZONTAL_BANNER, BitPopUpId.ACTION_TIMER_POPUP});
        }

        #endregion

        public void UpdateOfficeUIElement(int cameraState)
        {
            if (!_mActiveCanvasDict.ContainsKey((int) CanvasBitId.Office))
            {
                Debug.LogWarning("[UIController.ToggleDialogueObject] active canvas must contain Office canvas id.");
                return;
            }
            var bitList = new List<int>() {cameraState};
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
            IGeneralGameGameInputManager.Instance.OnGameStateChange += UpdatePauseState;
            _mInfoCanvasManager = FindObjectOfType<InfoCanvasManager>();
            SaveCanvasOperatorsDictionaries();
            _mDialogueOperator = GetComponent<DialogueOperator>();
        }
        private void SaveCanvasOperatorsDictionaries()
        {
            var activeCanvas = FindObjectsOfType<CanvasOperator>(includeInactive:true);
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
            IGeneralGameGameInputManager.Instance.OnGameStateChange -= UpdatePauseState;
        }

        #endregion
        
        #region Input Event Management
        private void UpdatePauseState(InputGameState newGameState)
        {
            if (newGameState == InputGameState.Pause ||
                IGeneralGameGameInputManager.Instance.CurrentInputGameState == InputGameState.Pause)
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