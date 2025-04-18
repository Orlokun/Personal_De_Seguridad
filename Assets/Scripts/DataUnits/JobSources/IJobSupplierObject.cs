using DialogueSystem.Units;
using GamePlayManagement.BitDescriptions.Suppliers;
namespace DataUnits.JobSources
{
    public interface IJobSupplierObject : ISupplierBaseObject, ICallableSupplier
    {
        public void AddFondness(int amount);
        public void CheckCallingTime(int hour, int minute);
        public int DaysAsEmployer { get; set; }
        
        //GeneralData
        public IJobSupplierObjectData JobSupplierData { get; }
        
        //Stats related
        //TODO: This stats should be in data
        public void SetStats(int sanity, int kindness, int violence, int intelligence, int power, string spriteName);
        public int Sanity { get; }
        public int Kindness { get; }
        public int Violence { get; }
        public int Intelligence { get; }
        public int Power { get; }
        
        //Modules
        public IJobSupplierProductsModule JobProductsModule { get; }
        
        public void LoadDeflectionDialoguesData();
        public void StartUnlockData();
        public void ExpendMoney(int amount);
        void LocalInitialize(JobSupplierBitId jobId, DialogueSpeakerId speakerId);
        void PlayerHired();
    }
}