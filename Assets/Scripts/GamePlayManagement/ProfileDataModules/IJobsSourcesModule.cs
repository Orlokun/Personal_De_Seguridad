using System.Collections.Generic;
using DataUnits.JobSources;
using GamePlayManagement.BitDescriptions.Suppliers;

namespace GamePlayManagement.ProfileDataModules
{
    public interface IJobsSourcesModule : IProfileModule, IJobSourcesModuleData
    {
        void UnlockJobSupplier(JobSupplierBitId gainedJobSupplier);
        public void SetNewEmployer(JobSupplierBitId newEmployer);
        public void ProcessEndOfDay();
    }

    public interface IJobSourcesModuleData
    {
        public int MUnlockedJobSuppliers { get; }
        Dictionary<JobSupplierBitId, IJobSupplierObject> ActiveJobObjects { get; }
        public Dictionary<JobSupplierBitId, IJobSupplierObject> ArchivedJobs { get; }
        public JobSupplierBitId CurrentEmployer { get; }
        public int TotalDaysEmployed { get; }
        public int DaysEmployedStreak { get; }
        public int TotalDaysUnemployed { get; }
        public int DaysUnemployedStreak { get; }
        public int StreakWithEmployer { get; }
        public IJobSupplierObject CurrentEmployerData();

    }
}