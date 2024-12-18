using System;
using System.Linq;
using System.Threading.Tasks;
using DialogueSystem;
using GameDirection;
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
    [CreateAssetMenu(menuName = "Jobs/JobSource")]
    public class JobSupplierObject : ScriptableObject, IJobSupplierObject
    {
        private IJobSupplierDialogueModule _dialogueModule;
        private IJobSupplierProductsModule _productsModuleModule;
        #region Constructor & API
        public JobSupplierObject()
        {
            _dialogueModule = new JobSupplierDialogueModule(this);
            _productsModuleModule = new JobSupplierProductsModule(this);
        }
        public JobSupplierBitId JobSupplierBitId { get; set; }
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
        public void StartCalling(int playerLevel)
        {
            if (playerLevel < StoreUnlockPoints)
            {
                RandomDeflection();
            }
            else
            {
                GetCurrentUnlockedCall();
            }
        }

        private void CheckLastCallsStatus()
        {
            
        }
        
        private async void GetCurrentUnlockedCall()
        {
            Debug.LogWarning("[GetCurrentCallAnswer] UNLOCKED STORE CALL");
            Random.InitState(DateTime.Now.Millisecond);
            var randomWaitTime = Random.Range(500, 4500);
            await Task.Delay(randomWaitTime);

            var randomAnswerIndex = Random.Range(StoreUnlockPoints, StoreHighestUnlockedDialogue);
            var randomDialogue = _dialogueModule.ImportantDialogues[randomAnswerIndex];
            PhoneCallOperator.Instance.PlayAnswerSound();
            GameDirector.Instance.GetDialogueOperator.StartNewDialogue(randomDialogue);
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