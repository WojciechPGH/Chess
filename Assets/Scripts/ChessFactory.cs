using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public abstract class ChessFactory
    {
        public abstract ChessPiece CreateChessPiece(int id, Vector2Int position, ChessPieceColor side);
    }

    public class PawnPieceFactory : ChessFactory
    {
        public override ChessPiece CreateChessPiece(int id, Vector2Int position, ChessPieceColor side)
        {
            return new PawnPiece(id, position, side);
        }
    }

    public class KingPieceFactory : ChessFactory
    {
        public override ChessPiece CreateChessPiece(int id, Vector2Int position, ChessPieceColor side)
        {
            return new KingPiece(id, position, side);
        }
    }

    public class QueenPieceFactory : ChessFactory
    {
        public override ChessPiece CreateChessPiece(int id, Vector2Int position, ChessPieceColor side)
        {
            return new QueenPiece(id, position, side);
        }
    }

    public class RookPieceFactory : ChessFactory
    {
        public override ChessPiece CreateChessPiece(int id, Vector2Int position, ChessPieceColor side)
        {
            return new RookPiece(id, position, side);
        }
    }

    public class KnightPieceFactory : ChessFactory
    {
        public override ChessPiece CreateChessPiece(int id, Vector2Int position, ChessPieceColor side)
        {
            return new KnightPiece(id, position, side);
        }
    }

    public class BishopPieceFactory : ChessFactory
    {
        public override ChessPiece CreateChessPiece(int id, Vector2Int position, ChessPieceColor side)
        {
            return new BishopPiece(id, position, side);
        }
    }
}
