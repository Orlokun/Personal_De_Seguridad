namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines
{
    public interface IState
    {
        void Enter();
        void Exit();
        void Update();
    }
}