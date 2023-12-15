using System;
using System.Threading.Tasks;
using GamePlayManagement.LevelManagement.LevelObjectsManagement;
using UnityEditor;
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
        
        [SerializeField] protected MultiAimConstraint MHeadAimConstraint;
        
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

            BaseAnimator.ChangeAnimationState(GetObject);
            await Task.Delay(1000);
            var path = ProductPrefabPath + _tempStoreProductOfInterest.GetData.PrefabName;
            _tempProductCopy = (GameObject)Instantiate(Resources.Load(path), Vector3.zero, new Quaternion(),rightHand);
            _tempProductCopy.transform.position *= .2f;
            BaseAnimator.ChangeAnimationState(EvaluateProductObject);
            if (!wouldStealProduct)
            {
                Debug.Log("[StartProductExamination] WOULD NOT STEAL PRODUCT");
                await Task.Delay(Random.Range(4500, 10000));
                Destroy(_tempProductCopy);
                _tempProductCopy = null;
                
                SetCustomerMovementStatus(BaseCustomerMovementStatus.Walking);
                SetCustomerAttitudeStatus(BaseAttitudeStatus.Shopping);
                ReleaseCurrentPoI();
                GoToNextProduct();
            }
            else
            {
                Debug.Log("[StartProductExamination] WOULD STEAL PRODUCT");
            }
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
                    LookAtObject(_tempTargetOfInterest);
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
            if (MHeadAimConstraint.data.constrainedObject == newTarget)
            {
                return;
            }
            MHeadAimConstraint.data.constrainedObject = newTarget;
        }

        private void ClearLookAt()
        {
            if (MHeadAimConstraint.data.constrainedObject == null)
            {
                return;
            }
            MHeadAimConstraint.data.constrainedObject = null;
        }
        private void EvaluateWalking()
        {
            if (NavMeshAgent.destination.Equals(default(Vector3)))
            {
                Debug.LogWarning("Destination to walk to must be already set");
                return;
            }
            if (NavMeshAgent.remainingDistance < 2 && (_mCustomerAttitudeStatus & BaseAttitudeStatus.Shopping) != 0)
            {
                SetProductAsTarget();
            }
            if (NavMeshAgent.remainingDistance < .5f && !NavMeshAgent.isStopped)
            {
                NavMeshAgent.isStopped = true;
                OnWalkingDestinationReached();
            }
        }

        private void SetProductAsTarget()
        {
            
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