using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.MovementStates;

namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.AttitudeStates
{
    public class WalkingTowardsPositionState : IAttitudeState
    {
        private IBaseCharacterInScene _mCharacter;
        public WalkingTowardsPositionState(IBaseCharacterInScene characterInScene)
        {
            _mCharacter = characterInScene;
        }
        
        public void Enter()
        {
        }

        public void Exit()
        {
        }

        public void Update()
        {
            if(_mCharacter.GetNavMeshAgent.remainingDistance < 4f)
            {
                _mCharacter.OnWalkingDestinationReached();
                _mCharacter.ChangeMovementState<IdleMovementState>();
            }
        }

        public void WalkingDestinationReached()
        {
        }
    }
}