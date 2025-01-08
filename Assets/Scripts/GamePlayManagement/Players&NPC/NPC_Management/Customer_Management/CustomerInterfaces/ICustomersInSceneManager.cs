using GamePlayManagement.BitDescriptions.Suppliers;
using UnityEngine;

namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.CustomerInterfaces
{
    public interface ICustomersInSceneManager
    {
        public GameObject MyGameObject { get; }
        void ToggleSpawning(bool isSpawning);
        public void RegisterObserver(ICustomerManagementObserver observer);
        public void UnregisterObserver(ICustomerManagementObserver observer);
        public void ClientReachedDestination(IBaseCustomer customerLeaving);
        public void LoadInstantiationProperties(JobSupplierBitId supplierId);
        public void LoadCustomerLevelStartTransforms();
    }

    public class CustomersInstantiationFlowData : ICustomersInstantiationFlowData
    {
        private int maxClients;

        public CustomersInstantiationFlowData(JobSupplierBitId jobId, int gameDifficultyLvl, int maxClients,
            string clientsPrefabsPath, int[] timeRange)
        {
            InstantiationFrequencyRange = timeRange;
            GameDifficultyLvl = gameDifficultyLvl;
            ClientPrefabPaths = clientsPrefabsPath;
            JobId = jobId;
        }

        public int[] InstantiationFrequencyRange { get; }

        public string ClientPrefabPaths { get; }

        public int GameDifficultyLvl { get; }

        public JobSupplierBitId JobId { get; }
    }
}