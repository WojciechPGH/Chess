using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public abstract class ChessPiece
    {
        protected ChessPieceColor _color;
        protected Vector2Int _position;
        protected readonly int _id;

        //private ChessPiece() { }
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

        public abstract bool ValidateMove(Vector2Int newPosition);

        public void Move(byte x, byte y)
        {
            //move monobehaviour
        }
    }

    public class PawnPiece : ChessPiece
    {
        public PawnPiece(int id, Vector2Int position, ChessPieceColor color) : base(id, position, color) { }

        public override bool ValidateMove(Vector2Int newPosition)
        {
            throw new System.NotImplementedException();
        }
    }

    public class KingPiece : ChessPiece
    {
        public KingPiece(int id, Vector2Int position, ChessPieceColor color) : base(id, position, color) { }

        public override bool ValidateMove(Vector2Int newPosition)
        {
            throw new System.NotImplementedException();
        }
    }

    public class QueenPiece : ChessPiece
    {
        public QueenPiece(int id, Vector2Int position, ChessPieceColor color) : base(id, position, color) { }

        public override bool ValidateMove(Vector2Int newPosition)
        {
            throw new System.NotImplementedException();
        }
    }

    public class KnightPiece : ChessPiece
    {
        public KnightPiece(int id, Vector2Int position, ChessPieceColor color) : base(id, position, color) { }

        public override bool ValidateMove(Vector2Int newPosition)
        {
            throw new System.NotImplementedException();
        }
    }

    public class RookPiece : ChessPiece
    {
        public RookPiece(int id, Vector2Int position, ChessPieceColor color) : base(id, position, color) { }

        public override bool ValidateMove(Vector2Int newPosition)
        {
            throw new System.NotImplementedException();
        }
    }

    public class BishopPiece : ChessPiece
    {
        public BishopPiece(int id, Vector2Int position, ChessPieceColor color) : base(id, position, color) { }

        public override bool ValidateMove(Vector2Int newPosition)
        {
            throw new System.NotImplementedException();
        }
    }
}
