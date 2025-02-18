using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using CameraManagement;
using DataUnits;
using DataUnits.GameCatalogues;
using DataUnits.JobSources;
using DialogueSystem;
using DialogueSystem.Interfaces;
using GameDirection.DayLevelSceneManagers;
using GameDirection.NewsManagement;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement;
using GamePlayManagement.BitDescriptions;
using GamePlayManagement.BitDescriptions.Suppliers;
using GamePlayManagement.LevelManagement;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.CustomerInterfaces;
using InputManagement;
using UI;
using UI.EndOfDay;
using UI.PopUpManager;
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
        private IGameCameraOperator _mGameCameraManager;
        private IGeneralGameInputManager _gameInputManager;
        private IDialogueOperator _mDialogueOperator;
        private ISoundDirector _mSoundDirector;
        private IClockManagement _mClockManager;
        private IFeedbackManager _mFeedbackManager;
        private IGeneralGameInputManager _mIGeneralGameInputManager;
        private IModularDialogueDataController _mModularDialogues;
        private ICustomersInSceneManager _mCustomerInstantiationManager;
        private INewsNarrativeDirector _mNarrativeNewsDirector;
        private IMetaGameDirector _mMetaGameDirector;
        private IBaseTutorialDialogueData _mTutorialDialogueData;
        
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
        public IGameCameraOperator GetGameCameraManager => _mGameCameraManager;
        public IGeneralGameInputManager GetGameInputManager => _gameInputManager;
        public IDialogueOperator GetDialogueOperator => _mDialogueOperator;
        public ISoundDirector GetSoundDirector => _mSoundDirector;
        public IItemsDataController GetItemsDataController => _mItemDataController;
        public IRentValuesCatalogue GetRentCatalogueData => _mRentCatalogueData;
        public IModularDialogueDataController GetModularDialogueManager => _mModularDialogues;
        public ICustomersInSceneManager GetCustomerInstantiationManager => _mCustomerInstantiationManager;
        public INewsNarrativeDirector GetNarrativeNewsDirector=> _mNarrativeNewsDirector;
        public IMetaGameDirector GetMetaGameDirector => _mMetaGameDirector;
        #endregion

        #region Init
        private void Awake()
        {
            SingletonManagement();
            DontDestroyOnLoad(this);
            SetComponentManagers();
            LoadUIScene();
        }
        private void SingletonManagement()
        {
            if (_mInstance != null)
            {
                Destroy(this);
                return;
            }
            _mInstance = this;
        }
        private void SetComponentManagers()
        {
            _mLevelManager = GetComponent<LevelLoadManager>();
            _mGeneralFader = GetComponent<GeneralUIFader>();
            _gameInputManager = IGeneralGameGameInputManager.Instance;
        }
        private void LoadUIScene()
        {
            _mLevelManager.ActivateScene(LevelIndexId.UILvl);
        }
        /// <summary>
        /// Start Function of the Game director is when all objects get Centralized
        /// </summary>
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
            _mIGeneralGameInputManager = IGeneralGameGameInputManager.Instance;
            _mGameCameraManager = GameCameraOperator.Instance;     
            
            _mRentCatalogueData = RentValuesCatalogue.Instance;
            _mFoodCatalogueData = FoodValuesCatalogue.Instance;
            _mTransportCatalogueData = TransportValuesCatalogue.Instance;

            _mModularDialogues = Factory.CreateModularDialoguesDataController();
            _mModularDialogues.Initialize();
            _mTutorialDialogueData = Factory.CreateTutorialDialogueData();
            
            _mCustomerInstantiationManager = CustomersInSceneManager.Instance;
            _mNarrativeNewsDirector = Factory.CreateNewsNarrativeDirector();
            _mMetaGameDirector = Factory.CreateMetaGameDirectory();
            
            WaitAndLoadDeflectionInitialDialogues();
                
            _mUIController.StartMainMenuUI();
            _gameInputManager.SetGamePlayState(InputGameState.MainMenu);
            _mGameState = HighLevelGameStates.MainMenu;
        }

        private void LoadDayManagement(DayBitId dayId)
        {
            _dayLevelManager =  Factory.CreateLevelDayManager(dayId);
            _dayLevelManager.Initialize(this, dayId);
        }

        /// <summary>
        /// Deflections are loaded before the game starts so the player can call still locked suppliers.
        /// </summary>
        private async void WaitAndLoadDeflectionInitialDialogues()
        {
            await Task.Delay(400);
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

        #region ManageNewGame
        public void ContinueGame()
        {
            if(_mMetaGameDirector.GetExistingProfile == null)
            {
                StartNewGame();
                return;
            }
            _mActiveGameProfile = _mMetaGameDirector.GetExistingProfile;
            ContinueGameWithProfile();
        }
        
        public void StartNewGame()
        {
            //_gameStateManager.SetGamePlayState(InputGameState.Pause);
            _mGeneralFader.GeneralCurtainAppear();
            CreateNewProfile();
            LoadDayManagement(_mActiveGameProfile.GetProfileCalendar().GetCurrentWorkDayObject().BitId);
            _mGameState = HighLevelGameStates.InCutScene;
            StartCoroutine(_dayLevelManager.StartDayManagement());
        }

        public void ContinueGameWithProfile()
        {
            _mGeneralFader.GeneralCurtainAppear();
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
            var lifestyleModule = Factory.CreateLifestyleModule(_mRentCatalogueData, _mFoodCatalogueData, _mTransportCatalogueData);
            var requestModuleManager = Factory.CreateRequestsModuleManager();
            var profileStatusModule = Factory.CreatePlayerStatusModule();
            
            _mActiveGameProfile = Factory.CreatePlayerGameProfile(itemSuppliersModule,jobSourcesModule,calendarModule,lifestyleModule, profileStatusModule, requestModuleManager);
            _mActiveGameProfile.GetActiveJobsModule().UnlockJobSupplier(JobSupplierBitId.COPY_OF_EDEN);
            //_mActiveGameProfile.GetActiveItemSuppliersModule().UnlockSupplier(BitItemSupplier.D1TV);
            _mActiveGameProfile.UpdateProfileData();
            _mUIController.InitializeBaseInfoCanvas(_mActiveGameProfile);
            _mUIController.UpdateInfoUI();
            OnFinishDay += _mActiveGameProfile.UpdateDataEndOfDay;
        }
        
        public void ReleaseFromDialogueStateToGame()
        {
            if (_gameInputManager.CurrentInputGameState != InputGameState.InDialogue)
            {
                return;
            }
            _gameInputManager.SetGamePlayState(InputGameState.InGame);
        }
        #endregion

        #region Public Functions
        public void SubscribeCurrentWorkDayToCustomerManagement()
        {
            var currentDay = _mActiveGameProfile.GetProfileCalendar().GetCurrentWorkDayObject();
            _mCustomerInstantiationManager.RegisterObserver(currentDay);
        }

        public void ChangeHighLvlGameState(HighLevelGameStates newState)
        {
            _mGameState = newState;
        }
        
        public async void PlayerLost(EndingTypes organSale)
        {
            _mGeneralFader.GeneralCurtainAppear();
            _mSoundDirector.StopRadio();            
            await Task.Delay(2500);
            _mActiveGameProfile.PlayerLost(organSale);
            _mUIController.DeactivateAllObjects();
            ChangeHighLvlGameState(HighLevelGameStates.MainMenu);
            _mIGeneralGameInputManager.SetGamePlayState(InputGameState.MainMenu);
            _mLevelManager.ReturnToMainScreen();
            _mUIController.UpdateInfoUI();
            _mUIController.StartMainMenuUI();
            await WaitAndOpenCurtain();
        }

        //TODO: Look for better solution that Task.Delay(). Maybe use a Coroutine
        public async void StartTutorialProcess(int tutorialIndex)
        {
            _mDialogueOperator.KillDialogue();
            await Task.Delay(300);
            var tutorialDialogue = _mTutorialDialogueData.GetTutorialDialogue(tutorialIndex);
            _mDialogueOperator.StartNewDialogue(tutorialDialogue);
        }

        private async Task WaitAndOpenCurtain()
        {
            await Task.Delay(2000);
            _mUIController.StartMainMenuUI();
            _mGeneralFader.GeneralCurtainDisappear();
        }

        /// <summary>
        /// Finish Workday Region
        /// </summary>
        #region FinishWork
        public void FinishWorkday()
        {
            ManageUIProcessEndOfDay();
            OnFinishDay?.Invoke();
        }

        public delegate void FinishDay();
        public event FinishDay OnFinishDay;
        public void ActCoroutine(IEnumerator coroutine)
        {
            StartCoroutine(coroutine);
        }

        public void ManageNewJobHiredEvent(JobSupplierBitId newJobSupplier)
        {
            GetActiveGameProfile.GetActiveJobsModule().SetNewEmployer(newJobSupplier);
            GetActiveGameProfile.GetProfileCalendar().GetNextWorkDayObject().SetJobSupplier(newJobSupplier);
            GetActiveGameProfile.GetRequestsModuleManager().CheckHireChallenges(newJobSupplier);
            _mUIController.HiredInJobFoundFeedbackEvent(newJobSupplier);
            GetActiveGameProfile.UpdateProfileData();
        }

        public void ManageNewItemSupplierUnlockedEvent(BitItemSupplier itemsupplier)
        {
            GetActiveGameProfile.GetActiveItemSuppliersModule().UnlockSupplier(itemsupplier);
            GetActiveGameProfile.UpdateProfileData();
        }

        public void LaunchTutorial()
        {
            
        }
        
        public void BeginNewDayProcess()
        {
            var newDayId = GetActiveGameProfile.GetProfileCalendar().GetNextWorkDayObject().BitId;
            GetActiveGameProfile.GetProfileCalendar().SetCurrentWorkDayObject(newDayId);
            _dayLevelManager = Factory.CreateLevelDayManager(newDayId);
            _dayLevelManager.Initialize(this, newDayId);
            StartCoroutine(_dayLevelManager.StartDayManagement());
        }

        public ICallableSupplier GetSpeakerData(DialogueSpeakerId dialogueNodeSpeakerId)
        {

            if (_mJobsCatalogue.JobSuppliersInData.Any(x => x.SpeakerIndex == dialogueNodeSpeakerId))
            {
                return _mJobsCatalogue.JobSuppliersInData.SingleOrDefault(x => x.SpeakerIndex == dialogueNodeSpeakerId);
            }
            if (_mItemSuppliersData.GetItemSuppliersInData.Any(x => x.SpeakerIndex == dialogueNodeSpeakerId))
            {
                return _mItemSuppliersData.GetItemSuppliersInData.SingleOrDefault(x => x.SpeakerIndex == dialogueNodeSpeakerId);
            }
            if(dialogueNodeSpeakerId == DialogueSpeakerId.Omnicorp)
            {
                return ScriptableObject.CreateInstance<OmniCorpCallObject>();
            }
            return null;
        }



        private void ManageUIProcessEndOfDay()
        {
            Debug.Log("[ManageUIProcessEndOfDay] Start");
            _mIGeneralGameInputManager.SetGamePlayState(InputGameState.OnlyOffice);
            _mSoundDirector.StopRadio();            
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
            await Task.Delay(4000);
            _mGeneralFader.GeneralCurtainDisappear();
        }
        #endregion
        #endregion
    }

    public interface IMetaGameDirector
    {
        public IPlayerGameProfile GetExistingProfile { get; }
        public void AddNewProfile(IPlayerGameProfile profle);
    }

    public class MetaGameDirector : IMetaGameDirector
    {
        private IPlayerGameProfile _mExistingProfile;
        public IPlayerGameProfile GetExistingProfile => _mExistingProfile;
        public void AddNewProfile(IPlayerGameProfile profile)
        {
            _mExistingProfile = null;
        }
    }
}