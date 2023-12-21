using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameDirection.GeneralLevelManager;
using GamePlayManagement.LevelManagement.LevelObjectsManagement;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Utils;
using Random = UnityEngine.Random;

namespace Players_NPC.NPC_Management.Customer_Management
{
    public class BaseCustomer : BaseCharacterInScene, IBaseCustomer
    {
        #region ConstantData
        private const string ProductPrefabPath = "LevelManagementPrefabs/ProductPrefabs/";
        private const string SearchAround = "SearchAround";
        #endregion

        #region ProceduralAnimConstraints
        [SerializeField] protected MultiAimConstraint mHeadAimConstraint;
        [SerializeField] protected TwoBoneIKConstraint MGrabObjectConstraint;
        [SerializeField] protected TwoBoneIKConstraint MInspectObjectConstraint;
        [SerializeField] protected Transform rightHand;
        
        private bool _productInHand;

        private Tuple<Transform, IStoreProductObjectData> _tempStoreProductOfInterest;
        private Transform _tempTargetOfInterest;
        private GameObject _tempProductCopy;
        
        #endregion
        
        private ICustomerTypeData _mCustomerTypeData;
        public ICustomerTypeData CustomerTypeData => _mCustomerTypeData;
        
        #region LevelData
        private Vector3 _mPayingPosition;
        private int _mNumberOfProductsLookingFor;
        private List<Guid> _mShelvesOfInterest;

        private bool _mAnyPoiAvailable;
        #endregion

        #region CurrentCustomerStatus
        private BaseCustomerMovementStatus _mCustomerMovementStatus = 0;
        private BaseAttitudeStatus _mCustomerAttitudeStatus = 0;

        private Dictionary<Guid, bool> _mPoisPurchaseStatus = new Dictionary<Guid, bool>();
        private Dictionary<Guid, IStoreProductObjectData> _mStolenProducts = new Dictionary<Guid, IStoreProductObjectData>();
        private Dictionary<Guid, IStoreProductObjectData> _mPurchasedProducts = new Dictionary<Guid, IStoreProductObjectData>();

        private Guid _currentPoiId;
        #endregion

        #region Events
        private delegate void ReachDestination();
        private event ReachDestination WalkingDestinationReached;

        #endregion

        #region Init
        protected override void Awake()
        {
            Random.InitState(DateTime.Now.Millisecond);
            _mNumberOfProductsLookingFor = Random.Range(1, 8);
            base.Awake();
            _mCustomerTypeData = Factory.CreateBaseCustomerTypeData();
            WalkingDestinationReached += ReachWalkingDestination;
        }
        
        protected override void Start()
        {
            base.Start();
            _mPayingPosition = _positionsManager.PayingPosition();
            PopulateShelvesOfInterestData();
            SetCustomerAttitudeStatus(BaseAttitudeStatus.Entering);
            StartWalking();

            Debug.Log($"[Awake] Initial Position: {MInitialPosition}. ");
        }

        private void PopulateShelvesOfInterestData()
        {
            _mShelvesOfInterest = _positionsManager.GetFirstPoiOfInterestIds(_mNumberOfProductsLookingFor);
            foreach (var guid in _mShelvesOfInterest)
            {
                _mPoisPurchaseStatus.Add(guid, false);
            }
        }
        private void StartWalking()
        {
            SetCustomerMovementStatus(BaseCustomerMovementStatus.Walking);
            BaseAnimator.ChangeAnimationState(WALK);
        }
        #endregion
        private void Update()
        {
            ManageAttitudeStatus();
            ManageMovementStatus();
        }

        #region UpdateManageAttitude
        private void ManageAttitudeStatus()
        {
            switch (_mCustomerAttitudeStatus)
            {
                case BaseAttitudeStatus.Entering:
                    break;
                case BaseAttitudeStatus.Paying:
                    GoToPay();
                    break;
                case BaseAttitudeStatus.Shopping:
                    break;
                case BaseAttitudeStatus.EvaluatingProduct:
                    break;
                case BaseAttitudeStatus.Fighting:
                    break;
                case BaseAttitudeStatus.Leaving:
                    break;
            }
        }
        private void ReleaseCurrentPoI()
        {
            var poi = _positionsManager.GetPoiData(_currentPoiId);
            if (!poi.IsOccupied || poi.OccupierId != MCustomerId)
            {
                return;
            }
            _positionsManager.ReleasePoi(MCustomerId, _currentPoiId);
        }
        private void GoToNextProduct()
        {
            if(_mPoisPurchaseStatus.All(x => x.Value != false))
            {
                Debug.Log("[GoToNextPoint] Going to Pay");
                SetCustomerAttitudeStatus(BaseAttitudeStatus.Paying);
                ReleaseCurrentPoI();
                return;
            }
            var anyPoiAvailable = CheckIfPoisAvailable();
            if (!anyPoiAvailable)
            {
                //TODO: Walk randomly and find something to do
                Debug.Log($"[GoToNextProduct] No Pois are available for customer {gameObject.name} - ID: {MCustomerId}");
                return;
            }
            _currentPoiId = GetNotVisitedPoi(); 
            var poiObject = _positionsManager.GetPoiData(_currentPoiId);
            _positionsManager.OccupyPoi(MCustomerId, _currentPoiId);
            Debug.Log($"[GoToNextPoint] Going to Poi: {_currentPoiId}.");
            NavMeshAgent.SetDestination(poiObject.GetPosition);
            NavMeshAgent.isStopped = false;
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
                var isPoiOccupied = _positionsManager.GetPoiData(notVisitedPois[i]).IsOccupied;
                if (isPoiOccupied)
                {
                    continue;
                }
                poiObject = _positionsManager.GetPoiData(notVisitedPois[i]);
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
            var availablePoisList = _positionsManager.GetPoisOfInterestData(unPurchasedPoisKeys.ToList());
            var anyPoiAvailable = availablePoisList.Any(x => x.IsOccupied==false);
            return anyPoiAvailable;
        }

