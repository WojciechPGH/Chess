using Codice.CM.Client.Differences;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Chess
{
    public class ChessBoard
    {
        private ChessPiece[,] _board;
        private Vector2Int? _enPassantPosition;
        private PawnPiece _twoStepLastTurn;
        private List<ChessPiece> _capturedPieces;
        private ChessPieceFactory _factory;
        public const byte BOARD_SIZE = 8;

        public Vector2Int? EnPassantPosition { get => _enPassantPosition; set => _enPassantPosition = value; }

        public event Action<ChessPiece, ChessFigures> OnChessPieceCreate;
        public event Action<PawnPiece> OnPawnPromote;

        public void InitBoard()
        {
            ChessFigures[] initialSetup = { ChessFigures.Rook, ChessFigures.Knight, ChessFigures.Bishop, ChessFigures.Queen, ChessFigures.King, ChessFigures.Bishop, ChessFigures.Knight, ChessFigures.Rook };
            _factory = new ChessPieceFactory();
            _capturedPieces = new List<ChessPiece>();
            _board = new ChessPiece[BOARD_SIZE, BOARD_SIZE];
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                //White
                _board[i, 0] = CreateChessPiece(initialSetup[i], ChessPieceColor.White, i, new Vector2Int(i, 0));
                _board[i, 1] = CreateChessPiece(ChessFigures.Pawn, ChessPieceColor.White, i + BOARD_SIZE, new Vector2Int(i, 1));
                //Black
                _board[i, 6] = CreateChessPiece(ChessFigures.Pawn, ChessPieceColor.Black, i + BOARD_SIZE * 3, new Vector2Int(i, 6));
                _board[i, 7] = CreateChessPiece(initialSetup[i], ChessPieceColor.Black, i + BOARD_SIZE * 2, new Vector2Int(i, 7));
            }
        }

        private ChessPiece CreateChessPiece(ChessFigures chessFifure, ChessPieceColor color, int id, Vector2Int position)
        {
            ChessPiece piece = _factory.CreateChessPiece(chessFifure, color, id, position);
            OnChessPieceCreate?.Invoke(piece, chessFifure);
            piece.OnMove += piece switch
            {
                PawnPiece => OnPawnMove,
                KingPiece => OnKingMove,
                _ => OnChessPieceMove
            };
            piece.OnCapture += OnChessPieceCapture;
            return piece;
        }

        private void OnChessPieceCapture(ChessPiece piece)
        {
            _capturedPieces.Add(_board[piece.Position.x, piece.Position.y]);
        }

        private void OnChessPieceMove(ChessPiece piece, Vector2Int previousPosition)
        {
            MovePieceOnBoard(piece, previousPosition);
            ResetEnPassantState();
        }

        private void OnPawnMove(ChessPiece piece, Vector2Int previousPosition)
        {
            PawnPiece pawn = piece as PawnPiece;
            MovePieceOnBoard(pawn, previousPosition);
            HandlePawnMoves(pawn, previousPosition);
            HandlePawnPromotion(pawn);
        }

        private void OnKingMove(ChessPiece piece, Vector2Int previousPosition)
        {
            KingPiece king = piece as KingPiece;
            MovePieceOnBoard(king, previousPosition);
            HandleCastling(king, previousPosition);
        }


        private void MovePieceOnBoard(ChessPiece piece, Vector2Int previousPosition)
        {
            _board[previousPosition.x, previousPosition.y] = null;
            _board[piece.Position.x, piece.Position.y]?.Captured();
            _board[piece.Position.x, piece.Position.y] = piece;
        }
        private void HandleCastling(KingPiece king, Vector2Int previousPosition)
        {
            int deltaX = king.Position.x - previousPosition.x;
            //Castling
            if (deltaX == 2 || deltaX == -2)
            {
                bool kingSide = deltaX == 2;
                ChessPiece rook = kingSide == true ? _board[7, king.Position.y] : _board[0, king.Position.y];
                if (rook != null && rook is RookPiece)
                {
                    Vector2Int rookPosition = (king.Position + previousPosition) / 2;
                    rook.Move(rookPosition);
                }
            }
        }

        private void HandlePawnMoves(PawnPiece pawn, Vector2Int previousPosition)
        {
            int deltaY = Mathf.Abs(previousPosition.y - pawn.Position.y);
            if (deltaY == 2)
            {
                SetEnPassantPosition(pawn, previousPosition);
            }
            else
            {
                CheckEnPassantCapture(pawn);
                ResetEnPassantState();
            }
        }

        private void SetEnPassantPosition(PawnPiece pawn, Vector2Int previousPosition)
        {
            Vector2Int enPPos = previousPosition;
            enPPos.y -= (previousPosition.y - pawn.Position.y) / 2;
            _enPassantPosition = enPPos;
            _twoStepLastTurn = pawn;
        }

        private void CheckEnPassantCapture(PawnPiece pawn)
        {
            if (_twoStepLastTurn != null && pawn.Position == _enPassantPosition)
            {
                _twoStepLastTurn.Captured();
                _board[_twoStepLastTurn.Position.x, _twoStepLastTurn.Position.y] = null;
            }
        }
        private void ResetEnPassantState()
        {
            _twoStepLastTurn = null;
            _enPassantPosition = null;
        }
        private void HandlePawnPromotion(PawnPiece pawn)
        {
            if ((pawn.Color == ChessPieceColor.White && pawn.Position.y == 7) || (pawn.Color == ChessPieceColor.Black && pawn.Position.y == 0))
            {
                OnPawnPromote?.Invoke(pawn);
            }
        }

        private bool PositionWithinBounds(Vector2Int position)
        {
            if (position.x >= 0 && position.y >= 0 && position.x < BOARD_SIZE && position.y < BOARD_SIZE)
                return true;
            return false;
        }

        public bool IsPositionEmpty(Vector2Int position)
        {
            if (PositionWithinBounds(position) && _board[position.x, position.y] == null) return true;
            return false;
        }

        public bool IsOppenentAt(Vector2Int position, ChessPieceColor pieceColor)
        {
            if (PositionWithinBounds(position) && !IsPositionEmpty(position))
            {
                if (_board[position.x, position.y].Color != pieceColor) return true;
            }
            return false;
        }

        public bool IsUnderAttack(Vector2Int position, ChessPieceColor pieceColor)
        {
            foreach (ChessPiece piece in _board)
            {
                if (piece != null && piece.Color != pieceColor)
                {
                    List<Vector2Int> validMoves;
                    validMoves = (piece is KingPiece) ? (piece as KingPiece).GetAttackMoves(this) : piece.GetValidMoves(this);
                    if (validMoves.Contains(position)) return true;
                }
            }
            return false;
        }

        public ChessPiece GetPiece(Vector2Int position)
        {
            if (PositionWithinBounds(position))
                return _board[position.x, position.y];
            return null;
        }

        public ChessPiece GetPiece(int x, int y)
        {
            if (PositionWithinBounds(new Vector2Int(x, y)))
                return _board[x, y];
            return null;
        }

        public void OnPawnPromoted(PawnPiece pawn, ChessFigures figure)
        {
            pawn.Destroy();
            _board[pawn.Position.x, pawn.Position.y] = CreateChessPiece(figure, pawn.Color, pawn.ID, pawn.Position);
        }
    }
}
