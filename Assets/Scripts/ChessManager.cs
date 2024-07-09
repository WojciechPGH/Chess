using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class ChessManager
    {
        private ChessManager()
        {
            _currentTurn = new Dictionary<ChessSide, uint>(2)
            {
                [ChessSide.White] = 1,
                [ChessSide.Black] = 1
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
        private Dictionary<ChessSide, uint> _currentTurn;

        public uint CurrentTurn(ChessSide side)
        {
            return _currentTurn[side];
        }

        public bool IsCellEmpty(ChessSide side, Vector2Int cell)
        {
            return false;
        }

        public bool IsEnemyAt(ChessSide side, Vector2Int cell)
        {
            return false;
        }
    }
}
