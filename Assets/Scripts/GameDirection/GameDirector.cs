using System;
using System.Collections;
using GameManagement;
using GameManagement.LevelManagement;
using GameManagement.ProfileDataModules.ItemStores.StoreInterfaces;
using UI;
using UnityEngine;

namespace GameDirection
{
    public enum HighLevelGameStates
    {
        MainMenu = 1,
        InGame = 2,
        Loading = 3,
        InCutScene = 4,
    }

    [RequireComponent(typeof(LevelManager))]
    [RequireComponent(typeof(GeneralUIFader))]
    public class GameDirector : MonoBehaviour, IGameDirector
    {
        private static GameDirector _mInstance;
        public static IGameDirector Instance => _mInstance;
        
        #region Members
        private HighLevelGameStates _mGameStates;
        private IPlayerGameProfile _mActiveGameProfile;
        private ILevelManager _mLevelManager;
        private IUIController _mUIController;
        private IGeneralUIFader _mGeneralFader;
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
        }
        private void LoadUIScene()
        {
            _mLevelManager.LoadUIScene();
        }
        private void Start()
        {
            _mUIController = UIController.Instance;
            _mUIController.StartMainMenuUI();
        }

        #endregion
        
        #region Public Fields
        public HighLevelGameStates GetCurrentState => _mGameStates;
        public IPlayerGameProfile GetActiveGameProfile => _mActiveGameProfile;
        public void StartNewGame()
        {
            _mGeneralFader.GeneralCameraFadeOut();
            _mActiveGameProfile = null;
            var newItemSources = new GeneralItemSource();
            _mActiveGameProfile = new PlayerGameProfile(newItemSources);
            StartCoroutine(LoadLevel());
        }

        private IEnumerator LoadLevel()
        {
            yield return new WaitForSeconds(5);
            _mLevelManager.LoadFirstLevel();
            _mGeneralFader.GeneralCameraFadeIn();
            _mUIController.ReturnToBaseGamePlayCanvasState();

        }
        
        public void ChangeHighLvlGameState(HighLevelGameStates newState)
        {
            _mGameStates = newState;
        }
        #endregion
    }
}