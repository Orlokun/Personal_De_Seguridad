using System.Collections;
using CameraManagement;
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
        private static GameDirector _mInstance;
        public static IGameDirector Instance => _mInstance;
        
        #region Members
        private HighLevelGameStates _mGameState;
        private IPlayerGameProfile _mActiveGameProfile;
        private ILevelManager _mLevelManager;
        private IUIController _mUIController;
        private IGeneralUIFader _mGeneralFader;
        private IGameCameraManager _mGameCameraManager;
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
            _mGameCameraManager = GameCameraManager.Instance;
        }
        private void LoadUIScene()
        {
            _mLevelManager.LoadUIScene();
        }
        private void Start()
        {
            _mUIController = UIController.Instance;
            _mUIController.StartMainMenuUI();
            
            GeneralGamePlayStateManager.Instance.SetGamePlayState(InputGameState.MainMenu);
            _mGameState = HighLevelGameStates.MainMenu;
        }

        #endregion
        
        #region Public Fields
        public HighLevelGameStates GetCurrentState => _mGameState;
        public IPlayerGameProfile GetActiveGameProfile => _mActiveGameProfile;
        
        public void StartNewGame()
        {
            _mGeneralFader.GeneralCameraFadeOut();
            _mActiveGameProfile = null;
            var newItemSources = new GeneralItemSource();
            _mActiveGameProfile = new PlayerGameProfile(newItemSources);
            StartCoroutine(LoadOfficeLevel());
        }

        private IEnumerator LoadOfficeLevel()
        {
            yield return new WaitForSeconds(5);
            _mGameState = HighLevelGameStates.OfficeMidScene;
            _mLevelManager.LoadOfficeLevel();
            _mGeneralFader.GeneralCameraFadeIn();
            _mUIController.ReturnToBaseGamePlayCanvasState();
            GeneralGamePlayStateManager.Instance.SetGamePlayState(InputGameState.InGame);
        }
        
        public void ChangeHighLvlGameState(HighLevelGameStates newState)
        {
            _mGameState = newState;
        }
        #endregion
    }
}