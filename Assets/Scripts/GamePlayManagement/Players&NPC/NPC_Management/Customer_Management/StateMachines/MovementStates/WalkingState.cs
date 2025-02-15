namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.MovementStates
{
    public class WalkingState : IMovementState
    {
        private BaseCharacterInScene character;
        public const string Walk = "Walk";

        public WalkingState(BaseCharacterInScene character)
        {
            this.character = character;
        }

        public void Enter()
        {
            character.BaseAnimator.ChangeAnimationState(Walk);
        }

        public void Exit() { }

        public void Update()
        {
            character.EvaluateWalkingDestination();
        }
    }
}