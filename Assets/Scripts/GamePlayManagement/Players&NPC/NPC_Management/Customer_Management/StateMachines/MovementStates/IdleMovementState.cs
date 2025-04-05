namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.MovementStates
{
    public class IdleMovementState : IIdleState
    {
        private BaseCharacterInScene character;

        public IdleMovementState(BaseCharacterInScene character)
        {
            this.character = character;
        }

        public void Enter()
        {
            character.ToggleNavMesh(false);
        }

        public void Exit() { }

        public void Update() { }
    }
}