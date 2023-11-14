using System.Collections.Generic;
using CameraManagement;
using DataUnits.GameCatalogues;
using DialogueSystem.Interfaces;
using DialogueSystem.Units;
using GameDirection.Initial_Office_Scene;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement;
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
        [SerializeField] private List<BaseDialogueObject> introDialogues;
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
        private IClockManagement _mClockManagement;
        //Scriptable Objects Catalogues
        private IBaseItemDataCatalogue _mItemDataCatalogue;
        private IBaseJobsCatalogue _mJobsCatalogue;
        private IBaseItemSuppliersCatalogue _mItemSuppliersData;
        
        /// <summary>
        /// Initial Scene Manager
        /// </summary>
        private DialoguesInSceneDataManager _dialoguesInSceneDataManager;
        #endregion

        #region Public Fields
        public HighLevelGameStates GetCurrentHighLvlGameState => _mGameState;
        public IPlayerGameProfile GetActiveGameProfile => _mActiveGameProfile;
        public ILevelManager GetLevelManager => _mLevelManager;
        public IClockManagement GetClockInDayManagement => _mClockManagement;
        public IUIController GetUIController => _mUIController;
        public IGeneralUIFader GetGeneralFader => _mGeneralFader;
        public IGameCameraManager GetGameCameraManager => _mGameCameraManager;
        public IGeneralGameStateManager GetGameStateManager => _gameStateManager;
        public IDialogueOperator GetDialogueOperator => _mDialogueOperator;
        public ISoundDirector GetSoundDirector => _mSoundDirector;
        public IBaseItemDataCatalogue GetBaseItemDataCatalogue => _mItemDataCatalogue;
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
            _gameStateManager = GeneralInputStateManager.Instance;
            _mGameCameraManager = GameCameraManager.Instance;             
        }
        private void LoadUIScene()
        {
            _mLevelManager.LoadUIScene();
        }
        private void Start()
        {
            _mSoundDirector = SoundDirector.Instance;
            _mUIController = UIController.Instance;
            _mDialogueOperator = _mUIController.DialogueOperator;
            _mItemDataCatalogue = BaseItemCatalogue.Instance;
            _mJobsCatalogue = BaseJobsCatalogue.Instance;
            _mItemSuppliersData = BaseItemSuppliersCatalogue.Instance;
            _mClockManagement = ClockManagement.Instance;
            
            //TODO: CHANGE ARGS INJECTED INTO INTRO SCENE MANAGER
            var dialoguesInterface = _mDialogueOperator.GetDialogueObjectInterfaces(introDialogues);
            _dialoguesInSceneDataManager = gameObject.AddComponent<DialoguesInSceneDataManager>();
            _dialoguesInSceneDataManager.Initialize(this);
            _dialoguesInSceneDataManager.OnFinishCurrentDialogue += LoadFirstLevel;
            
            _mUIController.StartMainMenuUI();
            
            _gameStateManager.SetGamePlayState(InputGameState.MainMenu);
            _mGameState = HighLevelGameStates.MainMenu;
        }
        #endregion
        
        #region Public Fields
        
        #region ManageNewGame
        public void StartNewGame()
        {
            //_gameStateManager.SetGamePlayState(InputGameState.Pause);
            _mGeneralFader.GeneralCameraFadeOut();
            CreateNewProfile();
            _mGameState = HighLevelGameStates.InCutScene;
            StartCoroutine(_dialoguesInSceneDataManager.PrepareIntroductionReading());
        }

        private void CreateNewProfile()
        {
            _mActiveGameProfile = null;
            var itemSuppliersModule = Factory.CreateItemSuppliersModule(_mItemDataCatalogue, _mItemSuppliersData);
            var jobSourcesModule = Factory.CreateJobSourcesModule(_mJobsCatalogue);
            _mActiveGameProfile = Factory.CreatePlayerGameProfile(itemSuppliersModule,jobSourcesModule);
            _mActiveGameProfile.UpdateProfileData();
        }
        
        private void LoadFirstLevel()
        {
            ChangeHighLvlGameState(HighLevelGameStates.OfficeMidScene);
            _mLevelManager.LoadFirstLevel();
        }
        
        public void ReleaseFromDialogueStateToGame()
        {
            if (_gameStateManager.CurrentInputGameState != InputGameState.InDialogue)
            {
                return;
            }
            _gameStateManager.SetGamePlayState(InputGameState.InGame);
        }
        #endregion
        
        public void ChangeHighLvlGameState(HighLevelGameStates newState)
        {
            _mGameState = newState;
        }
        #endregion
    }
}