using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class PawnMovement : IMovementValidation
    {
        private readonly ChessSide _side;
        private readonly ChessManager _manager;

        public PawnMovement(ChessSide side, ChessManager manager)
        {
            _side = side;
            _manager = manager;
        }

        public bool Validate(Vector2Int currentPosition, Vector2Int movePosition)
        {
            Vector2Int delta = currentPosition - movePosition;
            int deltaY = Mathf.Abs(delta.y);
            int deltaX = Mathf.Abs(delta.x);
            if (deltaX == 0 && deltaY == 2 && _manager.CurrentTurn(_side) == 1)
            {
                if (_manager.IsCellEmpty(_side, movePosition) && _manager.IsCellEmpty(_side, currentPosition - delta / 2))
                    return true;
            }
            else
            if (deltaY == 1)
            {
                if (deltaX == 0 && _manager.IsCellEmpty(_side, movePosition))
                    return true;
                if (deltaX == 1 && _manager.IsEnemyAt(_side, movePosition))
                    return true;
                if (deltaX == 1/* && enemy is at currentPosition.x +-1, enemy is pawn, enemy used two-step advance in last turn */)
                    return true;

            }
            return false;
            //en passant
            //promotion
        }
    }


    public interface IMovementValidation
    {
        public bool Validate(Vector2Int currentPosition, Vector2Int movePosition);
    }
}
