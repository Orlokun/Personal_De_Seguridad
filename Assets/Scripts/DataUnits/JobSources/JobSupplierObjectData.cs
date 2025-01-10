using GamePlayManagement.BitDescriptions.Suppliers;

namespace DataUnits.JobSources
{
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
}