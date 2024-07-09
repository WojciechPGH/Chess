using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class StateMachine
    {
        private IGameState _currentState;

        public event Action<IGameState> StateChanged;

        public StateMachine() { }

        public void Initialize(IGameState state)
        {
            _currentState = state;
            _currentState.Enter();
            StateChanged?.Invoke(state);
        }

        public void TransitionTo(IGameState state)
        {
            _currentState.Exit();
            _currentState = state;
            _currentState.Enter();
            StateChanged?.Invoke(state);
        }
    }
}
