using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameDirection.GeneralLevelManager.ShopPositions;
using GameDirection.GeneralLevelManager.ShopPositions.CustomerPois;
using GameDirection.GeneralLevelManager.ShopPositions.WaitingPositions;
using GamePlayManagement.LevelManagement.LevelObjectsManagement;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.CustomerInterfaces;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Random = UnityEngine.Random;

namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management
{
    public class BaseCustomer : BaseCharacterInScene, IBaseCustomer
    {
        #region ConstantData
        private const string ProductPrefabPath = "LevelManagementPrefabs/ProductPrefabs/";
        #endregion

        #region ProceduralAnimConstraints
        [SerializeField] protected MultiAimConstraint mHeadAimConstraint;
        [SerializeField] protected TwoBoneIKConstraint mGrabObjectConstraint;
        [SerializeField] protected TwoBoneIKConstraint mInspectObjectConstraint;
        [SerializeField] protected Transform rightHand;
        
        private bool _productInHand;

        private Tuple<Transform, IStoreProductObjectData> _tempStoreProductOfInterest;
        private Transform _tempTargetOfInterest;
        private GameObject _tempProductCopy;
        private IStoreEntrancePosition _entranceData;
        
        #endregion

        #region Characteristics and Theft
        private ICustomerTypeData _mCustomerTypeData;
        public ICustomerTypeData CustomerTypeData => _mCustomerTypeData;
        private ICustomerPurchaseStealData _mCustomerVisitData;
        public ICustomerPurchaseStealData GetCustomerStoreVisitData => _mCustomerVisitData;
        #endregion

        public Guid CustomerId => MCharacterId;
        public void SetCustomerId(Guid newId)
        {
            MCharacterId = newId;
        }

        public void SetInitialMovementData(IStoreEntrancePosition entranceData)
        {
            _entranceData = entranceData;
            MInitialPosition = entranceData.StartPosition;
        }

        public void SetCustomerTypeData(ICustomerTypeData customerTypeData)
        {
            _mCustomerTypeData = customerTypeData;
        }

        #region LevelData
        private Vector3 _mPayingPosition;
        private int _mNumberOfProductsLookingFor;
        private List<Guid> _mShelvesOfInterest;

        private bool _mAnyPoiAvailable;
        #endregion

        #region CurrentCustomerStatus
        private BaseCustomerAttitudeStatus _mCustomerAttitudeStatus = 0;
        private bool _mIsCustomerStealing;
        public bool IsCustomerStealing => _mIsCustomerStealing;

        private Dictionary<Guid, bool> _mPoisPurchaseStatus = new Dictionary<Guid, bool>();

        private Guid _currentPoiId;
        private ISingleWaitingSpot _currentWaitingSpot;
        #endregion


        #region Init
        protected override void Awake()
        {
            Random.InitState(DateTime.Now.Millisecond);
            _mCustomerVisitData = new CustomerPurchaseStealData();
            _mNumberOfProductsLookingFor = Random.Range(1, 3);
            base.Awake();
        }
        
        protected override void Start()
        {
            base.Start();
            _mPayingPosition = PositionsManager.PayingPosition();
            PopulateShelvesOfInterestData();
            SetCharacterAttitudeStatus(BaseCustomerAttitudeStatus.Entering);
            StartWalking();

            Debug.Log($"[Awake] Initial Position: {MInitialPosition}. ");
        }

        private void PopulateShelvesOfInterestData()
        {
            _mShelvesOfInterest = PositionsManager.GetFirstPoiOfInterestIds(_mNumberOfProductsLookingFor);
            foreach (var guid in _mShelvesOfInterest)
            {
                _mPoisPurchaseStatus.Add(guid, false);
            }
        }

        #endregion
        private void Update()
        {
            ManageAttitudeStatus();
            ManageMovementStatus();
        }
        protected override float GetStatusSpeed(BaseCharacterMovementStatus currentStatus)
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
        private void ManageAttitudeStatus()
        {
            switch (_mCustomerAttitudeStatus)
            {
                case BaseCustomerAttitudeStatus.Entering:
                    break;
                case BaseCustomerAttitudeStatus.Paying:
                    GoToPay();
                    break;
                case BaseCustomerAttitudeStatus.Shopping:
                    break;
                case BaseCustomerAttitudeStatus.EvaluatingProduct:
                    break;
                case BaseCustomerAttitudeStatus.Fighting:
                    break;
                case BaseCustomerAttitudeStatus.Leaving:
                    if (_mCustomerVisitData.StolenProductsValue > 0)
                    {
                        CheckRunningAwayChances();
                    }
                    break;
            }
        }

        private void CheckRunningAwayChances()
        {
            //TODO: Elaborate logic for this. For now they simply run. 
            if (Vector3.Distance(GetNavMeshAgent.destination, transform.position) < 3f)
            {
                SetCharacterMovementStatus(BaseCharacterMovementStatus.Running);
            }
        }
        
        private void ReleaseCurrentPoI()
        {
            var poi = PositionsManager.GetPoiData(_currentPoiId);
            if (!poi.IsOccupied || poi.OccupierId != MCharacterId)
            {
                return;
            }
            PositionsManager.ReleasePoi(MCharacterId, _currentPoiId);
        }
        private void GoToNextProduct()
        {
            if(_mPoisPurchaseStatus.All(x => x.Value))
            {
                Debug.Log("[GoToNextPoint] Going to Pay");
                SetCharacterAttitudeStatus(BaseCustomerAttitudeStatus.Paying);
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
            _currentPoiId = GetNotVisitedPoi(); 
            var poiObject = PositionsManager.GetPoiData(_currentPoiId);
            PositionsManager.OccupyPoi(MCharacterId, _currentPoiId);
            Debug.Log($"[GoToNextPoint] Going to Poi: {_currentPoiId}.");
            GetNavMeshAgent.SetDestination(poiObject.GetPosition);
            GetNavMeshAgent.isStopped = false;
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
                var isPoiOccupied = PositionsManager.GetPoiData(notVisitedPois[i]).IsOccupied;
                if (isPoiOccupied)
                {
                    continue;
                }
                poiObject = PositionsManager.GetPoiData(notVisitedPois[i]);
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
            var availablePoisList = PositionsManager.GetPoisOfInterestData(unPurchasedPoisKeys.ToList());
            var anyPoiAvailable = availablePoisList.Any(x => x.IsOccupied==false);
            return anyPoiAvailable;
        }

        #endregion

        #region ReachDestinationEvent
        // ReSharper disable Unity.PerformanceAnalysis
        protected override void ReachWalkingDestination()
        {
            switch (_mCustomerAttitudeStatus)
            {
                case BaseCustomerAttitudeStatus.Shopping:
                    ReachProductDestination();
                    break;
                case BaseCustomerAttitudeStatus.Paying:
                    Random.InitState(DateTime.Now.Millisecond);
                    PayAndLeave(Random.Range(5000,11000));
                    break;
                case BaseCustomerAttitudeStatus.Entering:
                    EvaluateStartShopping();
                    break;
                case BaseCustomerAttitudeStatus.Leaving:
                    CustomersInSceneManager.Instance.ClientReachedDestination(this);
                    Destroy(gameObject);
                    break;
                case BaseCustomerAttitudeStatus.Fighting:
                    break;
                case BaseCustomerAttitudeStatus.HangingAround:
                    StartCoroutine(StartHangingAround());
                    break;
            }
        }

        private IEnumerator StartHangingAround()
        {
            Debug.Log($"[StartHangingAround] Customer {gameObject.name} started hanging around.");
            yield return new WaitForSeconds(0);
        }
        
        private void ReachProductDestination()
        {
            EvaluateProduct();
        }

        private void EvaluateStartShopping()
        {
            if ((_mCustomerAttitudeStatus & BaseCustomerAttitudeStatus.Shopping) != 0)
            {
                return;
            }
            var pois = PositionsManager.GetPoisOfInterestData(_mShelvesOfInterest);
            _mAnyPoiAvailable =  pois.Any(x=> x.IsOccupied == false);
            if (!_mAnyPoiAvailable)
            {
                Debug.Log("No shelve has a Poi available. Will Walk around and try to interact.");
                StartFreeClientInteraction();
                return;                
            }
            StartShopping();
        }

        private void StartFreeClientInteraction()
        {
            Debug.Log("[StartFreeClientInteraction] Start Looking for chill place");
            var occupiedWaitingSpot = PositionsManager.OccupyEmptyWaitingSpot(CharacterId);
            if (occupiedWaitingSpot.Result.Item2 == false)
            {
                Debug.LogWarning("[StartFreeClientInteraction] No Empty waiting spot available. Making excuse and retiring back home");
                MakeExcuse();
                Leave();
            }
            WalkToWaitSpot(occupiedWaitingSpot.Result.Item1);
        }

        private void WalkToWaitSpot(ISingleWaitingSpot targetWaitSpot)
        {
            _currentWaitingSpot = targetWaitSpot;
            SetCharacterAttitudeStatus(BaseCustomerAttitudeStatus.HangingAround);
            SetCharacterMovementStatus(BaseCharacterMovementStatus.Walking);
        }

        private void MakeExcuse()
        {
            
        }

        private void StartShopping()
        {
            SetCharacterAttitudeStatus(BaseCustomerAttitudeStatus.Shopping);
            SetCharacterMovementStatus(BaseCharacterMovementStatus.Walking);
            GoToNextProduct();
        }
        #region ProductStealEvaluation
        private void EvaluateProduct()
        {
            SetCharacterMovementStatus(BaseCharacterMovementStatus.Idle);
            SetCharacterAttitudeStatus(BaseCustomerAttitudeStatus.EvaluatingProduct);
            var wouldStealProduct = EvaluateProductStealingChances();
            StartProductExamination(wouldStealProduct);
        }
        private bool EvaluateProductStealingChances()
        {
            var hasStealAbility = _tempStoreProductOfInterest.Item2.HideChances <= _mCustomerTypeData.StealAbility ? 1 : 0;
            var isTempting = _tempStoreProductOfInterest.Item2.Tempting >= _mCustomerTypeData.Corruptibility ? 1 : 0;
            var isDetermined = _tempStoreProductOfInterest.Item2.Punishment <= _mCustomerTypeData.Fearful ? 1 : 0;

            return hasStealAbility + isTempting + isDetermined >= 2;
        }
        
        private async void StartProductExamination(bool wouldStealProduct)
        {
            Random.InitState(DateTime.Now.Millisecond);
            //Wait To play Grab Animation
            await Task.Delay(Random.Range(1500, 2000));
            mGrabObjectConstraint.data.target = _tempTargetOfInterest.transform;
            StartCoroutine(SetGrabObjectConstraint(0,1,1));
            //Wait to instantiate when object is grabbed and look at it
            await Task.Delay(1000);
            InstantiateProductInHand();
            StartInspectObjectAnim();
            await Task.Delay(1000);
            //BaseAnimator.ChangeAnimationState(EvaluateProductObject);
            if (!wouldStealProduct)
            {
                AddProductAndKeepShopping();
            }
            else
            {
                StartStealingProductAttempt();
            }
        }
        private void InstantiateProductInHand()
        {
            var path = ProductPrefabPath + _tempStoreProductOfInterest.Item2.PrefabName;
            _tempProductCopy = (GameObject)Instantiate(Resources.Load(path),rightHand, false);
            _productInHand = true;
            _tempProductCopy.transform.localScale *= .4f;
            _tempProductCopy.transform.localPosition = new Vector3(0,0,0);
            _tempTargetOfInterest = _tempProductCopy.transform;
        }
        private void StartInspectObjectAnim()
        {
            StartCoroutine(SetGrabObjectConstraint(1, 0, 1));
            StartCoroutine(SetLookObjectWeight(0,1,1));
            StartCoroutine(UpdateInspectObjectRigWeight(0, 1, 1));
        }
        private IEnumerator SetLookObjectWeight(float start, float end,float time)
        {
            float elapsedTime = 0;
            while (elapsedTime < time)
            {
                mHeadAimConstraint.weight = Mathf.Lerp(start, end, (elapsedTime / time));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        private IEnumerator UpdateInspectObjectRigWeight(float start, float end,float time)
        {
            float elapsedTime = 0;
            while (elapsedTime < time)
            {
                mInspectObjectConstraint.weight = Mathf.Lerp(start, end, (elapsedTime / time));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        private IEnumerator SetGrabObjectConstraint(float start, float end,float time)
        {
            float elapsedTime = 0;
            while (elapsedTime < time)
            {
                mGrabObjectConstraint.weight = Mathf.Lerp(start, end, (elapsedTime / time));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        
        
        private async void AddProductAndKeepShopping()
        {
            Debug.Log("[AddProductAndKeepShopping] WOULD NOT STEAL PRODUCT");
            await Task.Delay(Random.Range(4500, 10000));
            StartCoroutine(UpdateInspectObjectRigWeight(1, 0, 1));
            _mCustomerVisitData.PurchaseProduct(Guid.NewGuid(), _tempStoreProductOfInterest.Item2);
            ClearProductInterest();
            _mPoisPurchaseStatus[_currentPoiId] = true;
            SetCharacterMovementStatus(BaseCharacterMovementStatus.Walking);
            SetCharacterAttitudeStatus(BaseCustomerAttitudeStatus.Shopping);
            ReleaseCurrentPoI();
            GoToNextProduct();
        }
        
        private async void StartStealingProductAttempt()
        {
            Debug.Log($"[StartProductExamination] {gameObject.name} WOULD STEAL PRODUCT. Start Process");
            await Task.Delay(Random.Range(1000, 1500));
            
            StartCoroutine(SetLookObjectWeight(1,0,1.5f));
            BaseAnimator.ChangeAnimationState(SearchAround);
            await Task.Delay(8000);
            StartCoroutine(UpdateInspectObjectRigWeight(1, 0, 1));
            _mCustomerVisitData.StealProduct(Guid.NewGuid(), _tempStoreProductOfInterest.Item2);
            //Debug.Log($"{gameObject.name} stole a {_tempStoreProductOfInterest.Item2.ProductName}!");
            ClearProductInterest();
            _mPoisPurchaseStatus[_currentPoiId] = true;
            SetCharacterMovementStatus(BaseCharacterMovementStatus.Walking);
            SetCharacterAttitudeStatus(BaseCustomerAttitudeStatus.Shopping);
            ReleaseCurrentPoI();
            GoToNextProduct();
        }
        private void ClearProductInterest()
        {
            Destroy(_tempProductCopy);
            _tempTargetOfInterest = null;
            _productInHand = false;
            _tempProductCopy = null;
        }
        #endregion

        #endregion

        #region UpdateMovementStatus
        private void ManageMovementStatus()
        {
            switch (MCharacterMovementStatus)
            {
                case BaseCharacterMovementStatus.Idle:
                    break;
                case BaseCharacterMovementStatus.Walking:
                    EvaluateWalkingDestination();
                    break;
                case BaseCharacterMovementStatus.EvaluatingProduct:
                    if (_productInHand)
                    {
                        break;
                    }
                    RotateTowardsYOnly(transform,_tempTargetOfInterest);
                    break;
                case BaseCharacterMovementStatus.Running:
                    EvaluateWalkingDestination();
                    break;
            }
        }
        
        #endregion
        
        #region Paying
        private void GoToPay()
        {
            if ((_mCustomerAttitudeStatus & BaseCustomerAttitudeStatus.Paying) != 0)
            {
                return;   
            }
            GetNavMeshAgent.destination = _mPayingPosition;
        }
        private async void PayAndLeave(int timePaying)
        {
            Pay();
            //Time to do something
            await Task.Delay(timePaying);
            Leave();
        }

        private void Pay()
        {
            SetCharacterMovementStatus(BaseCharacterMovementStatus.Idle);
            SetCharacterAttitudeStatus(BaseCustomerAttitudeStatus.Paying);
            GetNavMeshAgent.isStopped = true;
        }

        private void Leave()
        {
            SetCharacterMovementStatus(BaseCharacterMovementStatus.Walking);
            SetCharacterAttitudeStatus(BaseCustomerAttitudeStatus.Leaving);
        }
        
        #endregion

        #region Utils
        protected override void SetCharacterAttitudeStatus(BaseCustomerAttitudeStatus newCustomerAttitude)
        {
            _mCustomerAttitudeStatus = 0;
            _mCustomerAttitudeStatus |= newCustomerAttitude;
            switch (newCustomerAttitude)
            {
                case BaseCustomerAttitudeStatus.EvaluatingProduct:
                    var pointOfInterest = PositionsManager.GetPoiData(_currentPoiId);
                    _tempStoreProductOfInterest = pointOfInterest.ChooseRandomProduct();
                    _tempTargetOfInterest = _tempStoreProductOfInterest.Item1;
                    break;
                
                case BaseCustomerAttitudeStatus.Paying:
                    _tempStoreProductOfInterest = null;
                    SetMovementDestination(_mPayingPosition);
                    break;
                
                case BaseCustomerAttitudeStatus.Leaving:
                    SetMovementDestination(MInitialPosition);
                    break;
                
                case BaseCustomerAttitudeStatus.Entering:
                    SetMovementDestination(_entranceData.EntrancePosition);
                    break;
                case BaseCustomerAttitudeStatus.HangingAround:
                    SetMovementDestination(_currentWaitingSpot.Position);
                    break;
            }
        }
        #endregion
    }
}