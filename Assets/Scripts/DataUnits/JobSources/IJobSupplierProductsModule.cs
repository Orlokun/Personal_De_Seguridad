using System.Collections.Generic;
using GamePlayManagement.LevelManagement.LevelObjectsManagement;

namespace DataUnits.JobSources
{
    public interface IJobSupplierProductsModule
    {
        public void LoadProductData();
        public Dictionary<int, IStoreProductObjectData> ProductsInStore { get; }
    }
}