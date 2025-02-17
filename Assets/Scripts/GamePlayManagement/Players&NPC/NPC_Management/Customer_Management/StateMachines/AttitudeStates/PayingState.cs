using System;
using System.Threading.Tasks;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.MovementStates;
using Random = UnityEngine.Random;

namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.AttitudeStates
{
    public class PayingState : IAttitudeState
    {
        private BaseCustomer _mCharacter;

        public PayingState(BaseCharacterInScene mCharacter)
        {
            _mCharacter = (BaseCustomer)mCharacter;
        }

        public void Enter()
        {
            _mCharacter.WalkingDestinationReached += WalkingDestinationReached;
            _mCharacter.ClearProductOfInterest();
            _mCharacter.SetMovementDestination(_mCharacter.PayingPosition);
        }

        public void Exit() { }

        public void Update() { }
        
        public void WalkingDestinationReached()
        {
            Random.InitState(DateTime.Now.Millisecond);
            PayAndLeave(Random.Range(5000,11000));
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
            _mCharacter.ChangeMovementState<IdleMovementState>();
            _mCharacter.ToggleNavMesh(false);
        }

        private void Leave()
        {
            _mCharacter.ChangeAttitudeState<LeavingBuildingState>();
            _mCharacter.ChangeMovementState<WalkingState>();
            _mCharacter.WalkingDestinationReached -= WalkingDestinationReached;
        }
    }
}