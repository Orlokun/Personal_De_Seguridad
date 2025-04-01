namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines
{
    public interface IStateMachine<T> where T : IState
    {
        void Update();
        public T CurrentState { get; }

    }
}