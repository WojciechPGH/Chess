using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public /*abstract*/ class ChessPiece
    {
        protected ChessSide _side;
        protected ChessFifures _figure;
        protected IMovementValidation _validation;
        protected Vector2Int _position;

        public void Move(byte x, byte y)
        {
            //move monobehaviour
        }
    }

}
