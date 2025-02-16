using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.MovementStates;

namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.AttitudeStates
{
    public class ScaredRunningState : IAttitudeState
    {
        private BaseCharacterInScene character;

        public ScaredRunningState(BaseCharacterInScene character)
        {
            this.character = character;
        }

        public void Enter()
        {
        }

        public void Exit() { }

        public void Update() { }
        public void WalkingDestinationReached()
        {
            throw new System.NotImplementedException();
        }
    }
}