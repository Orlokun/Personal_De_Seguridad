using System.Collections.Generic;
using GamePlayManagement.LevelManagement.LevelObjectsManagement;
using Players_NPC.NPC_Management.Customer_Management;
using Players_NPC.NPC_Management.Customer_Management.CustomerInterfaces;

namespace DataUnits.JobSources
{
    public interface IJobSupplierCustomerManagementModule
    {
        public void LoadCustomerManagementData();
        public ICustomersInSceneManagerData GetCustomerManagementData { get; }
        public void StartUnlockCustomerManagementData();

    }
    public interface IJobSupplierProductsModule
    {
        public void LoadProductData();
        public Dictionary<ProductsLevelEden, IStoreProductObjectData> ProductsInStore { get; }
    }
}