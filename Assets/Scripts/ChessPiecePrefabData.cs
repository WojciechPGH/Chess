using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    [CreateAssetMenu(fileName = "ChessPieceData", menuName = "Scriptable Objects/Chess Piece Data")]
    public class ChessPiecePrefabData : ScriptableObject
    {
        public List<ChessData> chessFiguresData;

        public GameObject GetPrefab(ChessFigures figure, ChessPieceColor color)
        {
            foreach (var piece in chessFiguresData)
            {
                if (piece.figure == figure && piece.color == color)
                    return piece.prefab;
            }
            return null;
        }
    }
    [System.Serializable]
    public struct ChessData
    {
        public ChessFigures figure;
        public ChessPieceColor color;
        public GameObject prefab;
    }
}
