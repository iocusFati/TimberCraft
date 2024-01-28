using System;
using System.Collections.Generic;

namespace Infrastructure.States
{
    public class StateMachine : IGameStateMachine
    {
        protected Dictionary<Type, IExitState> _states;
        private IExitState _currentState;

        protected StateMachine()
        {
            _states = new Dictionary<Type, IExitState>();
        }

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }

        public void RegisterState<TState>(TState state) where TState : IExitState =>
            _states.Add(typeof(TState), state);

        private TState ChangeState<TState>() where TState : class, IExitState
        {
            _currentState?.Exit();

            TState state = GetState<TState>();
            _currentState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class, IExitState => 
            _states[typeof(TState)] as TState;
    }
}