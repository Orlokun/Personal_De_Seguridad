using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.MovementStates;

namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.AttitudeStates
{
    public class IdleAttitudeState : IAttitudeState
    {
        private const string Idle = "Idle";
        private BaseCharacterInScene character;

        public IdleAttitudeState(BaseCharacterInScene character)
        {
            this.character = character;
        }

        public void Enter()
        {
            character.BaseAnimator.ChangeAnimationState(Idle);
        }
        public void Exit() { }

        public void Update() { }
        
        public void WalkingDestinationReached()
        {
            throw new System.NotImplementedException();
        }
    }
}