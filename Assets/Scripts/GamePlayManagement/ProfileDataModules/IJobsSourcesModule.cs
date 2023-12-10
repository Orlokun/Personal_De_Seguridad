using System.Collections.Generic;
using DataUnits.JobSources;
using GamePlayManagement.BitDescriptions.Suppliers;

namespace GamePlayManagement.ProfileDataModules
{
    public interface IJobsSourcesModule : IProfileModule
    {
        void UnlockJobSupplier(JobSupplierBitId gainedJobSupplier);
        public int ElementsActive { get; }
        Dictionary<JobSupplierBitId, IJobSupplierObject> JobObjects { get; }
        public JobSupplierBitId CurrentEmployer { get; }
        public void SetNewEmployer(JobSupplierBitId newEmployer);

        public int TotalDaysEmployed { get; }
        public int DaysEmployedStreak { get; }
        public int TotalDaysUnemployed { get; }
        public int DaysUnemployedStreak { get; }
        public int StreakWithEmployer { get; }
    }
}