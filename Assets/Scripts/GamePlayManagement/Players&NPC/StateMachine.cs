using System;
using System.Collections.Generic;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management;

namespace GamePlayManagement.Players_NPC
{
    public interface IBaseStateMachine
    {
    }
    public class BaseStateMachine : IBaseStateMachine 
    {
        private Dictionary<Type, IState> states = new Dictionary<Type, IState>();
        private IState currentState;

        public void AddState(IState state)
        {
            states[state.GetType()] = state;
        }

        public void ChangeState<T>() where T : IState
        {
            if (currentState != null)
            {
                currentState.Exit();
            }

            currentState = states[typeof(T)];
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

    public class IdleState : IState
    {
        public BaseCharacterMovementStatus MovementStatus => BaseCharacterMovementStatus.Idle;
        public void Enter()
        {
            
        }

        public void Exit()
        {
        }

        public void Update()
        {
        }
    }
}