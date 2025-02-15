namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.MovementStates
{
    public interface IMovementState : IState
    {

    }

    public interface IAttitudeState : IState
    {
        public void WalkingDestinationReached();
    }
}