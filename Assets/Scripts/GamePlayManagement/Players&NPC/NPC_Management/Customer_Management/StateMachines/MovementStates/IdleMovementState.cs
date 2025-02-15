namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.MovementStates
{
    public class IdleMovementState : IIdleState
    {
        private BaseCharacterInScene character;
        private const string Idle = "Idle";

        public IdleMovementState(BaseCharacterInScene character)
        {
            this.character = character;
        }

        public void Enter()
        {
            character.BaseAnimator.ChangeAnimationState(Idle);
            character.ToggleNavMesh(false);
        }

        public void Exit() { }

        public void Update() { }
    }

    public interface IIdleState : IMovementState
    {
        
    }
}