        #endregion

        #region ReachDestinationEvent
        private void ReachWalkingDestination()
        {
            switch (_mCustomerAttitudeStatus)
            {
                case BaseAttitudeStatus.Shopping:
                    ReachProductDestination();
                    break;
                case BaseAttitudeStatus.Paying:
                    Random.InitState(DateTime.Now.Millisecond);
                    PayAndLeave(Random.Range(5000,11000));
                    break;
                case BaseAttitudeStatus.Entering:
                    EvaluateStartShopping();
                    break;
                case BaseAttitudeStatus.Leaving:
                    Destroy(gameObject);
                    break;
                case BaseAttitudeStatus.Fighting:
                    break;
            }
        }
        private void ReachProductDestination()
        {
            EvaluateProduct();
        }

        private void EvaluateStartShopping()
        {
            if ((_mCustomerAttitudeStatus & BaseAttitudeStatus.Shopping) != 0)
            {
                return;
            }
            var pois = _positionsManager.GetPoisOfInterestData(_mShelvesOfInterest);
            _mAnyPoiAvailable =  pois.Any(x=> x.IsOccupied == false);
            if (!_mAnyPoiAvailable)
            {
                Debug.Log("No shelve has a Poi available. Will Walk around and try to interact.");
                //TODO: SetNewStateBehavior
                return;                
            }
            StartShopping();
        }
        private void StartShopping()
        {
            SetCustomerAttitudeStatus(BaseAttitudeStatus.Shopping);
            SetCustomerMovementStatus(BaseCustomerMovementStatus.Walking);
            GoToNextProduct();
        }
        #region ProductStealEvaluation
        private void EvaluateProduct()
        {
            SetCustomerMovementStatus(BaseCustomerMovementStatus.EvaluatingProduct);
            SetCustomerAttitudeStatus(BaseAttitudeStatus.EvaluatingProduct);
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
            MGrabObjectConstraint.data.target = _tempTargetOfInterest.transform;
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
                MInspectObjectConstraint.weight = Mathf.Lerp(start, end, (elapsedTime / time));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        private IEnumerator SetGrabObjectConstraint(float start, float end,float time)
        {
            float elapsedTime = 0;
            while (elapsedTime < time)
            {
                MGrabObjectConstraint.weight = Mathf.Lerp(start, end, (elapsedTime / time));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        
        
        private async void AddProductAndKeepShopping()
        {
            Debug.Log("[AddProductAndKeepShopping] WOULD NOT STEAL PRODUCT");
            await Task.Delay(Random.Range(4500, 10000));
            StartCoroutine(UpdateInspectObjectRigWeight(1, 0, 1));
            _mPurchasedProducts.Add(Guid.NewGuid(), _tempStoreProductOfInterest.Item2);
            ClearProductInterest();
            _mPoisPurchaseStatus[_currentPoiId] = true;
            SetCustomerMovementStatus(BaseCustomerMovementStatus.Walking);
            SetCustomerAttitudeStatus(BaseAttitudeStatus.Shopping);
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
            _mStolenProducts.Add(Guid.NewGuid(), _tempStoreProductOfInterest.Item2);
            Debug.Log($"{gameObject.name} stole a {_tempStoreProductOfInterest.Item2.ProductName}!");
            ClearProductInterest();
            SetCustomerMovementStatus(BaseCustomerMovementStatus.Walking);
            SetCustomerAttitudeStatus(BaseAttitudeStatus.Shopping);
            _mPoisPurchaseStatus[_currentPoiId] = true;
            ReleaseCurrentPoI();
            GoToNextProduct();
        }
        #endregion


        private void ClearProductInterest()
        {
            Destroy(_tempProductCopy);
            _tempTargetOfInterest = null;
            _productInHand = false;
            _tempProductCopy = null;
        }
        #endregion

        #region UpdateMovementStatus
        private void ManageMovementStatus()
        {
            switch (_mCustomerMovementStatus)
            {
                case BaseCustomerMovementStatus.Idle:
                    break;
                case BaseCustomerMovementStatus.Walking:
                    EvaluateWalking();
                    break;
                case BaseCustomerMovementStatus.EvaluatingProduct:
                    if (_productInHand)
                    {
                        break;
                    }
                    RotateTowardsYOnly(transform,_tempTargetOfInterest);
                    break;
                case BaseCustomerMovementStatus.Running:
                    break;
            }
        }
        private void EvaluateWalking()
        {
            if (NavMeshAgent.destination.Equals(default(Vector3)))
            {
                Debug.LogWarning("Destination to walk to must be already set");
                return;
            }

            if (NavMeshAgent.remainingDistance < 1f && !NavMeshAgent.isStopped)
            {
                NavMeshAgent.isStopped = true;
                OnWalkingDestinationReached();
            }
        }

        protected override void RotateTowardsYOnly(Transform rotatingObject, Transform facingTowards)
        {
            base.RotateTowardsYOnly(rotatingObject, facingTowards);
        }
        #endregion
        
        #region Paying
        private void GoToPay()
        {
            if ((_mCustomerAttitudeStatus & BaseAttitudeStatus.Paying) != 0)
            {
                return;   
            }
            NavMeshAgent.destination = _mPayingPosition;
        }
        private async void PayAndLeave(int timePaying)
        {
            SetCustomerMovementStatus(BaseCustomerMovementStatus.Idle);
            SetCustomerAttitudeStatus(BaseAttitudeStatus.Paying);
            NavMeshAgent.isStopped = true;

            //Time to do something
            await Task.Delay(timePaying);
            SetCustomerMovementStatus(BaseCustomerMovementStatus.Walking);
            SetCustomerAttitudeStatus(BaseAttitudeStatus.Leaving);

            NavMeshAgent.SetDestination(MInitialPosition);
            NavMeshAgent.isStopped = false;
        }
        #endregion

        #region Utils
        private void SetCustomerAttitudeStatus(BaseAttitudeStatus newAttitude)
        {
            _mCustomerAttitudeStatus = 0;
            _mCustomerAttitudeStatus |= newAttitude;

            switch (newAttitude)
            {
                case BaseAttitudeStatus.EvaluatingProduct:
                    var pointOfInterest = _positionsManager.GetPoiData(_currentPoiId);
                    _tempStoreProductOfInterest = pointOfInterest.ChooseRandomProduct();
                    _tempTargetOfInterest = _tempStoreProductOfInterest.Item1;
                    break;
                
                case BaseAttitudeStatus.Paying:
                    _tempStoreProductOfInterest = null;
                    NavMeshAgent.enabled = true;
                    NavMeshAgent.SetDestination(_mPayingPosition);
                    NavMeshAgent.isStopped = false;
                    break;
                
                case BaseAttitudeStatus.Leaving:
                    NavMeshAgent.enabled = true;
                    NavMeshAgent.SetDestination(_positionsManager.EntrancePosition());
                    NavMeshAgent.isStopped = false;
                    break;
                
                case BaseAttitudeStatus.Entering:
                    NavMeshAgent.enabled = true;
                    NavMeshAgent.SetDestination(_positionsManager.EntrancePosition());
                    NavMeshAgent.isStopped = false;
                    break;
            }
        }
        private void SetCustomerMovementStatus(BaseCustomerMovementStatus newMovementStatus)
        {
            _mCustomerMovementStatus = 0;
            _mCustomerMovementStatus |= newMovementStatus;
            
            //set
            switch (newMovementStatus)
            {
                case  BaseCustomerMovementStatus.Walking:
                    ObstacleComponent.enabled = false;
                    NavMeshAgent.enabled = true;
                    NavMeshAgent.isStopped = false;
                    BaseAnimator.ChangeAnimationState(WALK);
                    break;
                case  BaseCustomerMovementStatus.Idle:
                    NavMeshAgent.isStopped = true;
                    NavMeshAgent.enabled = false;
                    ObstacleComponent.enabled = true;
                    BaseAnimator.ChangeAnimationState(IDLE);
                    break;
                case BaseCustomerMovementStatus.EvaluatingProduct:
                    NavMeshAgent.isStopped = true;
                    NavMeshAgent.enabled = false;

                    ObstacleComponent.enabled = true;
                    BaseAnimator.ChangeAnimationState(IDLE);

                    break;
            }
        }
        protected virtual void OnWalkingDestinationReached()
        {
            WalkingDestinationReached?.Invoke();
        }
        #endregion
    }
}