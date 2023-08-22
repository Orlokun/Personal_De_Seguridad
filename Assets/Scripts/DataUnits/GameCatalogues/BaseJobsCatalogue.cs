using System.Collections.Generic;
using System.Linq;
using GamePlayManagement.BitDescriptions.Suppliers;
using UnityEngine;

namespace DataUnits.GameCatalogues
{
    public interface IBaseJobsCatalogue
    {
        bool JobSupplierExists(BitGameJobSuppliers jobSupplier);
        IJobSupplierObject GetJobSupplierObject(BitGameJobSuppliers jobSupplier);
    }

    public class BaseJobsCatalogue : MonoBehaviour, IBaseJobsCatalogue
    {
        private static BaseJobsCatalogue _instance;
        public static IBaseJobsCatalogue Instance => _instance;
    
        [SerializeField] private List<JobSupplierObject> jobSuppliersInGame;

        private List<IJobSupplierObject> _mIjobSuppliers;
        public List<IJobSupplierObject> JobSuppliers => _mIjobSuppliers;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            if (_instance != null)
            {
                Destroy(this);
            }
            _instance = this;
        
        
            if (jobSuppliersInGame == null || jobSuppliersInGame.Count == 0)
            {
                return;
            }
            LoadInterfaces();
        }

        private void LoadInterfaces()
        {
            _mIjobSuppliers = new List<IJobSupplierObject>();
            foreach (var jobSupplierObject in jobSuppliersInGame)
            {
                _mIjobSuppliers.Add(jobSupplierObject);
            }
        }

        public bool JobSupplierExists(BitGameJobSuppliers jobSupplier)
        {
            return _mIjobSuppliers.Any(x => x.JobSupplier == jobSupplier);
        }

        public IJobSupplierObject GetJobSupplierObject(BitGameJobSuppliers jobSupplier)
        {
            return _mIjobSuppliers.SingleOrDefault(x => x.JobSupplier == jobSupplier);
        }
    }
}