using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        
        private IStoreProduct _tempStoreProductOfInterest;
        private Transform _tempTargetOfInterest;
        private GameObject _tempProductCopy;
        
        #endregion
        
        private ICustomerTypeData _mCustomerTypeData;
        public ICustomerTypeData CustomerTypeData => _mCustomerTypeData;
        
        #region LevelData
        private Vector3 _mPayingPosition;
        private int _mNumberOfProductsLookingFor;
        private List<Guid> _mShelvesOfInterest;

        private bool _mAnyShelfAvailable;
        #endregion

        #region CurrentCustomerStatus
        private BaseCustomerMovementStatus _mCustomerMovementStatus = 0;
        private BaseAttitudeStatus _mCustomerAttitudeStatus = 0;

        private Dictionary<Guid, bool> _mShelvesOfInterestPurchaseStatus = new Dictionary<Guid, bool>();
        private Dictionary<Guid, IStoreProductObjectData> _mStolenProducts = new Dictionary<Guid, IStoreProductObjectData>();
        private Dictionary<Guid, IStoreProductObjectData> _mPurchasedProducts = new Dictionary<Guid, IStoreProductObjectData>();

        private Guid _currentShelfId;
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
            _mShelvesOfInterest = _positionsManager.GetShelvesOfInterestIds(_mNumberOfProductsLookingFor);
            foreach (var guid in _mShelvesOfInterest)
            {
                _mShelvesOfInterestPurchaseStatus.Add(guid, false);
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

        private void PrepareProductEvaluation()
        {
            if ((_mCustomerAttitudeStatus & BaseAttitudeStatus.EvaluatingProduct) != 0)
            {
                return;
            }
        }
        private void ReleaseCurrentPoI()
        {
            var poi = _positionsManager.GetShelfObject(_currentShelfId).GetCustomerPoI;
            if (poi.OccupierId != MCustomerId)
            {
                return;
            }
            poi.LeavePoi(MCustomerId);
        }
        
        private void GoToNextProduct()
        {
            if(_mShelvesOfInterestPurchaseStatus.All(x => x.Value != false))
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
            _currentShelfId = GetNotVisitedShelf(); 
            var shelfObject = _positionsManager.GetShelfObject(_currentShelfId).GetCustomerPoI;
            _positionsManager.OccupyPoi(MCustomerId, _currentShelfId);
            Debug.Log($"[GoToNextPoint] Going to Shelf: {_currentShelfId}. Name: {shelfObject.gameObject.name}");
            NavMeshAgent.SetDestination(shelfObject.GetPosition);
            NavMeshAgent.isStopped = false;
        }

        private Guid GetNotVisitedShelf()
        {
            foreach (var shelf in _mShelvesOfInterestPurchaseStatus)
            {
                if (shelf.Value == false)
                {
                    return shelf.Key;
                }
            }
            Debug.LogError("[GetShelfObjectId] Guid not found");
            return new Guid();
        }

        private bool CheckIfPoisAvailable()
        {
            var unPurchasedPois = _mShelvesOfInterestPurchaseStatus.Where(x => x.Value == false);
            var unPurchasedPoisList = unPurchasedPois.Select(x => x.Key);
            var anyPoiAvailable = _positionsManager.GetShelvesOfInterestData(unPurchasedPoisList.ToList())
                .Any(x => x.GetCustomerPoI.IsOccupied != true);
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
            var shelves = _positionsManager.GetShelvesOfInterestData(_mShelvesOfInterest);
            _mAnyShelfAvailable =  shelves.Any(x=> x.GetCustomerPoI.IsOccupied != true);
            if (!_mAnyShelfAvailable)
            {
                Debug.Log("No shelves available. Will Walk around and try to interact.");
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
            var hasStealAbility = _tempStoreProductOfInterest.GetData.HideChances <= _mCustomerTypeData.StealAbility ? 1 : 0;
            var isTempting = _tempStoreProductOfInterest.GetData.Tempting >= _mCustomerTypeData.Corruptibility ? 1 : 0;
            var isDetermined = _tempStoreProductOfInterest.GetData.Punishment <= _mCustomerTypeData.Fearful ? 1 : 0;

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
            var path = ProductPrefabPath + _tempStoreProductOfInterest.GetData.PrefabName;
            _tempProductCopy = (GameObject)Instantiate(Resources.Load(path),rightHand, false);
            _productInHand = true;
            _tempProductCopy.transform.localScale *= .2f;
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
            _mPurchasedProducts.Add(Guid.NewGuid(), _tempStoreProductOfInterest.GetData);
            ClearProductInterest();
            _mShelvesOfInterestPurchaseStatus[_currentShelfId] = true;
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
            _mStolenProducts.Add(Guid.NewGuid(), _tempStoreProductOfInterest.GetData);
            Debug.Log($"{gameObject.name} stole a {_tempStoreProductOfInterest.GetData.ProductName}!");
            ClearProductInterest();
            SetCustomerMovementStatus(BaseCustomerMovementStatus.Walking);
            SetCustomerAttitudeStatus(BaseAttitudeStatus.Shopping);
            _mShelvesOfInterestPurchaseStatus[_currentShelfId] = true;
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
                    var shelfOfInterest = _positionsManager.GetShelfObject(_currentShelfId);
                    _tempStoreProductOfInterest = shelfOfInterest.ChooseRandomProduct();
                    _tempTargetOfInterest = _tempStoreProductOfInterest.ProductTransform;
                    break;
                
                case BaseAttitudeStatus.Paying:
                    _tempStoreProductOfInterest = null;
                    NavMeshAgent.SetDestination(_mPayingPosition);
                    NavMeshAgent.isStopped = false;
                    break;
                
                case BaseAttitudeStatus.Leaving:
                    NavMeshAgent.SetDestination(_positionsManager.EntrancePosition());
                    NavMeshAgent.isStopped = false;
                    break;
                
                case BaseAttitudeStatus.Entering:
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
                    ObstacleComponent.enabled = true;
                    NavMeshAgent.enabled = false;
                    BaseAnimator.ChangeAnimationState(IDLE);
                    NavMeshAgent.isStopped = true;
                    break;
                case BaseCustomerMovementStatus.EvaluatingProduct:
                    ObstacleComponent.enabled = true;
                    NavMeshAgent.enabled = false;
                    BaseAnimator.ChangeAnimationState(IDLE);
                    NavMeshAgent.isStopped = true;
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