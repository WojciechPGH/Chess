using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class KingPiece : ChessPiece
    {
        private bool _inCheck = false;
        public bool IsInCheck { get => _inCheck; set => _inCheck = value; }
        public KingPiece(int id, Vector2Int position, ChessPieceColor color) : base(color, id, position) { }

        public override void Move(ChessBoard board, Vector2Int boardPosition)
        {
            Vector2Int previousPosition = _position;
            base.Move(board, boardPosition);
            HandleCastling(board, previousPosition);
        }

        public override List<Vector2Int> GetValidMoves(ChessBoard board)
        {
            List<Vector2Int> validPositions = new List<Vector2Int>();
            Vector2Int[] directions = { Vector2Int.left, Vector2Int.right, Vector2Int.up, Vector2Int.down, Vector2Int.one, -Vector2Int.one, Vector2Int.left + Vector2Int.up, Vector2Int.right + Vector2Int.down };
            Vector2Int direction, nextPosition;
            for (int dir = 0; dir < directions.Length; dir++)
            {
                direction = directions[dir];
                nextPosition = _position + direction;
                if (board.IsPositionEmpty(nextPosition) || board.IsOppenentAt(nextPosition, _color))
                    validPositions.Add(nextPosition);
            }
            if (_hasMoved == false && !_inCheck)
            {
                if (CastlingValidation(board, true) == true)
                    validPositions.Add(_position + Vector2Int.right * 2);
                if (CastlingValidation(board, false) == true)
                    validPositions.Add(_position - (Vector2Int.right * 2));
            }
            return validPositions;
        }

        private void HandleCastling(ChessBoard board, Vector2Int previousPosition)
        {
            int deltaX = _position.x - previousPosition.x;
            //Castling
            if (deltaX == 2 || deltaX == -2)
            {
                bool kingSide = deltaX == 2;
                ChessPiece rook = kingSide == true ? board.GetPiece(7, _position.y) : board.GetPiece(0, _position.y);
                if (rook != null && rook is RookPiece)
                {
                    Vector2Int rookPosition = (_position + previousPosition) / 2;
                    rook.Move(board, rookPosition);
                }
            }
        }

        private bool CastlingValidation(ChessBoard board, bool kingSide)
        {
            ChessPiece rook = kingSide == true ? board.GetPiece(7, _position.y) : board.GetPiece(0, _position.y);
            if (rook == null || rook is not RookPiece || rook.HasMoved == true) return false;
            int startX = (kingSide == true ? _position.x : rook.Position.x) + 1;
            Vector2Int positionCheck = new Vector2Int(startX, _position.y);
            for (int i = startX; i < startX + 2; i++)
            {
                positionCheck.x = i;
                if (!board.IsPositionEmpty(positionCheck))
                    return false;
            }
            return true;
        }

        public bool CanCastle(ChessBoard board, bool kingSide)
        {
            ChessPiece rook = kingSide == true ? board.GetPiece(7, _position.y) : board.GetPiece(0, _position.y);
            if (rook == null || rook is not RookPiece || rook.HasMoved == true) return false;
            int startX = (kingSide == true ? _position.x : rook.Position.x) + 1;
            Vector2Int positionCheck = new Vector2Int(startX, _position.y);
            for (int i = startX; i < startX + 2; i++)
            {
                positionCheck.x = i;
                if (!board.IsPositionEmpty(positionCheck) || board.IsUnderAttack(positionCheck, _color))
                    return false;
            }
            return true;
        }
    }
}
