using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class PawnPiece : ChessPiece
    {
        private static PawnPiece _twoStepLastTurn;
        private static Vector2Int? _enPassantPosition;

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
            foreach (Vector2Int capture in captureDirections)
            {
                if ((board.IsOppenentAt(capture, _color) || capture == _enPassantPosition))
                {
                    validMoves.Add(capture);
                }
            }
            return validMoves;
        }

        public override void Move(ChessBoard board, Vector2Int boardPosition)
        {
            Vector2Int previousPosition = _position;
            _position = boardPosition;
            _hasMoved = true;
            InvokeMoveEvent(this, previousPosition);
            HandleMovement(previousPosition);
            HandlePromotion(board);
        }

        private void HandleMovement(Vector2Int previousPosition)
        {
            int deltaY = previousPosition.y - _position.y;
            if (deltaY == 2 || deltaY == -2)
            {
                SetEnPassantPosition(previousPosition);
            }
            else
            {
                CheckEnPassantCapture();
                ResetEnPassantState();
            }
        }
        private void SetEnPassantPosition(Vector2Int previousPosition)
        {
            Vector2Int enPPos = previousPosition;
            enPPos.y -= (previousPosition.y - _position.y) / 2;
            _enPassantPosition = enPPos;
            _twoStepLastTurn = this;
        }

        private void CheckEnPassantCapture()
        {
            if (_twoStepLastTurn != null && _position == _enPassantPosition)
            {
                _twoStepLastTurn.Captured();
            }
        }

        private void HandlePromotion(ChessBoard board)
        {
            float promotionLine = _color == ChessPieceColor.White ? 7 : 0;
            if (_position.y == promotionLine)
                board.PromotePawn(this);
        }
        public static void ResetEnPassantState()
        {
            _twoStepLastTurn = null;
            _enPassantPosition = null;
        }

    }
}
