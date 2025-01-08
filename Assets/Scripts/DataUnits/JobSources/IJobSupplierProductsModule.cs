using System.Collections.Generic;
using GamePlayManagement.LevelManagement.LevelObjectsManagement;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.CustomerInterfaces;

namespace DataUnits.JobSources
{
    public interface IJobSupplierCustomerManagementModule
    {
        public void LoadCustomerManagementData();
        public ICustomersInstantiationFlowData GetICustomerManagementData { get; }
        public void StartUnlockCustomerManagementData();

    }
    public interface IJobSupplierProductsModule
    {
        public void LoadProductData();
        public Dictionary<ProductsLevelEden, IStoreProductObjectData> ProductsInStore { get; }
    }
}