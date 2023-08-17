using System.Collections;
using CameraManagement;
using DialogueSystem.Interfaces;
using DialogueSystem.Units;
using GameManagement;
using GameManagement.ProfileDataModules.ItemStores.StoreInterfaces;
using GamePlayManagement.LevelManagement;
using InputManagement;
using UI;
using UnityEngine;
using Utils;

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
        private ISoundDirector _mSoundDirector;

        // Introduction Scene / FTUE
        private IntroductionSceneManager _introductionSceneManager;
        #endregion

        #region Public Fields
        public HighLevelGameStates GetCurrentHighLvlGameState => _mGameState;
        public IPlayerGameProfile GetActiveGameProfile => _mActiveGameProfile;
        public ILevelManager GetLevelManager => _mLevelManager;
        public IUIController GetUIController => _mUIController;
        public IGeneralUIFader GetGeneralFader => _mGeneralFader;
        public IGameCameraManager GetGameCameraManager => _mGameCameraManager;
        public IGeneralGameStateManager GetGameStateManager => _gameStateManager;
        public IDialogueOperator GetDialogueOperator => _mDialogueOperator;
        public ISoundDirector GetSoundDirector => _mSoundDirector;
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
            _mSoundDirector = SoundDirector.Instance;
        }
        private void LoadUIScene()
        {
            _mLevelManager.LoadUIScene();
        }
        private void Start()
        {
            _mUIController = UIController.Instance;
            _mDialogueOperator = _mUIController.DialogueOperator;
            _mSoundDirector = SoundDirector.Instance;

            //TODO: CHANGE ARGS INJECTED INTO INTRO SCENE MANAGER
            _introductionSceneManager = gameObject.AddComponent<IntroductionSceneManager>();
            _introductionSceneManager.Initialize(this, introDialogue);
            
            _mUIController.StartMainMenuUI();
            
            GeneralGamePlayStateManager.Instance.SetGamePlayState(InputGameState.MainMenu);
            _mGameState = HighLevelGameStates.MainMenu;
        }

        #endregion
        
        #region Public Fields
        
        #region ManageNewGame
        public void StartNewGame()
        {
            //_gameStateManager.SetGamePlayState(InputGameState.Pause);
            _mGeneralFader.GeneralCameraFadeOut();
            _mActiveGameProfile = null;
            var newItemSources = Factory.CreateGeneralItemSource();
            _mActiveGameProfile = Factory.CreatePlayerGameProfile(newItemSources);
            _mGameState = HighLevelGameStates.InCutScene;
            StartCoroutine(PrepareIntroductionReading());
        }
        private IEnumerator PrepareIntroductionReading()
        {
            yield return new WaitForSeconds(2f);
            StartCoroutine(_introductionSceneManager.PrepareIntroductionReading());
        }
        #endregion
        
        public void ChangeHighLvlGameState(HighLevelGameStates newState)
        {
            _mGameState = newState;
        }
        #endregion
    }
}