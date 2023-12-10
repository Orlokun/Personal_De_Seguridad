using System.Collections;
using System.Threading.Tasks;
using CameraManagement;
using DataUnits.GameCatalogues;
using DialogueSystem.Interfaces;
using GameDirection.DayLevelSceneManagers;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement;
using GamePlayManagement.BitDescriptions;
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
        OfficeMidScene = 16,
        EndOfDay = 32
    }

    [RequireComponent(typeof(LevelLoadManager))]
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
        private IGeneralInputStateManager _inputStateManager;
        private IDialogueOperator _mDialogueOperator;
        private ISoundDirector _mSoundDirector;
        private IClockManagement _mClockManager;
        private IFeedbackManager _mFeedbackManager;
        private IGeneralInputStateManager _mGeneralInputManager;
        
        //Scriptable Objects Catalogues
        private IItemsDataController _mItemDataController;
        private IBaseJobsCatalogue _mJobsCatalogue;
        private IBaseItemSuppliersCatalogue _mItemSuppliersData;
        
        private IRentValuesCatalogue _mRentCatalogueData;
        private IFoodValuesCatalogue _mFoodCatalogueData;
        private ITransportValuesCatalogue _mTransportCatalogueData;
        
        /// <summary>
        /// Initial Scene Manager
        /// </summary>
        private ILevelDayManager _dayLevelManager;
        #endregion

        #region Public Fields
        public HighLevelGameStates GetCurrentHighLvlGameState => _mGameState;
        public IPlayerGameProfile GetActiveGameProfile => _mActiveGameProfile;
        public ILevelManager GetLevelManager => _mLevelManager;
        public IClockManagement GetClockInDayManagement => _mClockManager;
        public IFeedbackManager GetFeedbackManager => _mFeedbackManager;
        public IUIController GetUIController => _mUIController;
        public IGeneralUIFader GetGeneralBackgroundFader => _mGeneralFader;
        public IGameCameraManager GetGameCameraManager => _mGameCameraManager;
        public IGeneralInputStateManager GetInputStateManager => _inputStateManager;
        public IDialogueOperator GetDialogueOperator => _mDialogueOperator;
        public ISoundDirector GetSoundDirector => _mSoundDirector;
        public IItemsDataController GetItemsDataController => _mItemDataController;
        public IRentValuesCatalogue GetRentCatalogueData => _mRentCatalogueData;

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
            _mLevelManager = GetComponent<LevelLoadManager>();
            _mGeneralFader = GetComponent<GeneralUIFader>();
            _inputStateManager = GeneralInputStateManager.Instance;
        }
        private void LoadUIScene()
        {
            _mLevelManager.LoadAdditiveLevel(LevelIndexId.UILvl);
        }
        private void Start()
        {
            _mSoundDirector = SoundDirector.Instance;
            _mUIController = UIController.Instance;
            _mDialogueOperator = _mUIController.DialogueOperator;
            _mItemDataController = ItemsDataController.Instance;
            _mJobsCatalogue = BaseJobsCatalogue.Instance;
            _mItemSuppliersData = BaseItemSuppliersCatalogue.Instance;
            _mClockManager = ClockManagement.Instance;
            _mFeedbackManager = FeedbackManager.Instance;
            _mGeneralInputManager = GeneralInputStateManager.Instance;
            _mGameCameraManager = GameCameraManager.Instance;     
            
            _mRentCatalogueData = RentValuesCatalogue.Instance;
            _mFoodCatalogueData = FoodValuesCatalogue.Instance;
            _mTransportCatalogueData = TransportValuesCatalogue.Instance;
            
            //TODO: CHANGE ARGS INJECTED INTO INTRO SCENE MANAGER
            
            _mUIController.StartMainMenuUI();
            
            _inputStateManager.SetGamePlayState(InputGameState.MainMenu);
            _mGameState = HighLevelGameStates.MainMenu;
            WaitAndLoadDialogues();
        }

        private void LoadDayManagement(DayBitId dayId)
        {
            _dayLevelManager =  Factory.CreateLevelDayManager(dayId);
            _dayLevelManager.Initialize(this, dayId);
        }

        private async void WaitAndLoadDialogues()
        {
            await Task.Delay(300);
            LoadDialoguesForSuppliers();
        }
        private void LoadDialoguesForSuppliers()
        {
            foreach (var itemSupplier in _mItemSuppliersData.GetItemSuppliersInData)
            {
                itemSupplier.LoadDeflectionsDialogueData();
            }
            foreach (var jobSupplier in _mJobsCatalogue.JobSuppliersInData)
            {
                jobSupplier.LoadDeflectionDialoguesData();
            }
        }


        #endregion
        
        #region Public Functions
        
        #region ManageNewGame
        public void StartNewGame()
        {
            //_gameStateManager.SetGamePlayState(InputGameState.Pause);
            _mGeneralFader.GeneralCurtainAppear();
            CreateNewProfile();
            LoadDayManagement(_mActiveGameProfile.GetProfileCalendar().GetCurrentWorkDayObject().BitId);
            _mGameState = HighLevelGameStates.InCutScene;
            StartCoroutine(_dayLevelManager.StartDayManagement());
        }
        private void CreateNewProfile()
        {
            _mActiveGameProfile = null;
            var itemSuppliersModule = Factory.CreateItemSuppliersModule(_mItemDataController, _mItemSuppliersData);
            var jobSourcesModule = Factory.CreateJobSourcesModule(_mJobsCatalogue);
            var calendarModule = Factory.CreateCalendarModule(_mClockManager);
            var lifestyleModule =
                Factory.CreateLifestyleModule(_mRentCatalogueData, _mFoodCatalogueData, _mTransportCatalogueData);
            _mActiveGameProfile = Factory.CreatePlayerGameProfile(itemSuppliersModule,jobSourcesModule,calendarModule,lifestyleModule);
            _mActiveGameProfile.UpdateProfileData();
        }

        public void ReleaseFromDialogueStateToGame()
        {
            if (_inputStateManager.CurrentInputGameState != InputGameState.InDialogue)
            {
                return;
            }
            _inputStateManager.SetGamePlayState(InputGameState.InGame);
        }
        #endregion
        public void ChangeHighLvlGameState(HighLevelGameStates newState)
        {
            _mGameState = newState;
        }

        /// <summary>
        /// Finish Workday Region
        /// </summary>
        #region FinishWork
        public void FinishWorkday()
        {
            ManageUIProcessEndOfDay();
        }

        public void ActCoroutine(IEnumerator coroutine)
        {
            StartCoroutine(coroutine);
        }

        public void ManageNewJobHiredEvent(JobSupplierBitId newJobSupplier)
        {
            GetActiveGameProfile.GetActiveJobsModule().SetNewEmployer(newJobSupplier);
            GetActiveGameProfile.GetProfileCalendar().GetNextWorkDayObject().SetJobSupplier(newJobSupplier);        
        }
        private void ManageUIProcessEndOfDay()
        {
            Debug.Log("[ManageUIProcessEndOfDay] Start");
            ChangeHighLvlGameState(HighLevelGameStates.EndOfDay);
            _mGeneralInputManager.SetGamePlayState(InputGameState.OnlyOffice);
            _mGeneralFader.GeneralCurtainAppear();
            UIFinishWorkday();
            _mGameCameraManager.ChangeCameraState(GameCameraState.Office);
            _mGameCameraManager.ActivateNewCamera(GameCameraState.Office, 0);
            FadeInEndOfScene();
            Debug.Log("[ManageUIProcessEndOfDay] Finish");
        }
        
        private async void UIFinishWorkday()
        {
            await Task.Delay(1000);
            _mUIController.DeactivateAllObjects();
            _mUIController.ActivateObject(CanvasBitId.EndOfDay, EndOfDayPanelsBitStates.FIRST_PANEL);
            var endOfDayController = EndOfDayPanelController.Instance;
            endOfDayController.Initialize(_mRentCatalogueData, _mActiveGameProfile, _mFoodCatalogueData, _mTransportCatalogueData);
        }

        private async void FadeInEndOfScene()
        {
            await Task.Delay(8000);
            _mGeneralFader.GeneralCurtainDisappear();
        }
        #endregion
        #endregion
    }
}