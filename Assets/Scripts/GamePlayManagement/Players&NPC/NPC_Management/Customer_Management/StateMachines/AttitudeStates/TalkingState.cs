using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.MovementStates;

namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.AttitudeStates
{
    public class TalkingState : IAttitudeState
    {
        private BaseCharacterInScene character;

        public TalkingState(BaseCharacterInScene character)
        {
            this.character = character;
        }

        public void Enter()
        {
            character.ChangeCharacterAttitudeState(BaseCharacterAttitudeStatus.Talking);
        }

        public void Exit() { }

        public void Update() { }
        public void WalkingDestinationReached()
        {
            throw new System.NotImplementedException();
        }
    }
}