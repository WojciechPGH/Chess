using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class ChessManager : MonoBehaviour
    {
        [SerializeField]
        private ChessPiecePrefabData _prefabData;
        [SerializeField]
        private GameObject _validMovesHighlight;
        [SerializeField]
        private UIManager _UIManager;
        private StateMachine _stateMachine;
        private ChessBoard _board;
        private MoveHighlightManager _highlightManager;
        private WhiteTurnState _whiteTurnState;
        private BlackTurnState _blackTurnState;

        private void Start()
        {
            TurnsInit();
            _highlightManager = new MoveHighlightManager(this, _validMovesHighlight);
            _stateMachine = StateMachine.Instance;
            _board = new ChessBoard();
            _board.OnChessPieceCreate += CreateChessPiece;
            _board.InitBoard();
            _UIManager.Init(_board, _stateMachine);
            _stateMachine.Initialize(_whiteTurnState);
        }
        private void OnDestroy()
        {
            _board.OnChessPieceCreate -= CreateChessPiece;
        }

        private void TurnsInit()
        {
            _whiteTurnState = new WhiteTurnState(ChessPieceColor.White);
            _blackTurnState = new BlackTurnState(ChessPieceColor.Black);
            _whiteTurnState.SetNextTurnState(_blackTurnState);
            _blackTurnState.SetNextTurnState(_whiteTurnState);
        }

        private void CreateChessPiece(ChessPiece piece, ChessFigures figure)
        {
            GameObject inst = Instantiate(_prefabData.GetPrefab(figure, piece.Color), transform);
            ChessPieceMono mono = inst.AddComponent<ChessPieceMono>();
            mono.Init(piece);
            _highlightManager.RegisterCallbacks(mono);
        }

        public List<Vector2Int> GetValidMoves(ChessPiece piece)
        {
            return piece.GetValidMoves(_board).FindAll(move => PruneValidMoves(piece, move));
        }
        private bool PruneValidMoves(ChessPiece piece, Vector2Int move)
        {
            if (piece is KingPiece kingPiece)
            {
                float deltaX = kingPiece.Position.x - move.x;
                if (deltaX == 2 || deltaX == -2)
                {
                    bool kingSide = deltaX == 2;
                    return kingPiece.CanCastle(_board, kingSide) & _board.SimulateMove(kingPiece, move);
                }
            }
            return _board.SimulateMove(piece, move);
        }
        public void ValidMoveClick(ValidMoveHighlight highlight, ChessPiece piece)
        {
            ITurnState nextTurn;
            nextTurn = _stateMachine.CurrentState is ITurnState turnState ? turnState.NextTurn : null;
            if (nextTurn != null)
            {
                _stateMachine.ReplaceState(nextTurn);
                piece.Move(_board, highlight.BoardPosition);
            }
        }
    }
}
