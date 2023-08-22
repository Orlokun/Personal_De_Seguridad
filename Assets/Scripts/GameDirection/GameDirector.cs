using System.Collections;
using System.Collections.Generic;
using CameraManagement;
using DataUnits.GameCatalogues;
using DialogueSystem.Interfaces;
using DialogueSystem.Units;
using GameDirection.Initial_Office_Scene;
using GamePlayManagement;
using GamePlayManagement.BitDescriptions.Suppliers;
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
        
        //Scriptable Objects Catalogues
        private IBaseItemDataCatalogue _mItemDataCatalogue;
        private IBaseJobsCatalogue _mJobsCatalogue;
        private IBaseItemSuppliersCatalogue _mSuppliersCatalogue;
        
        
        private IntroSceneManager _introSceneManager;
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
            _gameStateManager = GeneralGamePlayStateManager.Instance;
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
            _mItemDataCatalogue = BaseItemDataCatalogue.Instance;
            _mJobsCatalogue = BaseJobsCatalogue.Instance;
            _mSuppliersCatalogue = BaseItemSuppliersCatalogue.Instance;
            //TODO: CHANGE ARGS INJECTED INTO INTRO SCENE MANAGER
            var dialoguesInterface = _mDialogueOperator.GetDialogueObjects(introDialogues);
            _introSceneManager = gameObject.AddComponent<IntroSceneManager>();
            _introSceneManager.Initialize(this, dialoguesInterface);
            
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
            CreateNewProfile();
            _mGameState = HighLevelGameStates.InCutScene;
            StartCoroutine(PrepareIntroductionReading());
        }

        private void CreateNewProfile()
        {
            _mActiveGameProfile = null;
            var itemSuppliersModule = Factory.CreateItemSuppliersModule(_mItemDataCatalogue, _mSuppliersCatalogue);
            var jobSourcesModule = Factory.CreateJobSourcesModule(_mJobsCatalogue);
            jobSourcesModule.AddJobToModule(BitGameJobSuppliers.LOCAl_DE_BARRIO);
            
            //Activate Suppliers
            itemSuppliersModule.ActivateSupplier(BitItemSupplier.MONCHITO);
            itemSuppliersModule.ActivateSupplier(BitItemSupplier.TERCER_AIRE);
            
            //Activate Items in Suppliers
            itemSuppliersModule.AddItemToSupplier(BitItemSupplier.MONCHITO, (int)MonchitoItemsBitId.WEB_CAM);
            itemSuppliersModule.AddItemToSupplier(BitItemSupplier.MONCHITO, (int)MonchitoItemsBitId.ENCINA);
            itemSuppliersModule.AddItemToSupplier(BitItemSupplier.TERCER_AIRE, (int)TercerAireItemsBitId.DON_RONBINSON);
            itemSuppliersModule.AddItemToSupplier(BitItemSupplier.TERCER_AIRE, (int)TercerAireItemsBitId.ORYAN);
            _mActiveGameProfile = Factory.CreatePlayerGameProfile(itemSuppliersModule,jobSourcesModule);
        }
        private IEnumerator PrepareIntroductionReading()
        {
            yield return new WaitForSeconds(2f);
            StartCoroutine(_introSceneManager.PrepareIntroductionReading());
        }
        #endregion
        
        public void ChangeHighLvlGameState(HighLevelGameStates newState)
        {
            _mGameState = newState;
        }
        #endregion
    }
}