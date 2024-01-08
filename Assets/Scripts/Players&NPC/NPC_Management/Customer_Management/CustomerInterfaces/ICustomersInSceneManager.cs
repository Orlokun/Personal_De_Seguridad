using GamePlayManagement.BitDescriptions.Suppliers;
using UnityEngine;

namespace Players_NPC.NPC_Management.Customer_Management.CustomerInterfaces
{
    public interface ICustomersInSceneManager
    {
        void ToggleSpawning(bool isSpawning);
        public void RegisterObserver(ICustomerManagementObserver observer);
        public void UnregisterObserver(ICustomerManagementObserver observer);
        public void ClientReachedDestination(IBaseCustomer customerLeaving);
        public void LoadInstantiationProperties(JobSupplierBitId supplierId);
        public void LoadCustomerLevelStartTransforms();
        public GameObject MyGameObject { get; }

    }

    public class CustomersInSceneManagerData : ICustomersInSceneManagerData
    {
        public int[] InstantiationFrequencyRange =>_mInstantiationFrequencyRange;
        public string ClientPrefabPaths => _clientsPrefabsPath;
        public int GameDifficultyLvl => _mGameDifficultyLvl;

        
        private int[] _mInstantiationFrequencyRange;
        private int _mGameDifficultyLvl;
        private string _clientsPrefabsPath;
        private int maxClients;

        private JobSupplierBitId _mJobId;
        public JobSupplierBitId JobId => _mJobId;

        public CustomersInSceneManagerData(JobSupplierBitId jobId, int gameDifficultyLvl, int maxClients, string clientsPrefabsPath, int[] timeRange)
        {
            _mInstantiationFrequencyRange = timeRange;
            _mGameDifficultyLvl = gameDifficultyLvl;
            _clientsPrefabsPath = clientsPrefabsPath;
            _mJobId = jobId;
        }
    }
}