using GamePlayManagement.BitDescriptions.Suppliers;

namespace DataUnits.JobSources
{
    public interface IJobSupplierObject : ISupplierBaseObject, ICallableSupplier
    {

        public void CheckCallingTime(int hour, int minute);
        public int DaysAsEmployer { get; set; }
        
        //GeneralData
        public IJobSupplierObjectData JobSupplierData { get; }
        
        //Stats related
        public void SetStats(int sanity, int kindness, int violence, int intelligence, int money);
        public int Sanity { get; }
        public int Kindness { get; }
        public int Violence { get; }
        public int Intelligence { get; }
        public int Money { get; }
        
        //Modules
        public IJobSupplierProductsModule JobProductsModule { get; }
        
        public void LoadDeflectionDialoguesData();
        public void StartUnlockData();
        public void ExpendMoney(int amount);
        void LocalInitialize(JobSupplierBitId jobId);
    }
}