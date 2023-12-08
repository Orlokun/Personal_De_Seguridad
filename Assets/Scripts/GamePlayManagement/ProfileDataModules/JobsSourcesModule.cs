using System.Collections.Generic;
using DataUnits;
using DataUnits.GameCatalogues;
using DataUnits.ItemScriptableObjects;
using DataUnits.JobSources;
using GamePlayManagement.BitDescriptions.Suppliers;
using UnityEngine;

namespace GamePlayManagement.ProfileDataModules
{
    public class JobsSourcesModule : IJobsSourcesModule
    {
        private IPlayerGameProfile _activePlayer;
        private int unlockedJobSuppliers = 0;
        private int _archivedJobs = 0;
        private Dictionary<JobSupplierBitId, IJobSupplierObject> _mJobSuppliers = new Dictionary<JobSupplierBitId, IJobSupplierObject>();
        public int ElementsActive => unlockedJobSuppliers;
        public Dictionary<JobSupplierBitId, IJobSupplierObject> JobObjects => _mJobSuppliers;
        private JobSupplierBitId _mCurrentActiveEmployer;
        public JobSupplierBitId CurrentEmployer => _mCurrentActiveEmployer;
        
        public void SetNewEmployer(JobSupplierBitId newEmployer)
        {
            _mCurrentActiveEmployer = newEmployer;
        }

        public bool IsModuleActive => unlockedJobSuppliers > 0;
        private IBaseJobsCatalogue _jobsCatalogue;

        public JobsSourcesModule(IBaseJobsCatalogue jobsCatalogue)
        {
            _jobsCatalogue = jobsCatalogue;
        }
        
        public void UnlockJobSupplier(JobSupplierBitId gainedJobSupplier)
        {
            if ((unlockedJobSuppliers & (int) gainedJobSupplier) != 0)
            {
                return;
            }
            unlockedJobSuppliers |= (int) gainedJobSupplier;

            if (_mJobSuppliers.ContainsKey(gainedJobSupplier))
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
            _mJobSuppliers.Add(gainedJobSupplier, newSupplier);
        }


        public void ArchiveJob(JobSupplierBitId lostJobSupplier)
        {
            //Remove Job from active ones
            if ((unlockedJobSuppliers & (int) lostJobSupplier) == 0)
            {
                return;
            }
            unlockedJobSuppliers &= (int)~lostJobSupplier;
            
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
    }
}