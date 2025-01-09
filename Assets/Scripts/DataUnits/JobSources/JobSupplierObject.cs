using System;
using System.Linq;
using System.Threading.Tasks;
using DialogueSystem;
using GameDirection;
using GamePlayManagement;
using GamePlayManagement.BitDescriptions.Suppliers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DataUnits.JobSources
{
    public enum DialogueType
    {
        Deflections = 1,
        ImportantDialogue = 2,
        InsistenceDialogue = 3,
        CallingDialogues = 4,
        CallingDialoguesData = 5,
    }
    
    [Serializable]
    public class OmnicorpCallObject : ScriptableObject, ICallableSupplier
    {
        public OmnicorpCallObject()
        {
            SpeakerIndex = DialogueSpeakerId.Omnicorp;
        }
        public string SpeakerName => "Omnicorp";
        public DialogueSpeakerId SpeakerIndex { get; set; }
        public void ReceivePlayerCall(IPlayerGameProfile playerProfile)
        {
            
        }

        public int StoreHighestUnlockedDialogue => 0;
    }

    public class JobSupplierObjectData : IJobSupplierObjectData
    {
        public JobSupplierBitId JobSupplierBitId { get; set; }
        public string StoreType { get; set; }
        public string StoreOwnerName { get; set; }
        public int StoreOwnerAge { get; set; }
        public int Budget { get; set; }
        public int StoreUnlockPoints { get; set; }
        public string StoreDescription { get; set; }
        public int[] StoreMinMaxClients { get; set; }
        public string SpriteName { get; set; }
    }

    public interface IJobSupplierObjectData
    {
        //Base Data
        public JobSupplierBitId JobSupplierBitId { get; set; }
        public string StoreType{ get; set; }
        public string StoreOwnerName{ get; set; }

        public int StoreOwnerAge{ get; set; }
        public int Budget { get; set; }

        public int StoreUnlockPoints{ get; set; }
        public string StoreDescription{ get; set; }
        public int[] StoreMinMaxClients { get; set; }
        public string SpriteName { get; set; }
    }

    public enum CountPetrolkStates
    {
        WaitingForHire = 1,
        RequiresMindProtection = 2,
        RequiresProductVigilance = 4,
        RequiresPunishBloodless = 8
    }
    
    
    [Serializable]
    [CreateAssetMenu(menuName = "Jobs/JobSource")]
    public class JobSupplierObject : ScriptableObject, IJobSupplierObject
    {
        private IJobSupplierDialogueModule _dialogueModule;
        private IJobSupplierProductsModule _productsModuleModule;
        private IJobSupplierObjectData _mSupplierData;
        #region Constructor & API
        public void Initialize(JobSupplierBitId id)
        {
            _mSupplierData = new JobSupplierObjectData();
            _mSupplierData.JobSupplierBitId = id;
            _dialogueModule = new JobSupplierDialogueModule(this);
            _productsModuleModule = new JobSupplierProductsModule(this);
        }
        
        public  IJobSupplierObjectData JobSupplierData => _mSupplierData;
        public JobSupplierBitId JobSupplierBitId { get => _mSupplierData.JobSupplierBitId;}
        public string StoreType{ get; set; }
        public string StoreName{ get; set; }
        public string StoreOwnerName{ get; set; }
        public int Budget { get; set; }
        public int StoreUnlockPoints{ get; set; }
        public string StoreDescription{ get; set; }
        public int[] StoreMinMaxClients { get; set; }
        public int StorePhoneNumber{ get; set; }
        public int StoreOwnerAge{ get; set; }
        public DialogueSpeakerId SpeakerIndex { get; set; }
        public string SpriteName { get; set; }

        public int Sanity => _mSanity;
        public int Kindness => _mKindness;
        public int Violence => _mViolence;
        public int Intelligence => _mIntelligence;
        public int Money => _mMoney;


        public int DaysAsEmployer
        {
            get => _mDaysAsEmployer;
            set => _mDaysAsEmployer = value;
        }

        public IJobSupplierProductsModule JobProductsModule => _productsModuleModule;

        public void LoadDeflectionDialoguesData()
        {
            _dialogueModule.LoadInitialDeflectionDialogues();
        }

        public void StartUnlockData()
        {
            _dialogueModule.StartUnlockDialogueData();
            _productsModuleModule.LoadProductData();
        }

        public void ExpendMoney(int amount)
        {
            Budget -= amount;
        }

        #endregion

        #region SupplierStats
        private int _mSanity;
        private int _mKindness;
        private int _mViolence;
        private int _mIntelligence;
        private int _mMoney;

        ///Store Stats
        public void SetStats(int sanity, int kindness, int violence, int intelligence, int money)
        {
            _mSanity = sanity;
            _mKindness = kindness;
            _mViolence = violence;
            _mIntelligence = intelligence;
            _mMoney = money;
        }
        #endregion

        #region Members
        public int StoreHighestUnlockedDialogue => _mStoreHighestUnlockedDialogue;
        private int _mStoreHighestUnlockedDialogue;

        public string SpeakerName => StoreOwnerName;
        public string SpeakerImageString { get; }
        
        #endregion

        #region HiredStatus

        private int _mDaysAsEmployer;
        

        #endregion
        
        
        #region CallManagement
        
        public void CheckCallingTime(int hour, int minute)
        {
            var jobModule = GameDirector.Instance.GetActiveGameProfile.GetActiveJobsModule();
            if (jobModule.CurrentEmployer != JobSupplierBitId)
            {
                return;
            }
            var anyCallMatters = _dialogueModule.SupplierCallDialoguesDataDictionary.Any(x =>
                x.Value.JobDay == _mDaysAsEmployer &&
                x.Value.CallHour == hour && x.Value.CallMinute == minute);
            if (!anyCallMatters)
            {
                return;
            }
            var dialogueData = _dialogueModule.SupplierCallDialoguesDataDictionary.SingleOrDefault(x =>
                x.Value.JobDay == _mDaysAsEmployer &&
                x.Value.CallHour == hour && x.Value.CallMinute == minute);

            var getCallDialogue = _dialogueModule.SupplierCallDialogues[dialogueData.Key];
            PhoneCallOperator.Instance.StartCallFromSupplier(getCallDialogue);
        }
        
        private int _lastCallExp = 0;
        
        //TODO: Implement the call system with a class/interface argument for more better management 
        public void ReceivePlayerCall(IPlayerGameProfile playerProfile)
        {
            if (playerProfile.GetStatusModule().PlayerXp < StoreUnlockPoints)
            {
                RandomDeflection();
            }
            else
            {
                StartAnswerBuildingProcess();
            }
        }

        private void CheckLastCallsStatus()
        {
            
        }
        
        private async void StartAnswerBuildingProcess()
        {
            Debug.LogWarning("[StartAnswerBuildingProcess] Store can be called");
            Random.InitState(DateTime.Now.Millisecond);
            var randomWaitTime = Random.Range(500, 4500);
            await Task.Delay(randomWaitTime);
            var unlockedCallIndex = 0;
            
            //Step1: Get Only Unlocked Dialogues
            //Step2: Go from first to last and check conditions: 
            //          1. If the dialogue has not been activated. Activate it.
            //          2. If dialogue has been activated, check if has extra conditions before advancing to next.
            //          3. If condition has been met, flag as done and move to next.
            
            if (_dialogueModule.ImportantDialogues.Any(x => x.Value.TimesActivatedCount == 0))
            {
                unlockedCallIndex = _dialogueModule.ImportantDialogues.FirstOrDefault(x => x.Value.TimesActivatedCount == 0).Key;
            }
            else
            {
                unlockedCallIndex = _dialogueModule.ImportantDialogues.FirstOrDefault(x => x.Value.TimesActivatedCount == 1).Key;
            }
            var lastUnlockedDialogue = _dialogueModule.ImportantDialogues[unlockedCallIndex];
            PhoneCallOperator.Instance.PlayAnswerSound();
            GameDirector.Instance.GetDialogueOperator.StartNewDialogue(lastUnlockedDialogue);
        }
        
        private async void RandomDeflection()
        {
            Debug.LogWarning("[RandomDeflection] STORE NOT UNLOCKED");
            if (GameDirector.Instance.GetDialogueOperator.CurrentDialogueState != UIDialogueState.NotDisplayed)
            {
                return;
            }
            Random.InitState(DateTime.Now.Millisecond);
            var randomDeflectionIndex = Random.Range(1, _dialogueModule.DeflectionDialogues.Count-1);
            var randomDialogue = _dialogueModule.DeflectionDialogues[randomDeflectionIndex];
            Random.InitState(DateTime.Now.Millisecond);
            var randomWaitTime = Random.Range(500, 12500);
            await Task.Delay(randomWaitTime);

            PhoneCallOperator.Instance.PlayAnswerSound();
            GameDirector.Instance.GetDialogueOperator.StartNewDialogue(randomDialogue);
        }
        #endregion
    }
}