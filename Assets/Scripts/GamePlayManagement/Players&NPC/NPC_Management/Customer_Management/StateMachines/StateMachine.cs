using System;
using System.Collections.Generic;

namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines
{
    public interface IStateMachine<T> where T : IState
    {
        void Update();
        public T CurrentState { get; }

    }
    
    public class StateMachine<T> : IStateMachine<T> where T : IState
    {
        private Dictionary<Type, T> states = new Dictionary<Type, T>();
        private T currentState;
        public T CurrentState => currentState;

        public void AddState(T state)
        {
            states[state.GetType()] = state;
        }

        public void ChangeState<U>() where U : T
        {
            currentState?.Exit();
            currentState = states[typeof(U)];
            currentState.Enter();
        }

        public void Update()
        {
            currentState?.Update();
        }
    }

    public interface IState
    {
        void Enter();
        void Exit();
        void Update();
    }
}