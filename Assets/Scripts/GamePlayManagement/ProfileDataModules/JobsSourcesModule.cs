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
        private int _jobsActive = 0;
        private int _archivedJobs = 0;
        private Dictionary<BitGameJobSuppliers, IJobSupplierObject> _mJobSuppliers = new Dictionary<BitGameJobSuppliers, IJobSupplierObject>();
        public int ElementsActive => _jobsActive;
        public Dictionary<BitGameJobSuppliers, IJobSupplierObject> JobObjects => _mJobSuppliers;
        private BitGameJobSuppliers _mCurrentActiveEmployer;
        public BitGameJobSuppliers CurrentEmployer => _mCurrentActiveEmployer;
        public void SetNewEmployer(BitGameJobSuppliers newEmployer)
        {
            _mCurrentActiveEmployer = newEmployer;
        }

        public bool IsModuleActive => _jobsActive > 0;
        private IBaseJobsCatalogue _jobsCatalogue;

        public JobsSourcesModule(IBaseJobsCatalogue jobsCatalogue)
        {
            _jobsCatalogue = jobsCatalogue;
        }
        
        public void UnlockJobSupplier(BitGameJobSuppliers gainedJobSupplier)
        {
            if ((_jobsActive & (int) gainedJobSupplier) != 0)
            {
                return;
            }
            _jobsActive |= (int) gainedJobSupplier;

            if (_mJobSuppliers.ContainsKey(gainedJobSupplier))
            {
                Debug.LogError("[JobsSourcesModule.AddJobToModule] Job Supplier was already added");
                return;
            }

            if (!_jobsCatalogue.JobSupplierExists(gainedJobSupplier))
            {
                Debug.LogError("[JobsSourcesModule.AddJobToModule] Job Supplier must exist in catalogue before trying to activate it");
            }
            _mJobSuppliers.Add(gainedJobSupplier, _jobsCatalogue.GetJobSupplierObject(gainedJobSupplier));
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

        public void SetProfile(IPlayerGameProfile currentPlayerProfile)
        {
            _activePlayer = currentPlayerProfile;
        }
    }
}