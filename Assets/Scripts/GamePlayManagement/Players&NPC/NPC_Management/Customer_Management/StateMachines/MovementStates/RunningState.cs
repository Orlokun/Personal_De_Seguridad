namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.MovementStates
{
    public class RunningState : IMovementState
    {
        private BaseCharacterInScene character;
        protected const string Run = "Run";        

        public RunningState(BaseCharacterInScene character)
        {
            this.character = character;
        }

        public void Enter()
        {
            //Change Character Speed
            character.BaseAnimator.ChangeAnimationState(Run);
        }

        public void Exit() { }

        public void Update()
        {
            character.EvaluateWalkingDestination();
        }
    }
}