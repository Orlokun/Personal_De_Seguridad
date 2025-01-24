using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataUnits.GameCatalogues.JsonCatalogueLoaders;
using DialogueSystem;
using GameDirection;
using GameDirection.GeneralLevelManager.ShopPositions;
using GamePlayManagement.BitDescriptions.Suppliers;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.CustomerInterfaces;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Utils;
using Random = UnityEngine.Random;
namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management
{
    public class CustomersInSceneManager : MonoBehaviour, ICustomersInSceneManager
    {
        #region Singelton&Initialize
        public static ICustomersInSceneManager Instance => _mInstance;
        private static CustomersInSceneManager _mInstance;


        #endregion
        
        private ICustomerDataLoader _mCustomerDataLoader;
        

        #region Private Members
        private List<ICustomerManagementObserver> observers = new List<ICustomerManagementObserver>();

        //TODO: Get this string according to lvl
        private string BaseCustomersPath = "ClientsPrefabs/EdenCharacters/BaseCostumer";

        //Job suppliers Customer Management data
        private Dictionary<JobSupplierBitId, ICustomersInstantiationFlowData> _mClientManagementData = new Dictionary<JobSupplierBitId, ICustomersInstantiationFlowData>();
        private StoreCustomerManagementData _customerManagementRawData;
        
        
        private ICustomersInstantiationFlowData _mCurrentCustomerInstantiatorData;
        
        /// <summary>
        /// Turnis This into a class
        /// </summary>
        //Entrance points for customers
        
        private bool _mIsSpawning = false;
        private Coroutine _customersCoroutine;

        private Dictionary<Guid, IBaseCustomer> _mCustomersInScene = new Dictionary<Guid, IBaseCustomer>();
        #endregion


        [SerializeField] private ObstacleAvoidanceType avoidanceType;
        [SerializeField] private float agentSpeed;
        [SerializeField] private float agentRadius;
        
        [Header("NavMesh Configurations")]
        public float avoidancePredictionTime = 2;
        public int pathfindingIterationsPerFrame = 100;

        #region Observer
        public void RegisterObserver(ICustomerManagementObserver observer)
        {
            observers.Add(observer);
        }

        public void UnregisterObserver(ICustomerManagementObserver observer)
        {
            if (!observers.Contains(observer))
            {
                return;
            }
            observers.Remove(observer);
        }

        public void ClientReachedDestination(IBaseCustomer customerLeaving)
        {
            ClearClientFromScene(customerLeaving);
        }

        private void ClientCreatedEvent(IBaseCustomer customerData)
        {
            TrackCustomer(customerData);
            foreach (var observer in observers)
            {
                observer.UpdateCustomerAdded(customerData);
            }
        }
        private void ClearClientFromScene(IBaseCustomer removedCustomer)
        {
            if (!_mCustomersInScene.ContainsKey(removedCustomer.CustomerId))
            {
                Debug.LogWarning($"[ClearClientFromScene] Guid {removedCustomer.CustomerId} must be available before being removed");
                return;
            }
            _mCustomersInScene.Remove(removedCustomer.CustomerId);
            ClientRemovedEvent(removedCustomer.GetCustomerStoreVisitData);
        }
        private void ClientRemovedEvent(ICustomerPurchaseStealData customerData)
        {
            foreach (var observer in observers)
            {
                observer.UpdateCustomerRemoved(customerData);
            }
        }
        #endregion

        private void Awake()
        {
            if (Instance != null && _mInstance != this)
            {
                Destroy(gameObject);
            }
            _mInstance = this;
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            StartUnlockCustomerManagementData();
        }
        
        #region DataManagement
        private async void StartUnlockCustomerManagementData()
        {
            var url = DataSheetUrls.JobSuppliersCustomerManagementGameData;
            await Task.Delay(1000);
            GameDirector.Instance.ActCoroutine(LoadCustomerManagementFromWeb(url));
        }

        private IEnumerator LoadCustomerManagementFromWeb(string url)
        {
            //
            var webRequest = UnityWebRequest.Get(url);
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Jobs Catalogue management data must be reachable. Error: {webRequest.result}. {webRequest.error}");
            }
            else
            {
                var sourceJson = webRequest.downloadHandler.text;
                LoadCustomerDataFromJson(sourceJson);
            }
        }

        private void LoadCustomerDataFromJson(string sourceJson)
        {
            _mClientManagementData = new Dictionary<JobSupplierBitId, ICustomersInstantiationFlowData>();
            Debug.Log($"[LoadCustomerDataFromJson] Start Serializing customer instantiation management data.");
            _customerManagementRawData = JsonConvert.DeserializeObject<StoreCustomerManagementData>(sourceJson);
            if (_customerManagementRawData == null)
            {
                return;
            }
            for (var i = 1; i < _customerManagementRawData.values.Count; i++)
            {
                var gotId = int.TryParse(_customerManagementRawData.values[i][0], out var supplierId);
                var supplierBitId = (JobSupplierBitId) supplierId;
                if (supplierBitId == 0 || !gotId)
                {
                    Debug.LogWarning(
                        $"[CustomerInSceneManager.LoadCustomerDataFromJson] Job Supplier must have Id greater than zero");
                    return;
                }
                //var storeName = _customerManagementRawData.values[i][1]; 
                
                var gotMaxClients = int.TryParse(_customerManagementRawData.values[i][2], out var maxClients);
                if (maxClients == 0 || !gotMaxClients)
                {
                    Debug.LogWarning(
                        $"[CustomerInSceneManager.LoadProductsFromJson] Store must have max number of clients");
                    return;
                }
                var storePrefabsPath = _customerManagementRawData.values[i][3]; 
                var gotNumberOfPrefabs = int.TryParse(_customerManagementRawData.values[i][4], out var maxPrefabs);
                if (maxPrefabs == 0 || !gotNumberOfPrefabs)
                {
                    Debug.LogWarning(
                        $"[CustomerInSceneManager.LoadProductsFromJson]");
                    return;
                }
                var instantiationRange = _customerManagementRawData.values[i][5].Split(',');
                var castedRange = RangeProcessor.ProcessLinksStrings(instantiationRange);

                //TODO: GAME DIFFICULTY SHOUOLD BE SET SOMEWHERE ELSE
                var gameDifficulty = 1;
                var customerManagementData = Factory.CreateCustomersInSceneManagerData(supplierBitId, gameDifficulty, maxClients, storePrefabsPath, castedRange);
                _mClientManagementData.Add(supplierBitId, customerManagementData);
            }
        }
        #endregion

        /// <summary>
        /// Make sure a lvl with a Shop position manager is available before calling
        /// </summary>
        /// <param name="jobId"></param>
        public void LoadInstantiationProperties(JobSupplierBitId jobId)
        {
            try
            {
                if (!_mClientManagementData.ContainsKey(jobId))
                {
                    return;
                }
                _mCurrentCustomerInstantiatorData = _mClientManagementData[jobId];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public void LoadCustomerLevelStartTransforms()
        {
            try
            {
                var shopPositionManager = (IShopPositionsManager)FindFirstObjectByType<ShopPositionsManager>();
                _mCurrentCustomerInstantiatorData.SetEntrancePositions(shopPositionManager.StartPositions);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public GameObject MyGameObject => transform.gameObject;
        public void ToggleSpawning(bool isSpawning, JobSupplierBitId storeId)
        {
            if (isSpawning && _mIsSpawning)
            {
                return;
            }
            _mIsSpawning = isSpawning;
            switch (_mIsSpawning)
            {
                case false:
                    if (_customersCoroutine == null)
                        break;
                    StopCoroutine(_customersCoroutine);
                    break;
                case true:
                    LoadInstantiationProperties(storeId);
                    LoadCustomerLevelStartTransforms();
                    _customersCoroutine = StartCoroutine(StartInstantiatingClients());
                    break;
            }
        }
        private IEnumerator StartInstantiatingClients()
        {
            while (_mIsSpawning)
            {
                Random.InitState(DateTime.Now.Millisecond);
                var randomRange = Random.Range(_mCurrentCustomerInstantiatorData.InstantiationFrequencyRange[0], _mCurrentCustomerInstantiatorData.InstantiationFrequencyRange[1]);
                //var randomRange = Random.Range(3, 5);
                var randomPrefabInstantiated = InstantiateRandomClient();
                StartMovingClientAgent(randomPrefabInstantiated);
                SceneManager.MoveGameObjectToScene(randomPrefabInstantiated, SceneManager.GetSceneByName("Level_One"));
                //Debug.Log($"Instantiated Object: {randomPrefabInstantiated.name}. Waiting {_mInstantiationFrequency}");
                yield return new WaitForSeconds(randomRange);
            }
        }
        private void TrackCustomer(IBaseCustomer customer)
        {
            if (_mCustomersInScene.ContainsKey(customer.CustomerId))
            {
                Debug.LogWarning($"[TrackCustomer] Guid {customer.CustomerId} is already a tracked client");
                return;
            }
            _mCustomersInScene.Add(customer.CustomerId, customer);
        }
        private void StartMovingClientAgent(GameObject clientPrefab)
        {
            var navMesh = clientPrefab.GetComponent<NavMeshAgent>();
            navMesh.speed = 3.5f;
            navMesh.avoidancePriority = 50;
        }
        private void Update()
        {
            NavMesh.avoidancePredictionTime = avoidancePredictionTime;
            NavMesh.pathfindingIterationsPerFrame = pathfindingIterationsPerFrame;
        }
        private GameObject InstantiateRandomClient()
        {
            Random.InitState(DateTime.Now.Millisecond);
            //Random costumer index is also used as string in the clients folder
            var randomCustomer = Random.Range(1, 20);
            var randomIndex = Random.Range(0, _mCurrentCustomerInstantiatorData.GetEntrancePositions.Count - 1);
            //Choose a random position from the ones available in the screen
            var randomPositionData = _mCurrentCustomerInstantiatorData.GetEntrancePositions[randomIndex];
            
            var customerPath = BaseCustomersPath + randomCustomer;
            var randomCustomerData = Factory.CreateBaseCustomerTypeData();
            var client = (GameObject)Instantiate(Resources.Load(customerPath), randomPositionData.StartPosition, new Quaternion(0,0,0,0));
            var customerDataObj = client.GetComponent<IBaseCustomer>();
            customerDataObj.SetCustomerId(Guid.NewGuid());
            customerDataObj.SetInitialMovementData(randomPositionData);
            customerDataObj.SetCustomerTypeData(randomCustomerData);
            ClientCreatedEvent(customerDataObj);
            client.transform.Rotate(0,80,0);
            return client;
        }
        private void SetupAgent(NavMeshAgent agent)
        {
            agent.obstacleAvoidanceType = avoidanceType;
            agent.radius = agentRadius;
            agent.speed = agentSpeed;
            agent.avoidancePriority = 50;
        }
    }
}