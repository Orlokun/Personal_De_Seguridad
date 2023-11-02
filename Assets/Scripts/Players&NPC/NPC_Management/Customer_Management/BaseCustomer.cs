using System;
using System.Collections;
using System.Threading.Tasks;
using GamePlayManagement.LevelManagement.LevelObjectsManagement;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Players_NPC.NPC_Management.Customer_Management
{
    public interface IShopProductCatalogue
    {
        
    }

    public class ShopCatalogueManager : MonoBehaviour
    {
        
    }

    public class ShopCatalogueOrganizer : MonoBehaviour
    {
        
    }

    public class ShopShelfUnit
    {
        
    }

    public class BaseCustomer : BaseCharacterInScene, IBaseCustomer
    {
        [SerializeField] private int mNumberOfProductsLookingFor;
        [SerializeField] private Transform mPayingPosition;
    
        private IShelfInMarket[] _mShelvesOfInterest;
        private IProductInShelf tempProductOfInterest;
        
        private ICustomerTypeData _mCustomerTypeData;
        
        private Vector3 _mPayingPosition;
    
        private bool _mFinishedPath = false;

        
        private BaseCustomerMovementStatus _mCustomerMovementStatus = 0;
        private BaseAttitudeStatus _mCustomerAttitudeStatus = 0;

        private delegate void ReachDestination();
        private event ReachDestination WalkingDestinationReached;
        protected override void Awake()
        {
            base.Awake();
            _mPayingPosition = mPayingPosition.position;
            _mCustomerTypeData = new BaseCustomerTypeData();
            WalkingDestinationReached += ReachWalkingDestination;
            _mShelvesOfInterest = new IShelfInMarket[mNumberOfProductsLookingFor];
        }
        
        protected override void Start()
        {
            base.Start();
            GoToEntrance();
            StartWalking();
            _mShelvesOfInterest = _positionsManager.GetUnoccupiedShelf(mNumberOfProductsLookingFor);
            OccupyPois();
            Debug.Log($"[Awake] Initial Position: {MInitialPosition}. ");
        }

        private void OccupyPois()
        {
            foreach (var shelf in _mShelvesOfInterest)
            {
                _positionsManager.OccupyPoi(MCustomerId, shelf.GetCustomerPoI);
            }
        }
        private void StartWalking()
        {
            SetCustomerMovementStatus(BaseCustomerMovementStatus.Walking);
            BaseAnimator.ChangeAnimationState(WALK);
        }
        
        private void Update()
        {
            ManageAttitudeStatus();
            ManageMovementStatus();
        }

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
                    StartProductEvaluation();
                    break;
                case BaseAttitudeStatus.Fighting:
                    break;
                case BaseAttitudeStatus.Leaving:
                    break;
            }
        }

        private void StartProductEvaluation()
        {
            if ((_mCustomerAttitudeStatus & BaseAttitudeStatus.EvaluatingProduct) != 0)
            {
                return;
            }
            var shelfOfInterest = _mShelvesOfInterest[CurrentProductSearchIndex];
            tempProductOfInterest = shelfOfInterest.GetRandomProductPosition();
        }
        private void ManageMovementStatus()
        {
            switch (_mCustomerMovementStatus)
            {
                case BaseCustomerMovementStatus.Idle:
                    break;
                case BaseCustomerMovementStatus.Walking:
                    Walking();
                    break;
                case BaseCustomerMovementStatus.EvaluatingProduct:
                    RotateTowardsYOnly(transform,tempProductOfInterest.ProductTransform);
                    break;
                case BaseCustomerMovementStatus.Stealing:
                    break;
                case BaseCustomerMovementStatus.Running:
                    break;
                case BaseCustomerMovementStatus.Detained:
                    break;
            }
        }
        private void Walking()
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
            Debug.Log("Rotating!");
        }

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
                    Destroy(this.gameObject);
                    break;
                case BaseAttitudeStatus.Fighting:
                    break;
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

        private void CheckShoppingStatus()
        {
            if (CurrentProductSearchIndex <= mNumberOfProductsLookingFor)
            {
                EvaluateProduct();
            }
        }

        private void EvaluateProduct()
        {
            SetCustomerMovementStatus(BaseCustomerMovementStatus.EvaluatingProduct);
            SetCustomerAttitudeStatus(BaseAttitudeStatus.EvaluatingProduct);
            var wouldStealProduct = EvaluateProductStealingChances();
            WaitProductExamination();
        }

        private async void WaitProductExamination()
        {
            Random.InitState(DateTime.Now.Millisecond);
            await Task.Delay(Random.Range(7000, 15000));
            SetCustomerMovementStatus(BaseCustomerMovementStatus.Walking);
            SetCustomerAttitudeStatus(BaseAttitudeStatus.Shopping);
            ReleaseCurrentPoI();
            GoToNextProduct();
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
        private bool EvaluateProductStealingChances()
        {
            return false;
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
        private void GoToNextProduct()
        {
            //return in idle
            if(CurrentProductSearchIndex == mNumberOfProductsLookingFor)
            {
                Debug.Log("[GoToNextPoint] Going to Pay");
                SetCustomerAttitudeStatus(BaseAttitudeStatus.Paying);
                return;
            }
            Debug.Log($"[GoToNextPoint] Going to Shelf Indexed: {CurrentProductSearchIndex}");
            var destinationCorrectlySet = NavMeshAgent.SetDestination(_mShelvesOfInterest[CurrentProductSearchIndex].GetCustomerPoI.GetPosition);
            NavMeshAgent.isStopped = !destinationCorrectlySet;
            CurrentProductSearchIndex++;
        }

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
                    tempProductOfInterest = shelfOfInterest.GetRandomProductPosition();
                    break;
                case BaseAttitudeStatus.Paying:
                    tempProductOfInterest = null;
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
        #endregion

        protected virtual void OnWalkingDestinationReached()
        {
            WalkingDestinationReached?.Invoke();
        }
    }

    public interface IBaseCustomer
    {
        
    }

    [Flags]
    public enum BaseCustomerMovementStatus
    {
        Idle = 1,
        Walking = 2, 
        EvaluatingProduct = 4,
        Running = 8,
        Dancing = 16,
        Stealing = 32,
        Sitting = 64,
        Falling = 128,
        Detained = 256,
    }

    [Flags]
    public enum BaseAttitudeStatus
    {
        Neutral = 1,
        Fighting = 2,
        Stealing = 4,
        Shopping = 8,
        Paying = 16,
        Leaving = 32,
        Entering = 64,
        EvaluatingProduct = 128
    }
}