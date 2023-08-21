using GamePlayManagement.BitDescriptions.Suppliers;

namespace GamePlayManagement.ProfileDataModules
{
    public class JobsSourcesModule : IJobsSourcesModule
    {
        private int _jobsActive = 0;
        private int _archivedJobs = 0;
        public int ElementsActive => _jobsActive;
        public bool IsModuleActive => _jobsActive > 0;
        
        public void AddJobToModule(BitGameJobSuppliers gainedJobSupplier)
        {
            if ((_jobsActive & (int) gainedJobSupplier) != 0)
            {
                return;
            }
            _jobsActive |= (int) gainedJobSupplier;
        }

        public void ArchiveJob(BitGameJobSuppliers lostJobSupplier)
        {
            //Remove Job from active ones
            if ((_jobsActive & (int) lostJobSupplier) == 0)
            {
                return;
            }
            _jobsActive &= (int)~lostJobSupplier;
            
            //Add to archived jobs
            if ((_archivedJobs & (int) lostJobSupplier) != 0)
            {
                return;
            }
            _archivedJobs |= (int) lostJobSupplier;
        }
    }
}