using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class ChessManager
    {
        private Dictionary<ChessPieceColor, uint> _currentTurn; // int currentTurn?

        private ChessManager()
        {
            _currentTurn = new Dictionary<ChessPieceColor, uint>(2)
            {
                [ChessPieceColor.White] = 1,
                [ChessPieceColor.Black] = 1
            };
        }
        #region Singleton
        private static ChessManager _instance;
        public static ChessManager Instance
        {
            get
            {
                _instance ??= new ChessManager();
                return _instance;
            }
        }
        #endregion

        public uint CurrentTurn(ChessPieceColor side)
        {
            return _currentTurn[side];
        }
    }
}
