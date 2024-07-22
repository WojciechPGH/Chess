using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public abstract class ChessPiece
    {
        protected ChessPieceColor _color;
        protected Vector2Int _position;
        protected readonly int _id;
        protected bool _hasMoved = false;

        public event Action<ChessPiece, Vector2Int> OnMove;
        public event Action<ChessPiece> OnCapture;
        public event Action<ChessPiece> OnDestroy;

        public ChessPieceColor Color => _color;
        public Vector2Int Position => _position;
        public int ID => _id;
        public ChessPiece(ChessPieceColor color, int id, Vector2Int position)
        {
            _id = id;
            _position = position;
            _color = color;
        }
        public virtual void Move(Vector2Int boardPosition)
        {
            Vector2Int previousPosition = _position;
            _position = boardPosition;
            _hasMoved = true;
            OnMove?.Invoke(this, previousPosition);
        }
        public void Captured()
        {
            OnCapture?.Invoke(this);
        }
        public void Destroy()
        {
            OnDestroy?.Invoke(this);
        }
        public abstract List<Vector2Int> GetValidMoves(ChessBoard board);

    }

    public class PawnPiece : ChessPiece
    {
        public PawnPiece(int id, Vector2Int position, ChessPieceColor color) : base(color, id, position) { }

        public override List<Vector2Int> GetValidMoves(ChessBoard board)
        {
            List<Vector2Int> validMoves = new List<Vector2Int>();
            Vector2Int direction = _color == ChessPieceColor.White ? Vector2Int.up : Vector2Int.down;
            Vector2Int oneStepForward = _position + direction;
            //1 step forward
            if (board.IsPositionEmpty(oneStepForward))
            {
                validMoves.Add(oneStepForward);
                if (_hasMoved == false)
                {
                    //2 steps forward
                    Vector2Int twoStepsForward = _position + direction * 2;
                    if (board.IsPositionEmpty(twoStepsForward))
                        validMoves.Add(twoStepsForward);
                }
            }
            //capture diagonally
            Vector2Int[] captureDirections = { oneStepForward + Vector2Int.left, oneStepForward + Vector2Int.right };
            Vector2Int? enPassant = board.EnPassantPosition;
            foreach (Vector2Int capture in captureDirections)
            {
                if (board.IsOppenentAt(capture, _color) || capture == enPassant)
                {
                    validMoves.Add(capture);
                }
            }
            return validMoves;
        }
    }
    public class KingPiece : ChessPiece
    {
        public KingPiece(int id, Vector2Int position, ChessPieceColor color) : base(color, id, position) { }

        public override List<Vector2Int> GetValidMoves(ChessBoard board)
        {
            List<Vector2Int> validPositions = new List<Vector2Int>();
            Vector2Int[] directions = { Vector2Int.left, Vector2Int.right, Vector2Int.up, Vector2Int.down, Vector2Int.one, -Vector2Int.one, Vector2Int.left + Vector2Int.up, Vector2Int.right + Vector2Int.down };
            Vector2Int direction, nextPosition;
            for (int dir = 0; dir < directions.Length; dir++)
            {
                direction = directions[dir];
                nextPosition = _position + direction;
                if (board.IsPositionEmpty(nextPosition))
                    validPositions.Add(nextPosition);
                else
                if (board.IsOppenentAt(nextPosition, _color))
                {
                    validPositions.Add(nextPosition);
                }
            }

            return validPositions;
        }
    }
    public class QueenPiece : ChessPiece
    {
        public QueenPiece(int id, Vector2Int position, ChessPieceColor color) : base(color, id, position) { }

        public override List<Vector2Int> GetValidMoves(ChessBoard board)
        {
            List<Vector2Int> validPositions = new List<Vector2Int>();
            Vector2Int[] directions = { Vector2Int.left, Vector2Int.right, Vector2Int.up, Vector2Int.down, Vector2Int.one, -Vector2Int.one, Vector2Int.left + Vector2Int.up, Vector2Int.right + Vector2Int.down };
            Vector2Int direction, nextPosition;
            for (int dir = 0; dir < directions.Length; dir++)
            {
                direction = directions[dir];
                for (int i = 1; i <= ChessBoard.BOARD_SIZE; i++)
                {
                    nextPosition = _position + direction * i;
                    if (board.IsPositionEmpty(nextPosition))
                        validPositions.Add(nextPosition);
                    else
                    {
                        if (board.IsOppenentAt(nextPosition, _color))
                        {
                            validPositions.Add(nextPosition);
                        }
                        break;
                    }
                }
            }

            return validPositions;
        }
    }
    public class KnightPiece : ChessPiece
    {
        private static readonly Vector2Int[] _knightMoves = new Vector2Int[]
        {
            new Vector2Int(2, 1),
            new Vector2Int(2, -1),
            new Vector2Int(-2, 1),
            new Vector2Int(-2, -1),
            new Vector2Int(1, 2),
            new Vector2Int(1, -2),
            new Vector2Int(-1, 2),
            new Vector2Int(-1, -2)
        };
        public KnightPiece(int id, Vector2Int position, ChessPieceColor color) : base(color, id, position) { }

        public override List<Vector2Int> GetValidMoves(ChessBoard board)
        {
            List<Vector2Int> validMoves = new List<Vector2Int>();
            foreach (Vector2Int move in _knightMoves)
            {
                Vector2Int newPosition = _position + move;
                if (board.IsPositionEmpty(newPosition) || board.IsOppenentAt(newPosition, _color))
                    validMoves.Add(newPosition);
            }
            return validMoves;
        }
    }
    public class RookPiece : ChessPiece
    {
        public RookPiece(int id, Vector2Int position, ChessPieceColor color) : base(color, id, position) { }

        public override List<Vector2Int> GetValidMoves(ChessBoard board)
        {
            List<Vector2Int> validPositions = new List<Vector2Int>();
            Vector2Int[] directions = { Vector2Int.left, Vector2Int.right, Vector2Int.up, Vector2Int.down };
            Vector2Int direction, nextPosition;
            for (int dir = 0; dir < directions.Length; dir++)
            {
                direction = directions[dir];
                for (int i = 1; i <= ChessBoard.BOARD_SIZE; i++)
                {
                    nextPosition = _position + direction * i;
                    if (board.IsPositionEmpty(nextPosition))
                        validPositions.Add(nextPosition);
                    else
                    {
                        if (board.IsOppenentAt(nextPosition, _color))
                        {
                            validPositions.Add(nextPosition);
                        }
                        break;
                    }
                }
            }

            return validPositions;
        }
    }
    public class BishopPiece : ChessPiece
    {
        public BishopPiece(int id, Vector2Int position, ChessPieceColor color) : base(color, id, position) { }

        public override List<Vector2Int> GetValidMoves(ChessBoard board)
        {
            List<Vector2Int> validPositions = new List<Vector2Int>();
            Vector2Int[] directions = { Vector2Int.one, -Vector2Int.one, Vector2Int.left + Vector2Int.up, Vector2Int.right + Vector2Int.down };
            Vector2Int direction, nextPosition;
            for (int dir = 0; dir < directions.Length; dir++)
            {
                direction = directions[dir];
                for (int i = 1; i <= ChessBoard.BOARD_SIZE; i++)
                {
                    nextPosition = _position + direction * i;
                    if (board.IsPositionEmpty(nextPosition))
                        validPositions.Add(nextPosition);
                    else
                    {
                        if (board.IsOppenentAt(nextPosition, _color))
                        {
                            validPositions.Add(nextPosition);
                        }
                        break;
                    }
                }
            }

            return validPositions;
        }
    }
}
