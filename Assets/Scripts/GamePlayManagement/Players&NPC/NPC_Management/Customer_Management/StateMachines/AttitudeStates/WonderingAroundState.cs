using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.MovementStates;

namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.AttitudeStates
{
    public class WonderingAroundState : IAttitudeState
    {
        private BaseCharacterInScene character;

        public WonderingAroundState(BaseCharacterInScene character)
        {
            this.character = character;
        }

        public void Enter()
        {
            character.ChangeCharacterAttitudeState(BaseCharacterAttitudeStatus.Idle);
        }

        public void Exit() { }

        public void Update() { }
        public void WalkingDestinationReached()
        {
            throw new System.NotImplementedException();
        }
    }
}