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
        private readonly WhiteTurnState _whiteTurnState = new WhiteTurnState();
        private readonly BlackTurnState _blackTurnState = new BlackTurnState();
        private readonly PawnPromotionState _pawnPromotionState = new PawnPromotionState();

        private void Start()
        {
            _validMovesHighlights = new List<ValidMoveHighlight>();
            _stateMachine = StateMachine.Instance;
            _board = new ChessBoard();
            _board.OnChessPieceCreate += CreateChessPiece;
            _board.OnPawnPromote += OnPawnPromote;
            _promotionUIHandler.OnUIClose += OnPawnPromotionExit;
            _board.InitBoard();
            _stateMachine.ReplaceState(_whiteTurnState);
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
            _promotionUIHandler.OnPawnPromotion(piece);
            _stateMachine.AddState(_pawnPromotionState);
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

        private void OnChessPieceSelected(ChessPieceMono obj)
        {
            ClearHighlight();
            _selectedPiece = obj;
            List<Vector2Int> validMoves = obj.ChessPiece.GetValidMoves(_board);
            Vector3 slightUp = Vector3.up * 0.001f;
            foreach (Vector2Int move in validMoves)
            {
                Vector3 position = obj.BoardToWorldPosition(move);
                GameObject highlightObject = Instantiate(_validMovesHighlight, position + slightUp, _validMovesHighlight.transform.rotation);
                ValidMoveHighlight highlight = highlightObject.AddComponent<ValidMoveHighlight>();
                highlight.BoardPosition = move;
                highlight.OnValidMoveClick += OnValidMoveClick;
                _validMovesHighlights.Add(highlight);
            }
        }

        private void OnValidMoveClick(ValidMoveHighlight highlight)
        {
            IGameState gameTurn;
            gameTurn = _stateMachine.CurrentState switch
            {
                WhiteTurnState => _blackTurnState,
                BlackTurnState => _whiteTurnState,
                _ => null
            };
            if (gameTurn != null)
            {
                _stateMachine.ReplaceState(gameTurn);
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
