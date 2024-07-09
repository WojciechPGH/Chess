using UnityEngine;

namespace Chess
{
    public enum ChessFifures
    {
        King = 1,
        Queen,
        Rook,
        Bishop,
        Knight,
        Pawn
    }

    public enum ChessPieceColor
    {
        White = 1,
        Black = 2
    }

    public static class MathfExtensions
    {
        public static int ManhattanDistance(this Vector2Int from, Vector2Int to)
        {
            return (Mathf.Abs(to.x - from.x) + Mathf.Abs(to.y - from.y));
        }
    }
}