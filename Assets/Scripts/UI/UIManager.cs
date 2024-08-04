using System;
using UnityEngine;

namespace Chess
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private GameEndUIHandler _gameEndUIHandler;
        [SerializeField]
        private PawnPromotionUIHandler _promotionUIHandler;
        [SerializeField]
        private CheckPopupUIHandler _checkPopupUIHandler;
        [SerializeField]
        private ChessManager _chessManager;
        private ChessBoard _chessBoard;
        private StateMachine _stateMachine;

        private void OnDestroy()
        {
            _promotionUIHandler.OnUIClose -= PawnPromotionFinish;
            _chessBoard.OnCheck -= ShowCheckPopup;
            _chessBoard.OnCheckmate -= ShowGameEndPopup;
            _chessBoard.OnStalemate -= ShowGameEndPopup;
            _chessBoard.OnPawnPromote -= PawnPromotePopup;
        }

        public void Init(ChessBoard board, StateMachine stateMachine)
        {
            _chessBoard = board;
            _stateMachine = stateMachine;
            _promotionUIHandler.OnUIClose += PawnPromotionFinish;
            _chessBoard.OnCheck += ShowCheckPopup;
            _chessBoard.OnCheckmate += ShowGameEndPopup;
            _chessBoard.OnStalemate += ShowGameEndPopup;
            _chessBoard.OnPawnPromote += PawnPromotePopup;
        }

        private void PawnPromotionFinish(PawnPiece pawn, ChessFigures figure)
        {
            pawn.Destroy();
            _chessBoard.OnPawnPromoted(pawn, figure);
            _stateMachine.RemoveState();
        }

        public void PawnPromotePopup(PawnPiece pawn)
        {
            _stateMachine.AddState(new PawnPromotionState(_promotionUIHandler, pawn));
        }

        public void ShowCheckPopup()
        {
            _checkPopupUIHandler.ShowPopup();
        }

        public void ShowGameEndPopup(string gameEndString)
        {
            _stateMachine.AddState(new GameEndState(_gameEndUIHandler, gameEndString));
        }

    }
}
