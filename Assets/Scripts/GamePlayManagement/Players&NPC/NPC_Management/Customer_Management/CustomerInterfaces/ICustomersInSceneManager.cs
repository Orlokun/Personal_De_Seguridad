using GamePlayManagement.BitDescriptions.Suppliers;
using UnityEngine;

namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.CustomerInterfaces
{
    public interface ICustomersInSceneManager
    {
        public GameObject MyGameObject { get; }
        void ToggleSpawning(bool isSpawning, JobSupplierBitId storeId);
        public void RegisterObserver(ICustomerManagementObserver observer);
        public void UnregisterObserver(ICustomerManagementObserver observer);
        public void ClientReachedDestination(IBaseCustomer interestLeaving);
        public void LoadInstantiationProperties(JobSupplierBitId supplierId);
        public void LoadCustomerLevelStartTransforms();
    }
}