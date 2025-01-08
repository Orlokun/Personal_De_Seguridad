using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DialogueSystem.Interfaces;
using GameDirection;
using Utils;

namespace DataUnits.JobSources
{
    public interface IJobSupplierDialogueModule
    {
        void LoadInitialDeflectionDialogues();
        void StartUnlockDialogueData();
        public Dictionary<int, IDialogueObject> DeflectionDialogues { get; }
        public Dictionary<int, IDialogueObject> ImportantDialogues { get; }
        public Dictionary<int, IDialogueObject> InsistenceDialogues { get; }
        public Dictionary<int, IDialogueObject> SupplierCallDialogues{ get; }
        public Dictionary<int, ISupplierCallDialogueDataObject> SupplierCallDialoguesDataDictionary { get; }
    }

    public interface IJobSupplierDialogueModuleData
    {
        public Dictionary<int, IDialogueObject> DeflectionDialogues { get; }
        public Dictionary<int, IDialogueObject> ImportantDialogues { get; }
        public Dictionary<int, IDialogueObject> InsistenceDialogues { get; }
        public Dictionary<int, IDialogueObject> SupplierCallDialogues { get; }
        public void LoadInitialDeflectionDialogues();
        public Dictionary<int, ISupplierCallDialogueDataObject> SupplierCallDialoguesDataDictionary { get; }
        public IEnumerator DownloadDialogueData(DialogueType dialogueType, string url);

    }

    public class JobSupplierDialogueModule : IJobSupplierDialogueModule
    {
        private IJobSupplierObject _supplierObject;
        private IJobSupplierDialogueModuleData _mDialogueData;
        public JobSupplierDialogueModule(IJobSupplierObject supplierObject)
        {
            _supplierObject = supplierObject;
            _mDialogueData = new JobSupplierDialogueModuleData(supplierObject);
        }
       
        public Dictionary<int, IDialogueObject> DeflectionDialogues => _mDialogueData.DeflectionDialogues;
        public Dictionary<int, IDialogueObject> ImportantDialogues => _mDialogueData.ImportantDialogues;
        public Dictionary<int, IDialogueObject> InsistenceDialogues => _mDialogueData.ImportantDialogues;
        public Dictionary<int, IDialogueObject> SupplierCallDialogues => _mDialogueData.SupplierCallDialogues;
        public Dictionary<int, ISupplierCallDialogueDataObject> SupplierCallDialoguesDataDictionary => _mDialogueData.SupplierCallDialoguesDataDictionary;

        public void LoadInitialDeflectionDialogues()
        {
            _mDialogueData.LoadInitialDeflectionDialogues();
        }

        public void StartUnlockDialogueData()
        {
            GetUnlockedDialogues();
            
            //Subscribe Unlocked NPC to possible phone call events
            GameDirector.Instance.GetClockInDayManagement.OnPassMinute += _supplierObject.CheckCallingTime;
        }
        private async void GetUnlockedDialogues()
        {
            var importantDialoguesUrl = DataSheetUrls.SuppliersDialogueGameData(_supplierObject.SpeakerIndex, DialogueType.ImportantDialogue);
            var insistenceDialoguesUrl = DataSheetUrls.SuppliersDialogueGameData(_supplierObject.SpeakerIndex, DialogueType.InsistenceDialogue);
            var callDialoguesUrl = DataSheetUrls.SuppliersDialogueGameData(_supplierObject.SpeakerIndex, DialogueType.CallingDialogues);
            var callDialoguesDataUrl = DataSheetUrls.SuppliersDialogueGameData(_supplierObject.SpeakerIndex, DialogueType.CallingDialoguesData);
            await Task.Delay(300);
            GameDirector.Instance.ActCoroutine(_mDialogueData.DownloadDialogueData(DialogueType.ImportantDialogue, importantDialoguesUrl));
            await Task.Delay(500);
            GameDirector.Instance.ActCoroutine(_mDialogueData.DownloadDialogueData(DialogueType.InsistenceDialogue, insistenceDialoguesUrl));
            await Task.Delay(500);
            GameDirector.Instance.ActCoroutine(_mDialogueData.DownloadDialogueData(DialogueType.CallingDialogues, callDialoguesUrl));
            await Task.Delay(500);
            GameDirector.Instance.ActCoroutine(_mDialogueData.DownloadDialogueData(DialogueType.CallingDialoguesData, callDialoguesDataUrl));
        }
    }
}