using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.MovementStates;

namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.AttitudeStates
{
    public class AccessingBuildingState : IAttitudeState, IAccessingBuildingState
    {
        private BaseCharacterInScene _mCharacter;

        public AccessingBuildingState(BaseCharacterInScene mcharacter)
        {
            _mCharacter = mcharacter;
            _mCharacter.WalkingDestinationReached += WalkingDestinationReached;
        }

        public void Enter()
        {
            _mCharacter.SetMovementDestination(_mCharacter.EntranceData.EntrancePosition);
            _mCharacter.ChangeMovementState<WalkingState>();
        }

        public void Exit() { }

        public void Update() { }
        
        public void WalkingDestinationReached()
        {
            EvaluateStartShopping();
            _mCharacter.WalkingDestinationReached -= WalkingDestinationReached;
        }
        
        private void EvaluateStartShopping()
        {
            var character = (BaseCustomer)_mCharacter;
            character.EvaluateStartShopping();
        }
    }

    public interface IAccessingBuildingState
    {
        
    }
}