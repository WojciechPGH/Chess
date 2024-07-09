using Codice.Client.BaseCommands.BranchExplorer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class ChessBoard
    {
        public const byte BOARD_SIZE = 8;
        private ChessPiece[,] _board;
        private Vector2Int? enPassantPosition;

        public ChessBoard()
        {
            InitBoard();
        }

        private void InitBoard()
        {
            ChessFactory[] piecesSetup = { new RookPieceFactory(), new KnightPieceFactory(), new BishopPieceFactory(), new QueenPieceFactory(), new KingPieceFactory(), new BishopPieceFactory(), new KnightPieceFactory(), new RookPieceFactory() };
            ChessFactory pawnFactory = new PawnPieceFactory();
            int id = 1;
            _board = new ChessPiece[BOARD_SIZE, BOARD_SIZE];
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                //White
                _board[i, 0] = piecesSetup[i].CreateChessPiece(id++, new Vector2Int(i, 0), ChessPieceColor.White);
                _board[i, 1] = pawnFactory.CreateChessPiece(id++, new Vector2Int(i, 1), ChessPieceColor.White);
                //Black
                _board[i, 6] = pawnFactory.CreateChessPiece(id++, new Vector2Int(i, 6), ChessPieceColor.Black);
                _board[i, 7] = piecesSetup[i].CreateChessPiece(id++, new Vector2Int(i, 7), ChessPieceColor.Black);
            }
        }

        private bool PositionWithinBounds(Vector2Int position)
        {
            if ((position.x >= 0 && position.y >= 0) && (position.x < BOARD_SIZE && position.y < BOARD_SIZE))
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

        public Vector2Int? GetEnPassantPosition()
        {
            return enPassantPosition;
        }
    }
}
