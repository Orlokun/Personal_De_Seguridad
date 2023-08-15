using System.Collections;
using CameraManagement;
using DialogueSystem.Interfaces;
using DialogueSystem.Units;
using GameManagement;
using GameManagement.LevelManagement;
using GameManagement.ProfileDataModules.ItemStores.StoreInterfaces;
using InputManagement;
using UI;
using UnityEngine;

namespace GameDirection
{
    public enum HighLevelGameStates
    {
        MainMenu = 1,
        InGame = 2,
        Loading = 4,
        InCutScene = 8,
        OfficeMidScene = 16
    }

    [RequireComponent(typeof(LevelManager))]
    [RequireComponent(typeof(GeneralUIFader))]
    public class GameDirector : MonoBehaviour, IGameDirector
    {
        /////////////////////////////
        //TODO: DELETE - JUST FOR TEST
        [SerializeField] private BaseDialogueObject introDialogue;
        //TODO: END TODO:
        /// <summary>
        /// ////////////////////////////////////////////
        /// </summary>
        
        private static GameDirector _mInstance;
        public static IGameDirector Instance => _mInstance;
        
        #region Members
        private HighLevelGameStates _mGameState;
        private IPlayerGameProfile _mActiveGameProfile;
        private ILevelManager _mLevelManager;
        private IUIController _mUIController;
        private IGeneralUIFader _mGeneralFader;
        private IGameCameraManager _mGameCameraManager;
        private IGeneralGameStateManager _gameStateManager;
        private IDialogueOperator _mDialogueOperator;
        #endregion

        #region Init
        private void Awake()
        {
            DontDestroyOnLoad(this);
            SingletonManagement();
            SetComponentManagers();
            LoadUIScene();
        }
        private void SingletonManagement()
        {
            if (_mInstance != null)
            {
                Destroy(this);
            }
            _mInstance = this;
        }
        private void SetComponentManagers()
        {
            _mLevelManager = GetComponent<LevelManager>();
            _mGeneralFader = GetComponent<GeneralUIFader>();
            _gameStateManager = GeneralGamePlayStateManager.Instance;
            _mGameCameraManager = GameCameraManager.Instance;
        }
        private void LoadUIScene()
        {
            _mLevelManager.LoadUIScene();
        }
        private void Start()
        {
            _mUIController = UIController.Instance;
            _mDialogueOperator = _mUIController.DialogueOperator;
            _mUIController.StartMainMenuUI();
            
            GeneralGamePlayStateManager.Instance.SetGamePlayState(InputGameState.MainMenu);
            _mGameState = HighLevelGameStates.MainMenu;
        }

        #endregion
        
        #region Public Fields
        public HighLevelGameStates GetCurrentState => _mGameState;
        public IPlayerGameProfile GetActiveGameProfile => _mActiveGameProfile;

        #region ManageNewGame
        public void StartNewGame()
        {
            //_gameStateManager.SetGamePlayState(InputGameState.Pause);
            _mGeneralFader.GeneralCameraFadeOut();
            _mActiveGameProfile = null;
            var newItemSources = new GeneralItemSource();
            _mActiveGameProfile = new PlayerGameProfile(newItemSources);
            StartCoroutine(PrepareIntroductionReading());
        }

        private IEnumerator PrepareIntroductionReading()
        {
            yield return new WaitForSeconds(2f);
            _mUIController.DeactivateAllObjects();
            _mUIController.ToggleBackground(true);
            _mGameState = HighLevelGameStates.InCutScene;
            _mGeneralFader.GeneralCameraFadeIn();
            _mDialogueOperator.OnDialogueCompleted += FinishIntroductionText;
            StartCoroutine(StartIntroductionReading());
        }

        private IEnumerator StartIntroductionReading()
        {
            yield return new WaitForSeconds(2f);
            _mDialogueOperator.StartNewDialogue(introDialogue);
        }
        
        private void FinishIntroductionText()
        {
            _mGeneralFader.GeneralCameraFadeOut();
            StartCoroutine(FinishIntroductionReading());
        }
        private IEnumerator FinishIntroductionReading()
        {
            yield return new WaitForSeconds(2f);
            _mLevelManager.LoadOfficeLevel();
            _mUIController.ToggleBackground(false);
            _mGeneralFader.GeneralCameraFadeIn();
            _mDialogueOperator.OnDialogueCompleted -= FinishIntroductionText;
            GeneralGamePlayStateManager.Instance.SetGamePlayState(InputGameState.InGame);
        }
        #endregion
        public void ChangeHighLvlGameState(HighLevelGameStates newState)
        {
            _mGameState = newState;
        }
        #endregion
    }
}