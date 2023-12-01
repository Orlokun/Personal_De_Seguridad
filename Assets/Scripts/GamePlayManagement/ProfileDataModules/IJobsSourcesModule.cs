using System.Collections.Generic;
using DataUnits.JobSources;
using GamePlayManagement.BitDescriptions.Suppliers;

namespace GamePlayManagement.ProfileDataModules
{
    public interface IJobsSourcesModule : IProfileModule
    {
        void UnlockJobSupplier(BitGameJobSuppliers gainedJobSupplier);
        public int ElementsActive { get; }
        Dictionary<BitGameJobSuppliers, IJobSupplierObject> JobObjects { get; }
        public BitGameJobSuppliers CurrentEmployer { get; }
        public void SetNewEmployer(BitGameJobSuppliers newEmployer);
    }
}