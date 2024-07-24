using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class ChessBoard
    {
        public const byte BOARD_SIZE = 8;
        private ChessPiece[,] _board;
        private List<ChessPiece> _capturedPieces;
        private ChessPieceFactory _factory;
        //private ChessPieceColor _currentTurnColor;
        //public ChessPieceColor CurrentTurnColor => _currentTurnColor;

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
            //_currentTurnColor = ChessPieceColor.White;
        }

        private ChessPiece CreateChessPiece(ChessFigures chessFifure, ChessPieceColor color, int id, Vector2Int position)
        {
            ChessPiece piece = _factory.CreateChessPiece(chessFifure, color, id, position);
            OnChessPieceCreate?.Invoke(piece, chessFifure);
            piece.OnCapture += OnChessPieceCapture;
            return piece;
        }

        private void OnChessPieceCapture(ChessPiece piece)
        {
            _capturedPieces.Add(_board[piece.Position.x, piece.Position.y]);
            _board[piece.Position.x, piece.Position.y] = null;
        }

        public void MovePieceOnBoard(ChessPiece piece, Vector2Int previousPosition)
        {
            _board[previousPosition.x, previousPosition.y] = null;
            _board[piece.Position.x, piece.Position.y]?.Captured();
            _board[piece.Position.x, piece.Position.y] = piece;
        }

        private bool IsInCheck(ChessPieceColor currentColor)
        {
            KingPiece king = null;
            foreach (ChessPiece piece in _board)
            {
                if (piece != null && piece.Color != currentColor && piece is KingPiece)
                    king = (KingPiece)piece;
            }
            return king.IsInCheck = IsUnderAttack(king.Position, king.Color);
        }

        private void IsCheckmate(ChessPieceColor currentColor)
        {
            ChessPiece piece;
            for (int x = 0; x < BOARD_SIZE; x++)
                for (int y = 0; y < BOARD_SIZE; y++)
                {
                    piece = _board[x, y];
                    if (piece != null && piece.Color != currentColor)
                    {
                        List<Vector2Int> validMoves = piece.GetValidMoves(this);
                        ChessPiece capturedPiece;
                        foreach (Vector2Int move in validMoves)
                        {
                            //simulate move
                            capturedPiece = _board[move.x, move.y];
                            _board[move.x, move.y] = piece;
                            _board[x, y] = null;

                            bool isCheckAfterMove = IsInCheck(currentColor);
                            //revert move
                            _board[x, y] = piece;
                            _board[move.x, move.y] = capturedPiece;
                        }
                    }
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

        public void PromotePawn(PawnPiece pawn)
        {
            OnPawnPromote?.Invoke(pawn);
        }
    }
}
