using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameDirection.GeneralLevelManager.ShopPositions.CustomerPois;
using GameDirection.GeneralLevelManager.ShopPositions.WaitingPositions;
using GamePlayManagement.LevelManagement.LevelObjectsManagement;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.CustomerInterfaces;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.AttitudeStates;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.MovementStates;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management
{
    public class BaseCustomer : BaseCharacterInScene, IBaseCustomer
    {
        #region ConstantData
        private const string ProductPrefabPath = "LevelManagementPrefabs/ProductPrefabs/";
        #endregion

        private bool _productInHand;
        public bool HasProductInHand => _productInHand;

        private GameObject _tempProductCopy;
        
        private ICustomerInstanceData _mCustomerInstanceData;
        private ICustomersInSceneManager _mCustomersManager;

        

        #region Customer Data, Stats, Current Stolen Elements and Ethics
        private ICustomerTypeData _mCustomerTypeData;
        public ICustomerTypeData CustomerTypeData => _mCustomerTypeData;
        private ICustomerPurchaseStealData _mCustomerVisitData;
        public ICustomerPurchaseStealData GetCustomerStoreVisitData => _mCustomerVisitData;
        #endregion

        public Guid CustomerId => MCharacterId;


        #region LevelData
        private Vector3 _mPayingPosition;
        private int _mNumberOfProductsLookingFor;
        private List<Guid> _mShelvesOfInterest;
        public Vector3 PayingPosition => _mPayingPosition;

        private bool _mAnyPoiAvailable;
        #endregion

        #region CurrentCustomerStatus
        private BaseCharacterAttitudeStatus _mCharacterAttitudeStatus = 0;
        private bool _mIsCustomerStealing;
        public bool IsCustomerStealing => _mIsCustomerStealing;
        public GameObject CustomerGameObject => gameObject;


        private Dictionary<Guid, bool> _mPoisPurchaseStatus = new Dictionary<Guid, bool>();
        public Dictionary<Guid, bool> PoisPurchaseStatus=> _mPoisPurchaseStatus;

        private Guid _mCurrentPoiId;
        public Guid MCurrentPoiId => _mCurrentPoiId;
        private ISingleWaitingSpot _currentWaitingSpot;
        #endregion


        #region Init
        protected override void Awake()
        {
            Random.InitState(DateTime.Now.Millisecond);
            _mCustomerVisitData = new CustomerPurchaseStealData();
            _mNumberOfProductsLookingFor = Random.Range(1, 3);
            base.Awake();
            InitiateCustomerStateMachines();
        }

        private void InitiateCustomerStateMachines()
        {
            _mAttitudeStateMachine.AddState(new AccessingBuildingState(this));
            _mAttitudeStateMachine.AddState(new ShoppingState(this));
            _mAttitudeStateMachine.AddState(new StealingState(this));
            _mAttitudeStateMachine.AddState(new EvaluatingProductState(this));
            _mAttitudeStateMachine.AddState(new PayingState(this));
            _mAttitudeStateMachine.AddState(new LeavingBuildingState(this));
        }

        protected override void Start()
        {
            base.Start();
            _mPayingPosition = MPositionsManager.PayingPosition();
            PopulateShelvesOfInterestData();
            ChangeMovementState<IdleMovementState>();
            ChangeAttitudeState<AccessingBuildingState>();

            Debug.Log($"[Awake] Initial Position: {MInitialPosition}. ");
        }

        private void PopulateShelvesOfInterestData()
        {
            _mShelvesOfInterest = MPositionsManager.GetFirstPoiOfInterestIds(_mNumberOfProductsLookingFor);
            foreach (var guid in _mShelvesOfInterest)
            {
                _mPoisPurchaseStatus.Add(guid, false);
            }
        }
        public void SetTempProductOfInterest(Tuple<Transform, IStoreProductObjectData> productOfInterest)
        {
            MTempStoreProductOfInterest = productOfInterest;
            MTempTargetOfInterest = MTempStoreProductOfInterest.Item1;
        }
        #endregion
        private void Update()
        {
            _mMovementStateMachine.Update();
            _mAttitudeStateMachine.Update();
        }
        protected float GetStatusSpeed(BaseCharacterMovementStatus currentStatus)
        {
            var guardSpeed = (float)_mCustomerTypeData.Speed / 10;
            switch (currentStatus)
            {
                case BaseCharacterMovementStatus.Walking:
                    // ReSharper disable once PossibleLossOfFraction
                    return BaseWalkSpeed * guardSpeed;
                case BaseCharacterMovementStatus.Running:
                    // ReSharper disable once PossibleLossOfFraction
                    return BaseRunSpeed * guardSpeed;
                default:
                    return 1;
            }
        }
        #region UpdateManageAttitude


        private void CheckRunningAwayChances()
        {
            //TODO: Elaborate logic for this. For now they simply run. 
            if (Vector3.Distance(GetNavMeshAgent.destination, transform.position) < 3f)
            {
                ChangeMovementState<RunningState>();
            }
        }
        
        public void ReleaseCurrentPoI()
        {
            var poi = MPositionsManager.GetPoiData(_mCurrentPoiId);
            if (!poi.IsOccupied || poi.OccupierId != MCharacterId)
            {
                return;
            }
            MPositionsManager.ReleasePoi(MCharacterId, _mCurrentPoiId);
        }
        public void GoToNextProduct()
        {
            if(_mPoisPurchaseStatus.All(x => x.Value))
            {
                Debug.Log("[GoToNextPoint] Going to Pay");
                ChangeAttitudeState<PayingState>();
                ReleaseCurrentPoI();
                return;
            }
            var anyPoiAvailable = CheckIfPoisAvailable();
            if (!anyPoiAvailable)
            {
                //TODO: Walk randomly and find something to do
                Debug.Log($"[GoToNextProduct] No Pois are available for customer {gameObject.name} - ID: {MCharacterId}");
                return;
            }
            _mCurrentPoiId = GetNotVisitedPoi(); 
            var poiObject = MPositionsManager.GetPoiData(_mCurrentPoiId);
            MPositionsManager.OccupyPoi(MCharacterId, _mCurrentPoiId);
            Debug.Log($"[GoToNextPoint] Going to Poi: {_mCurrentPoiId}.");
            SetMovementDestination(poiObject.GetPosition);
        }

        private Guid GetNotVisitedPoi()
        {
            var notVisitedPois = new List<Guid>();
            foreach (var poi in _mPoisPurchaseStatus)
            {
                if (poi.Value == false)
                {
                    notVisitedPois.Add(poi.Key);
                }
            }
            var notVisitedPoisCount = notVisitedPois.Count;
            if (notVisitedPois.Count == 0)
            {
                Debug.LogWarning("[GetNotVisitedPoi] Not visited Poi must be more than 0");
                return new Guid();
            }

            IShopPoiData poiObject = null;
            for (int i = 0; i < notVisitedPoisCount; i++)
            {
                var isPoiOccupied = MPositionsManager.GetPoiData(notVisitedPois[i]).IsOccupied;
                if (isPoiOccupied)
                {
                    continue;
                }
                poiObject = MPositionsManager.GetPoiData(notVisitedPois[i]);
            }
            
            if (poiObject == null)
            {
                Debug.LogWarning("[GetNotVisitedPoi] Not visited Poi must not be null");
                return new Guid();
            }
            return poiObject.PoiId;
        }

        private bool CheckIfPoisAvailable()
        {
            var unPurchasedPoiIds = _mPoisPurchaseStatus.Where(x => x.Value == false);
            var unPurchasedPoisKeys = unPurchasedPoiIds.Select(x => x.Key);
            var availablePoisList = MPositionsManager.GetPoisOfInterestData(unPurchasedPoisKeys.ToList());
            var anyPoiAvailable = availablePoisList.Any(x => x.IsOccupied==false);
            return anyPoiAvailable;
        }

        #endregion

        #region ReachDestinationEvent
        // ReSharper disable Unity.PerformanceAnalysis

        private IEnumerator StartHangingAround()
        {
            Debug.Log($"[StartHangingAround] Customer {gameObject.name} started hanging around.");
            yield return new WaitForSeconds(0);
        }
        
        public void EvaluateStartShopping()
        {
            var pois = MPositionsManager.GetPoisOfInterestData(_mShelvesOfInterest);
            _mAnyPoiAvailable =  pois.Any(x=> x.IsOccupied == false);
            if (!_mAnyPoiAvailable)
            {
                Debug.Log("No shelve has a Poi available. Will Walk around and try to interact.");
                StartFreeClientInteraction();
                return;                
            }
            StartShopping();
        }
        private void StartShopping()
        {
            _mAttitudeStateMachine.ChangeState<ShoppingState>();
            _mMovementStateMachine.ChangeState<WalkingState>();
            GoToNextProduct();
        }
        private void StartFreeClientInteraction()
        {
            Debug.Log("[StartFreeClientInteraction] Start Looking for chill place");
            var occupiedWaitingSpot = MPositionsManager.OccupyEmptyWaitingSpot(CharacterId);
            if (occupiedWaitingSpot.Result.Item2 == false)
            {
                Debug.LogWarning("[StartFreeClientInteraction] No Empty waiting spot available. Making excuse and retiring back home");
                MakeExcuse();
                return;
            }
            WalkToWaitSpot(occupiedWaitingSpot.Result.Item1);
        }

        private void WalkToWaitSpot(ISingleWaitingSpot targetWaitSpot)
        {
            _currentWaitingSpot = targetWaitSpot;
            //ChangeCharacterAttitudeState(BaseCharacterAttitudeStatus.WonderingAround);
            //SetCharacterMovementStatus(BaseCharacterMovementStatus.Walking);
        }

        private void MakeExcuse()
        {
            
        }


        #region ProductStealEvaluation
        private void EvaluateProduct()
        {
            ChangeAttitudeState<EvaluatingProductState>();
        }


        public void InstantiateProductInHand()
        {
            var path = ProductPrefabPath + MTempStoreProductOfInterest.Item2.PrefabName;
            _tempProductCopy = (GameObject)Instantiate(Resources.Load(path),rightHand, false);
            _productInHand = true;
            _tempProductCopy.transform.localScale *= .4f;
            _tempProductCopy.transform.localPosition = new Vector3(0,0,0);
            MTempTargetOfInterest = _tempProductCopy.transform;
        }
        #endregion

        #endregion

        public override void ClearProductOfInterest()
        {
            if (_tempProductCopy is not null)
            {
                Destroy(MTempTargetOfInterest.gameObject);
            }
            base.ClearProductOfInterest();
            MTempTargetOfInterest = null;
            _productInHand = false;
            _tempProductCopy = null;
        }

        private bool _mInitialized;
        public bool IsInitialized => _mInitialized;
        
        public void Initialize(ICustomerInstanceData injectionClass)
        {
            if (_mInitialized)
            {
                return;
            }
            _mCustomersManager = injectionClass.CustomerManager;
            MCharacterId = injectionClass.CustomerId;
            MEntranceData = injectionClass.EntrancePositions;
            MInitialPosition = MEntranceData.StartPosition;
            _mCustomerTypeData = injectionClass.CustomerTypeData;
            _mInitialized = true;
        }

        public void ClearCustomer()
        {
            _mCustomersManager.ClientReachedDestination(this);
        }
    }
}