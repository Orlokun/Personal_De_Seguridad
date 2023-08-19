using GameManagement.BitDescriptions.Suppliers;
using GamePlayManagement.BitDescriptions.Suppliers;

namespace GamePlayManagement.ProfileDataModules
{
    public class JobsSourceModule : IJobsSourcesModule
    {
        private int _jobsActive = 0;
        private int _archivedJobs = 0;
        public int ElementsActive => _jobsActive;
        public bool IsModuleActive => _jobsActive > 0;
        
        public void AddJobToModule(GameJobs gainedJob)
        {
            if ((_jobsActive & (int) gainedJob) != 0)
            {
                return;
            }
            _jobsActive |= (int) gainedJob;
        }

        public void ArchiveJob(GameJobs lostJob)
        {
            //Remove Job from active ones
            if ((_jobsActive & (int) lostJob) == 0)
            {
                return;
            }
            _jobsActive &= (int)~lostJob;
            
            //Add to archived jobs
            if ((_archivedJobs & (int) lostJob) != 0)
            {
                return;
            }
            _archivedJobs |= (int) lostJob;
        }
    }
}