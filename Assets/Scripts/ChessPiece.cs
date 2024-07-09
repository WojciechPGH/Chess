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

        public ChessPieceColor Color => _color;
        public Vector2Int Position => _position;

        public ChessPiece(int id, Vector2Int position, ChessPieceColor color)
        {
            _id = id;
            _position = position;
            _color = color;
        }

        public override bool Equals(object obj)
        {
            if (obj is not ChessPiece piece) return false;
            return _id == piece._id;
        }

        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }

        public abstract List<Vector2Int> GetValidMoves(ChessBoard board);

    }

    public class PawnPiece : ChessPiece
    {
        public PawnPiece(int id, Vector2Int position, ChessPieceColor color) : base(id, position, color) { }

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
            Vector2Int? enPassant = board.GetEnPassantPosition();
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
        public KingPiece(int id, Vector2Int position, ChessPieceColor color) : base(id, position, color) { }

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
                    break;
                }
            }

            return validPositions;
        }
    }

    public class QueenPiece : ChessPiece
    {
        public QueenPiece(int id, Vector2Int position, ChessPieceColor color) : base(id, position, color) { }

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
                    if (board.IsOppenentAt(nextPosition, _color))
                    {
                        validPositions.Add(nextPosition);
                        break;
                    }
                }
            }

            return validPositions;
        }
    }

    public class KnightPiece : ChessPiece
    {
        public KnightPiece(int id, Vector2Int position, ChessPieceColor color) : base(id, position, color) { }

        public override List<Vector2Int> GetValidMoves(ChessBoard board)
        {
            throw new System.NotImplementedException();
        }
    }

    public class RookPiece : ChessPiece
    {
        public RookPiece(int id, Vector2Int position, ChessPieceColor color) : base(id, position, color) { }

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
                    if (board.IsOppenentAt(nextPosition, _color))
                    {
                        validPositions.Add(nextPosition);
                        break;
                    }
                }
            }

            return validPositions;
        }
    }

    public class BishopPiece : ChessPiece
    {
        public BishopPiece(int id, Vector2Int position, ChessPieceColor color) : base(id, position, color) { }

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
                    if (board.IsOppenentAt(nextPosition, _color))
                    {
                        validPositions.Add(nextPosition);
                        break;
                    }
                }
            }

            return validPositions;
        }
    }
}
