using GamePlayManagement.BitDescriptions.Suppliers;

namespace DataUnits.JobSources
{
    public interface IJobSupplierObject : ISupplierBaseObject, ICallableSupplier
    {

        public void CheckCallingTime(int hour, int minute);
        public int DaysAsEmployer { get; set; }
        //Base Data
        public JobSupplierBitId JobSupplierBitId { get; set; }
        public string StoreType{ get; set; }
        public string StoreOwnerName{ get; set; }
        public int StoreOwnerAge{ get; set; }
        public int InitialBudget { get; set; }

        public int StoreUnlockPoints{ get; set; }
        public string StoreDescription{ get; set; }
        public int[] StoreMinMaxClients { get; set; }
        public string SpriteName { get; set; }
        
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
    }
}