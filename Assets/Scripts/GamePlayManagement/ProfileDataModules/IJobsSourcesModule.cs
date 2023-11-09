using System.Collections.Generic;
using DataUnits;
using DataUnits.ItemScriptableObjects;
using GamePlayManagement.BitDescriptions.Suppliers;

namespace GamePlayManagement.ProfileDataModules
{
    public interface IJobsSourcesModule : IProfileModule
    {
        void UnlockJobModule(BitGameJobSuppliers gainedJobSupplier);
        public int ElementsActive { get; }
        Dictionary<BitGameJobSuppliers, IJobSupplierObject> JobObjects { get; }
    }
}