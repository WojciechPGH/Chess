using System;
using UnityEngine;

namespace Chess
{
    public interface IChessPieceFactory
    {
        public ChessPiece CreateChessPiece(ChessFigures figure, ChessPieceColor color, int id, Vector2Int position);
    }

    public class ChessPieceFactory : IChessPieceFactory
    {

        public ChessPiece CreateChessPiece(ChessFigures figure, ChessPieceColor color, int id, Vector2Int position)
        {
            ChessPiece piece = figure switch
            {
                ChessFigures.King => new KingPiece(id, position, color),
                ChessFigures.Queen => new QueenPiece(id, position, color),
                ChessFigures.Rook => new RookPiece(id, position, color),
                ChessFigures.Bishop => new BishopPiece(id, position, color),
                ChessFigures.Knight => new KnightPiece(id, position, color),
                ChessFigures.Pawn => new PawnPiece(id, position, color),
                _ => throw new ArgumentOutOfRangeException()
            };

            return piece;
        }
    }
}
