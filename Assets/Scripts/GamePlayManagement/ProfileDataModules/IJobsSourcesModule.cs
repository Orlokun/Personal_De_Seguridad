using System.Collections.Generic;
using DataUnits.JobSources;
using GamePlayManagement.BitDescriptions.Suppliers;

namespace GamePlayManagement.ProfileDataModules
{
    public interface IJobsSourcesModule : IProfileModule, IJobSourcesModuleData
    {
        void UnlockJobSupplier(JobSupplierBitId gainedJobSupplier);
        public void SetNewEmployer(JobSupplierBitId newEmployer);
        public void StartFinishDay();
    }

    public interface IJobSourcesModuleData
    {
        public int UnlockedJobSuppliers { get; }
        Dictionary<JobSupplierBitId, IJobSupplierObject> ActiveJobObjects { get; }
        public JobSupplierBitId CurrentEmployer { get; }
        public int TotalDaysEmployed { get; }
        public int DaysEmployedStreak { get; }
        public int TotalDaysUnemployed { get; }
        public int DaysUnemployedStreak { get; }
        public int StreakWithEmployer { get; }
        public IJobSupplierObject CurrentEmployerData();

    }
}