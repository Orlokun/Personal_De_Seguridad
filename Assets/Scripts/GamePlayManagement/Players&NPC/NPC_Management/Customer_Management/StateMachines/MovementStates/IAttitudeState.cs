using System.Numerics;

namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.MovementStates
{
    public interface IAttitudeState : IState
    {
        public void WalkingDestinationReached();
    }
}