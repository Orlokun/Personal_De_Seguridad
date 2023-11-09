using GamePlayManagement.BitDescriptions.Suppliers;

namespace DataUnits.JobSources
{
    public interface IJobSupplierObject : ISupplierBaseObject, ICallableSupplier
    {
        public BitGameJobSuppliers BitId { get; set; }
        public string StoreType{ get; set; }
        public string StoreOwnerName{ get; set; }
        public int StoreUnlockPoints{ get; set; }
        public string StoreDescription{ get; set; }
        public void LoadDialogueData();
    }
}