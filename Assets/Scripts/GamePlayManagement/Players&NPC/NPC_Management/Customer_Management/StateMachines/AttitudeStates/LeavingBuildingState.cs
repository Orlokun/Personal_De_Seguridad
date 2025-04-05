using System.Numerics;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.MovementStates;

namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.AttitudeStates
{
    public class LeavingBuildingState : IAttitudeState
    {
        private BaseCharacterInScene _mCharacter;

        public LeavingBuildingState(BaseCharacterInScene mcharacter)
        {
            _mCharacter = mcharacter;
        }

        public void Enter()
        {
            _mCharacter.SetMovementDestination(_mCharacter.InitialPosition);
            _mCharacter.ChangeMovementState<WalkingState>();
            _mCharacter.WalkingDestinationReached += WalkingDestinationReached;
        }

        public void Exit() { }

        public void Update()
        {
            if(_mCharacter.GetNavMeshAgent.remainingDistance < 0.5f)
            {
                _mCharacter.ChangeMovementState<RunningState>();
            }
        }
        
        public void WalkingDestinationReached()
        {
            _mCharacter.WalkingDestinationReached -= WalkingDestinationReached;
            var customer = _mCharacter as BaseCustomer;
            customer.ClearCustomer();
        }

        public Vector3 PositionTarget { get; set; }

    }
}