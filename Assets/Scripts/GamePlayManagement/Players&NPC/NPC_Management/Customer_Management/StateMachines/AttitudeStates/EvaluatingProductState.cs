using System;
using System.Collections;
using System.Threading.Tasks;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.MovementStates;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.AttitudeStates
{
    public class EvaluatingProductState : IAttitudeState
    {
        //Eventually change into restricted interface so it can be used by any character, not only customers.
        private BaseCustomer _mCharacter;

        public EvaluatingProductState(IBaseCharacterInScene mCharacter)
        {
            _mCharacter = (BaseCustomer)mCharacter;
        }
        public void Enter()
        {
            var wouldStealProduct = EvaluateProductStealingChances();
            StartProductExamination(wouldStealProduct);
        }
        public void Exit() { }

        public void Update()
        {
            if (_mCharacter.HasProductInHand || _mCharacter.TempObjectTargetOfInterest == null)
            {
                return;
            }
            _mCharacter.RotateTowardsYOnly(_mCharacter.TempObjectTargetOfInterest.position);
        }
        public void WalkingDestinationReached()
        {
            throw new NotImplementedException();
        }

        private bool EvaluateProductStealingChances()
        {
            var hasStealAbility = _mCharacter.TempStoreProductOfInterest.Item2.HideChances <= _mCharacter.CustomerTypeData.StealAbility ? 1 : 0;
            var isTempting = _mCharacter.TempStoreProductOfInterest.Item2.Tempting >= _mCharacter.CustomerTypeData.Corruptibility ? 1 : 0;
            var isDetermined = _mCharacter.TempStoreProductOfInterest.Item2.Punishment <= _mCharacter.CustomerTypeData.Fearful ? 1 : 0;

            return hasStealAbility + isTempting + isDetermined >= 2;
        }

        private async void StartProductExamination(bool wouldStealProduct)
        {
            Random.InitState(DateTime.Now.Millisecond);
            await Task.Delay(Random.Range(1500, 2000));
            _mCharacter.GrabObjectConstraint.data.target = _mCharacter.TempObjectTargetOfInterest.transform;
            _mCharacter.StartCoroutine(SetGrabObjectConstraint(0, 1, 1));
            await Task.Delay(1000);
            _mCharacter.InstantiateProductInHand();
            StartInspectObjectAnim();
            await Task.Delay(1000);
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
            _mCharacter.StartCoroutine(SetGrabObjectConstraint(1, 0, 1));
            _mCharacter.StartCoroutine(SetLookObjectWeight(0, 1, 1));
            _mCharacter.StartCoroutine(UpdateInspectObjectRigWeight(0, 1, 1));
        }
        private IEnumerator SetLookObjectWeight(float start, float end, float time)
        {
            float elapsedTime = 0;
            while (elapsedTime < time)
            {
                _mCharacter.HeadAimConstraint.weight = Mathf.Lerp(start, end, (elapsedTime / time));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        private IEnumerator UpdateInspectObjectRigWeight(float start, float end, float time)
        {
            float elapsedTime = 0;
            while (elapsedTime < time)
            {
                _mCharacter.InspectObjectConstraint.weight = Mathf.Lerp(start, end, (elapsedTime / time));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        private IEnumerator SetGrabObjectConstraint(float start, float end, float time)
        {
            float elapsedTime = 0;
            while (elapsedTime < time)
            {
                _mCharacter.GrabObjectConstraint.weight = Mathf.Lerp(start, end, (elapsedTime / time));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        private async void AddProductAndKeepShopping()
        {
            Debug.Log("[AddProductAndKeepShopping] WOULD NOT STEAL PRODUCT");
            await Task.Delay(Random.Range(4500, 10000));
            _mCharacter.StartCoroutine(UpdateInspectObjectRigWeight(1, 0, 1));
            _mCharacter.GetCustomerStoreVisitData.PurchaseProduct(Guid.NewGuid(), _mCharacter.TempStoreProductOfInterest.Item2);
            ClearProductInterest();
            _mCharacter.PoisPurchaseStatus[_mCharacter.MCurrentPoiId] = true;
            _mCharacter.ChangeMovementState<WalkingState>();
            _mCharacter.ChangeAttitudeState<ShoppingState>();
            _mCharacter.ReleaseCurrentPoI();
            _mCharacter.GoToNextProduct();
        }

        /// <summary>
        /// This logic should be separated
        /// </summary>
        private const string SearchAround = "SearchAround";
        private async void StartStealingProductAttempt()
        {
            Debug.Log($"[StartProductExamination] {_mCharacter.gameObject.name} WOULD STEAL PRODUCT. Start Process");
            await Task.Delay(Random.Range(1000, 1500));

            _mCharacter.StartCoroutine(SetLookObjectWeight(1, 0, 1.5f));
            _mCharacter.BaseAnimator.ChangeAnimationState(SearchAround);
            await Task.Delay(8000);
            _mCharacter.StartCoroutine(UpdateInspectObjectRigWeight(1, 0, 1));
            _mCharacter.GetCustomerStoreVisitData.StealProduct(Guid.NewGuid(), _mCharacter.TempStoreProductOfInterest.Item2);
            ClearProductInterest();
            _mCharacter.PoisPurchaseStatus[_mCharacter.MCurrentPoiId] = true;
            _mCharacter.ChangeMovementState<WalkingState>();
            _mCharacter.ChangeAttitudeState<ShoppingState>();
            _mCharacter.ReleaseCurrentPoI();
            _mCharacter.GoToNextProduct();
        }
        private void ClearProductInterest()
        {
            _mCharacter.ClearProductOfInterest();
        }
    }
}