using System.Collections.Generic;
using DataUnits.GameCatalogues;
using DataUnits.JobSources;
using GamePlayManagement.BitDescriptions.Suppliers;
using UnityEngine;

namespace GamePlayManagement.ProfileDataModules
{
    public class JobSourceModule : IJobsSourcesModule
    {
        private IPlayerGameProfile _activePlayer;
        private int _unlockedJobJobSuppliers = 0;
        private int _archivedJobs = 0;
        private Dictionary<JobSupplierBitId, IJobSupplierObject> _mActiveJobSuppliers = new Dictionary<JobSupplierBitId, IJobSupplierObject>();
        
        private int _mTotalDaysEmployed;
        private int _mDaysEmployedStreak;
        private int _mTotalDaysUnemployed;
        private int _mDaysUnemployedStreak;
        private int _mStreakWithEmployer;
        
        public int UnlockedJobSuppliers => _unlockedJobJobSuppliers;
        public Dictionary<JobSupplierBitId, IJobSupplierObject> ActiveJobObjects => _mActiveJobSuppliers;
        private JobSupplierBitId _mCurrentActiveEmployer;
        public JobSupplierBitId CurrentEmployer => _mCurrentActiveEmployer;
        
        #region Constructor & API
        public JobSourceModule(IBaseJobsCatalogue jobsCatalogue)
        {
            _jobsCatalogue = jobsCatalogue;
        }
        
        /// <summary>
        /// Current Employer
        /// </summary>
        /// <returns></returns>
        public IJobSupplierObject CurrentEmployerData()
        {
            if (_mCurrentActiveEmployer == 0)
            {
                Debug.LogWarning("[CurrentEmployerData] Current Employer Data must not be 0. Make sure to add a new employer before accessing level");
                return null;
            }
            return _mActiveJobSuppliers[_mCurrentActiveEmployer];
        }
        public void SetNewEmployer(JobSupplierBitId newEmployer)
        {
            _mStreakWithEmployer = 0;
            _mDaysUnemployedStreak = 0;
            _mCurrentActiveEmployer = newEmployer;
            _mActiveJobSuppliers[newEmployer].PlayerHired();
        }

        public void QuitFiredFromJob()
        {
            _mCurrentActiveEmployer = 0;
        }
        public void StartFinishDay()
        {
            if (_mCurrentActiveEmployer == 0)
            {
                _mTotalDaysUnemployed++;
                _mDaysUnemployedStreak++;
            }
            else
            {
                _mStreakWithEmployer++;
                _mDaysEmployedStreak++;
                _mTotalDaysEmployed++;
                _mActiveJobSuppliers[_mCurrentActiveEmployer].DaysAsEmployer++;
                _mDaysUnemployedStreak = 0;
            }
        }
        public int TotalDaysEmployed => _mTotalDaysEmployed;
        public int DaysEmployedStreak => _mDaysEmployedStreak;
        public int TotalDaysUnemployed => _mTotalDaysUnemployed;
        public int DaysUnemployedStreak => _mDaysUnemployedStreak;
        public int StreakWithEmployer => _mStreakWithEmployer;

        public bool IsModuleActive => _unlockedJobJobSuppliers > 0;


        private IBaseJobsCatalogue _jobsCatalogue;

        public void UnlockJobSupplier(JobSupplierBitId gainedJobSupplier)
        {
            if ((_unlockedJobJobSuppliers & (int) gainedJobSupplier) != 0)
            {
                return;
            }
            _unlockedJobJobSuppliers |= (int) gainedJobSupplier;

            if (_mActiveJobSuppliers.ContainsKey(gainedJobSupplier))
            {
                Debug.LogError("[JobsSourcesModule.AddJobToModule] Job Supplier was already added");
                return;
            }

            if (!_jobsCatalogue.JobSupplierExists(gainedJobSupplier))
            {
                Debug.LogError("[JobsSourcesModule.AddJobToModule] Job Supplier must exist in catalogue before trying to activate it");
            }

            var newSupplier = _jobsCatalogue.GetJobSupplierObject(gainedJobSupplier);
            newSupplier.StartUnlockData();
            _mActiveJobSuppliers.Add(gainedJobSupplier, newSupplier);
        }
        public void ArchiveJob(JobSupplierBitId lostJobSupplier)
        {
            //Remove Job from active ones
            if ((_unlockedJobJobSuppliers & (int) lostJobSupplier) == 0)
            {
                return;
            }
            _unlockedJobJobSuppliers &= (int)~lostJobSupplier;
            
            //Add to archived jobs
            if ((_archivedJobs & (int) lostJobSupplier) != 0)
            {
                return;
            }
            _archivedJobs |= (int) lostJobSupplier;
        }

        public void SetProfile(IPlayerGameProfile currentPlayerProfile)
        {
            _activePlayer = currentPlayerProfile;
        }
        #endregion
    }
}