using System;
using System.Collections;
using System.Collections.Generic;
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
        private const string ProductPrefabPath = "LevelManagementPrefabs/ProductPrefabs/";
        protected const string GetObject = "GetProduct";
        protected const string EvaluateProductObject = "InspectProduct";
        protected const string SearchAround = "SearchAround";
        protected bool productInHand;
        
        [SerializeField] protected MultiAimConstraint mHeadAimConstraint;
        
        [SerializeField] protected TwoBoneIKConstraint MGrabObjectConstraint;
        [SerializeField] protected TwoBoneIKConstraint MInspectObjectConstraint;
        
        [SerializeField] protected Transform rightHand;
        private Vector3 mPayingPosition;
        private int _mNumberOfProductsLookingFor;

        private IShelfInMarket[] _mShelvesOfInterest;
        private IStoreProduct _tempStoreProductOfInterest;
        private Transform _tempTargetOfInterest;
        private GameObject _tempProductCopy;
        
        private ICustomerTypeData _mCustomerTypeData;
        private Vector3 _mPayingPosition;

        private BaseCustomerMovementStatus _mCustomerMovementStatus = 0;
        private BaseAttitudeStatus _mCustomerAttitudeStatus = 0;

        private Dictionary<Guid, IStoreProductObjectData> _mStolenProducts = new Dictionary<Guid, IStoreProductObjectData>();
        private Dictionary<Guid, IStoreProductObjectData> _mPurchasedProducts = new Dictionary<Guid, IStoreProductObjectData>();

        private delegate void ReachDestination();
        private event ReachDestination WalkingDestinationReached;

        #region Init
        protected override void Awake()
        {
            Random.InitState(DateTime.Now.Millisecond);
            _mNumberOfProductsLookingFor = Random.Range(1, 8);
            base.Awake();
            _mCustomerTypeData = Factory.CreateBaseCustomerTypeData();
            WalkingDestinationReached += ReachWalkingDestination;
            _mShelvesOfInterest = new IShelfInMarket[_mNumberOfProductsLookingFor];
        }
        
        protected override void Start()
        {
            base.Start();
            _mPayingPosition = _positionsManager.PayingPosition();
            GoToEntrance();
            StartWalking();
            _mShelvesOfInterest = _positionsManager.GetUnoccupiedShelf(_mNumberOfProductsLookingFor);
            Debug.Log($"[Awake] Initial Position: {MInitialPosition}. ");
        }
        private void OccupyPoi(int shelfOfInterest)
        {
            _mShelvesOfInterest[shelfOfInterest].GetCustomerPoI.OccupyPoi(MCustomerId);
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

        #region UpdateProductEvaluation





        #endregion
        
        #region UpdateMangeAttitude
        private void ManageAttitudeStatus()
        {
            switch (_mCustomerAttitudeStatus)
            {
                case BaseAttitudeStatus.Entering:
                    GoToEntrance();
                    break;
                case BaseAttitudeStatus.Paying:
                    GoToPay();
                    break;
                case BaseAttitudeStatus.Shopping:
                    StartShopping();
                    break;
                case BaseAttitudeStatus.EvaluatingProduct:
                    break;
                case BaseAttitudeStatus.Fighting:
                    break;
                case BaseAttitudeStatus.Leaving:
                    break;
            }
        }
        private void GoToEntrance()
        {
            if ((_mCustomerAttitudeStatus & BaseAttitudeStatus.Entering) != 0)
            {
                return;
            }
            NavMeshAgent.destination = _positionsManager.EntrancePosition();
            SetCustomerAttitudeStatus(BaseAttitudeStatus.Entering);
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
            var poi = _mShelvesOfInterest[CurrentProductSearchIndex-1].GetCustomerPoI;
            if (poi.OccupierId != MCustomerId)
            {
                return;
            }
            poi.LeavePoi(MCustomerId);
        }
        private void GoToNextProduct()
        {
            //return in idle
            if(CurrentProductSearchIndex == _mNumberOfProductsLookingFor)
            {
                Debug.Log("[GoToNextPoint] Going to Pay");
                SetCustomerAttitudeStatus(BaseAttitudeStatus.Paying);
                return;
            }
            Debug.Log($"[GoToNextPoint] Going to Shelf Indexed: {CurrentProductSearchIndex}");
            var destinationCorrectlySet = NavMeshAgent.SetDestination(_mShelvesOfInterest[CurrentProductSearchIndex].GetCustomerPoI.GetPosition);
            OccupyPoi(CurrentProductSearchIndex);
            NavMeshAgent.isStopped = !destinationCorrectlySet;
            CurrentProductSearchIndex++;
        }

        #endregion

        #region ReachDestinationEvent
        private void ReachWalkingDestination()
        {
            switch (_mCustomerAttitudeStatus)
            {
                case BaseAttitudeStatus.Shopping:
                    CheckShoppingStatus();
                    break;
                case BaseAttitudeStatus.Paying:
                    Random.InitState(DateTime.Now.Millisecond);
                    PayAndLeave(Random.Range(5000,11000));
                    break;
                case BaseAttitudeStatus.Entering:
                    StartShopping();
                    break;
                case BaseAttitudeStatus.Leaving:
                    Destroy(gameObject);
                    break;
                case BaseAttitudeStatus.Fighting:
                    break;
            }
        }
        private void CheckShoppingStatus()
        {
            if (CurrentProductSearchIndex <= _mNumberOfProductsLookingFor)
            {
                EvaluateProduct();
            }
        }
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
            ReleaseCurrentPoI();
            GoToNextProduct();
        }


        private void InstantiateProductInHand()
        {
            var path = ProductPrefabPath + _tempStoreProductOfInterest.GetData.PrefabName;
            _tempProductCopy = (GameObject)Instantiate(Resources.Load(path),rightHand, false);
            productInHand = true;
            _tempProductCopy.transform.localScale *= .2f;
            _tempProductCopy.transform.localPosition = new Vector3(0,0,0);
            _tempTargetOfInterest = _tempProductCopy.transform;
        }
        private void StartShopping()
        {
            if ((_mCustomerAttitudeStatus & BaseAttitudeStatus.Shopping) != 0)
            {
                return;
            }
            SetCustomerAttitudeStatus(BaseAttitudeStatus.Shopping);
            SetCustomerMovementStatus(BaseCustomerMovementStatus.Walking);
            GoToNextProduct();
        }

        private void ClearProductInterest()
        {
            Destroy(_tempProductCopy);
            _tempTargetOfInterest = null;
            productInHand = false;
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
                    if (productInHand)
                    {
                        break;
                    }
                    RotateTowardsYOnly(transform,_tempTargetOfInterest);
                    break;
                case BaseCustomerMovementStatus.Stealing:
                    break;
                case BaseCustomerMovementStatus.Running:
                    break;
                case BaseCustomerMovementStatus.Detained:
                    break;
            }
        }
        private void LookAtObject(Transform newTarget)
        {
            var targetArray = new WeightedTransformArray(1);
            var target = new WeightedTransform(newTarget, 1f);
            targetArray[0] = target;
            var headObject = mHeadAimConstraint.data.constrainedObject;
            mHeadAimConstraint.data = new MultiAimConstraintData
            {
                constrainedObject = headObject.transform,
                constrainedXAxis = true,
                constrainedYAxis = true,
                constrainedZAxis = true,
                sourceObjects = targetArray,
                limits = new Vector2(-90f, 42f)   
            };
            BaseAnimator.ChangeAnimationState("StartLook");
        }
        private void StopLooking()
        {
            mHeadAimConstraint.data.sourceObjects.Clear();
            mHeadAimConstraint.weight = 1;
        }

        private void ClearLookAt()
        {
            if (mHeadAimConstraint.data.constrainedObject == null)
            {
                return;
            }
            mHeadAimConstraint.data.constrainedObject = null;
        }
        private void EvaluateWalking()
        {
            if (NavMeshAgent.destination.Equals(default(Vector3)))
            {
                Debug.LogWarning("Destination to walk to must be already set");
                return;
            }

            if (NavMeshAgent.remainingDistance < .5f && !NavMeshAgent.isStopped)
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
                    var shelfOfInterest = _mShelvesOfInterest[CurrentProductSearchIndex-1];
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
                    NavMeshAgent.isStopped = false;
                    BaseAnimator.ChangeAnimationState(WALK);
                    break;
                case  BaseCustomerMovementStatus.Idle:
                    BaseAnimator.ChangeAnimationState(IDLE);
                    NavMeshAgent.isStopped = true;
                    break;
                case BaseCustomerMovementStatus.EvaluatingProduct:
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