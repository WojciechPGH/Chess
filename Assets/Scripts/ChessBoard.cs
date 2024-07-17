using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class ChessBoard
    {
        private ChessPiece[,] _board;
        private Vector2Int? _enPassantPosition;
        private PawnPiece _twoStepLastTurn;
        private List<ChessPiece> _capturedPieces;
        public const byte BOARD_SIZE = 8;

        public Vector2Int? EnPassantPosition { get => _enPassantPosition; set => _enPassantPosition = value; }

        public event Action<ChessPiece, ChessFigures> OnChessPieceCreate;

        public void InitBoard()
        {
            ChessFigures[] initialSetup = { ChessFigures.Rook, ChessFigures.Knight, ChessFigures.Bishop, ChessFigures.Queen, ChessFigures.King, ChessFigures.Bishop, ChessFigures.Knight, ChessFigures.Rook };
            ChessPieceFactory factory = new ChessPieceFactory();
            _capturedPieces = new List<ChessPiece>();
            _board = new ChessPiece[BOARD_SIZE, BOARD_SIZE];
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                //White
                _board[i, 0] = CreateChessPiece(factory, initialSetup[i], ChessPieceColor.White, i, new Vector2Int(i, 0));
                _board[i, 1] = CreateChessPiece(factory, ChessFigures.Pawn, ChessPieceColor.White, i + BOARD_SIZE, new Vector2Int(i, 1));
                //Black
                _board[i, 6] = CreateChessPiece(factory, ChessFigures.Pawn, ChessPieceColor.Black, i + BOARD_SIZE * 3, new Vector2Int(i, 6));
                _board[i, 7] = CreateChessPiece(factory, initialSetup[i], ChessPieceColor.Black, i + BOARD_SIZE * 2, new Vector2Int(i, 7));
            }
        }

        private ChessPiece CreateChessPiece(ChessPieceFactory factory, ChessFigures chessFifure, ChessPieceColor color, int id, Vector2Int position)
        {
            ChessPiece piece = factory.CreateChessPiece(chessFifure, color, id, position);
            OnChessPieceCreate?.Invoke(piece, chessFifure);
            piece.OnMove += piece switch
            {
                PawnPiece => OnPawnMove,
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

        private void OnPawnMove(ChessPiece pawn, Vector2Int previousPosition)
        {
            MovePieceOnBoard(pawn, previousPosition);
            HandlePawnSpecialMoves(pawn as PawnPiece, previousPosition);
            //promotion logic
        }

        private void MovePieceOnBoard(ChessPiece piece, Vector2Int previousPosition)
        {
            _board[previousPosition.x, previousPosition.y] = null;
            _board[piece.Position.x, piece.Position.y]?.Captured();
            _board[piece.Position.x, piece.Position.y] = piece;
        }

        private void ResetEnPassantState()
        {
            _twoStepLastTurn = null;
            _enPassantPosition = null;
        }

        private void HandlePawnSpecialMoves(PawnPiece pawn, Vector2Int previousPosition)
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
    }
}
