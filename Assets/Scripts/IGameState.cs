using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public interface IGameState
    {
        public void Enter();
        public void Update();
        public void Exit();
    }
}
