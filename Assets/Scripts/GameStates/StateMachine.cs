using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class StateMachine : MonoBehaviour
    {
        private static StateMachine _instance;
        private Stack<IGameState> _stateStack;
        public IGameState CurrentState => _stateStack?.Peek();
        public static StateMachine Instance
        {
            get
            {
                return _instance;
            }
        }

        public event Action<IGameState> OnStateChanged;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = GetComponent<StateMachine>();
                Initialize(new MainMenuState());
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        public void Initialize(IGameState state)
        {
            _stateStack = new Stack<IGameState>();
            _stateStack.Push(state);
            CurrentState.Enter();
            OnStateChanged?.Invoke(state);
        }

        /// <summary>
        /// Pop & Push State on stack
        /// </summary>
        /// <param name="state"></param>
        public void ReplaceState(IGameState state)
        {
            if (state == CurrentState) return;
            CurrentState?.Exit();
            if (_stateStack.Count > 0)
                _stateStack.Pop();
            _stateStack.Push(state);
            CurrentState?.Enter();
            OnStateChanged?.Invoke(state);

        }

        /// <summary>
        /// Only Push on stack
        /// </summary>
        /// <param name="state"></param>
        public void AddState(IGameState state)
        {
            if (state == CurrentState) return;

            CurrentState?.Exit();
            _stateStack.Push(state);
            CurrentState?.Enter();
            OnStateChanged?.Invoke(state);

        }

        public void RemoveState()
        {
            if (_stateStack.Count <= 1) return;
            CurrentState?.Exit();
            _stateStack.Pop();
            CurrentState?.Enter();
            OnStateChanged?.Invoke(CurrentState);
        }
    }
}
