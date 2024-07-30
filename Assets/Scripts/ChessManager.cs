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
        private PawnPromotionUIHandler _promotionUIHandler;
        private StateMachine _stateMachine;
        private ChessBoard _board;
        private ChessPieceMono _selectedPiece;
        private List<ValidMoveHighlight> _validMovesHighlights;
        private WhiteTurnState _whiteTurnState;
        private BlackTurnState _blackTurnState;

        private void Start()
        {
            TurnsInit();
            _validMovesHighlights = new List<ValidMoveHighlight>();
            _stateMachine = StateMachine.Instance;
            _board = new ChessBoard();
            _board.OnChessPieceCreate += CreateChessPiece;
            _board.OnPawnPromote += OnPawnPromote;
            _promotionUIHandler.OnUIClose += OnPawnPromotionExit;
            _board.InitBoard();
            _stateMachine.ReplaceState(_whiteTurnState);
        }

        private void TurnsInit()
        {
            _whiteTurnState = new WhiteTurnState(ChessPieceColor.White);
            _blackTurnState = new BlackTurnState(ChessPieceColor.Black);
            _whiteTurnState.SetNextTurnState(_blackTurnState);
            _blackTurnState.SetNextTurnState(_whiteTurnState);
        }

        private void OnDestroy()
        {
            _board.OnChessPieceCreate -= CreateChessPiece;
            _board.OnPawnPromote -= OnPawnPromote;
            _promotionUIHandler.OnUIClose -= _board.OnPawnPromoted;
        }

        private void OnPawnPromotionExit(PawnPiece piece, ChessFigures figure)
        {
            _board.OnPawnPromoted(piece, figure);
            _stateMachine.RemoveState();
        }

        private void OnPawnPromote(PawnPiece piece)
        {
            _stateMachine.AddState(new PawnPromotionState(_promotionUIHandler, piece));
        }


        private void CreateChessPiece(ChessPiece piece, ChessFigures figure)
        {
            GameObject inst = Instantiate(_prefabData.GetPrefab(figure, piece.Color), transform);
            ChessPieceMono mono = inst.AddComponent<ChessPieceMono>();
            inst.transform.position = mono.BoardToWorldPosition(piece.Position);
            mono.Init(piece);
            mono.OnChessPieceSelected += OnChessPieceSelected;
            mono.OnChessPieceDeselected += OnChessPieceDeselected;
            mono.OnDestroyEvent += OnChessPieceDestroy;
            _stateMachine.OnStateChanged += mono.TurnChanged;
        }

        private void OnChessPieceDestroy(ChessPieceMono mono)
        {
            mono.OnChessPieceSelected -= OnChessPieceSelected;
            mono.OnChessPieceDeselected -= OnChessPieceDeselected;
            _stateMachine.OnStateChanged -= mono.TurnChanged;
            mono.OnDestroyEvent -= OnChessPieceDestroy;
        }

        private void OnChessPieceDeselected(ChessPieceMono obj)
        {
            ClearHighlight();
        }

        private void OnChessPieceSelected(ChessPieceMono chessPiece)
        {
            ClearHighlight();
            _selectedPiece = chessPiece;
            List<Vector2Int> validMoves = chessPiece.ChessPiece.GetValidMoves(_board);
            Vector3 slightUp = Vector3.up * 0.001f;
            foreach (Vector2Int move in validMoves)
            {
                if (_board.SimulateMove(chessPiece.ChessPiece, move))
                {
                    Vector3 position = chessPiece.BoardToWorldPosition(move);
                    GameObject highlightObject = Instantiate(_validMovesHighlight, position + slightUp, _validMovesHighlight.transform.rotation);
                    ValidMoveHighlight highlight = highlightObject.AddComponent<ValidMoveHighlight>();
                    highlight.BoardPosition = move;
                    highlight.OnValidMoveClick += OnValidMoveClick;
                    _validMovesHighlights.Add(highlight);
                }
            }
        }

        private void OnValidMoveClick(ValidMoveHighlight highlight)
        {
            ITurnState nextTurn;
            nextTurn = _stateMachine.CurrentState is ITurnState turnState ? turnState.NextTurn : null;
            if (nextTurn != null)
            {
                _stateMachine.ReplaceState(nextTurn);
                _selectedPiece.ChessPiece.Move(_board, highlight.BoardPosition);
                ClearHighlight();
            }
        }

        private void ClearHighlight()
        {
            if (_validMovesHighlights.Count > 0)
            {
                for (int i = 0; i < _validMovesHighlights.Count; i++)
                {
                    _validMovesHighlights[i].OnValidMoveClick -= OnValidMoveClick;
                    Destroy(_validMovesHighlights[i].gameObject);
                }
            }
            _validMovesHighlights.Clear();
            if (_selectedPiece != null)
            {
                _selectedPiece.Deselect();
                _selectedPiece = null;
            }
        }

    }
}
