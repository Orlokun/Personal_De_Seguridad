using GamePlayManagement.BitDescriptions.Suppliers;

namespace GamePlayManagement.ProfileDataModules
{
    public interface IJobsSourcesModule : IProfileModule, IJobSourcesModuleData
    {
        void UnlockJobSupplier(JobSupplierBitId gainedJobSupplier);
        public void SetNewEmployer(JobSupplierBitId newEmployer);
        public void ProcessEndOfDay();
    }
}