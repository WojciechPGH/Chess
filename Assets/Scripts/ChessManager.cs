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
        private uint _currentTurn;
        private ChessBoard _board;
        private ChessPieceMono _selectedPiece;
        private List<ValidMoveHighlight> _validMovesHighlights;
        public uint CurrentTurn => _currentTurn;

        private void Start()
        {
            _currentTurn = 1;
            _validMovesHighlights = new List<ValidMoveHighlight>();
            _board = new ChessBoard();
            _board.OnChessPieceCreate += CreateChessPiece;
            _board.InitBoard();
        }

        private void OnDestroy()
        {
            _board.OnChessPieceCreate -= CreateChessPiece;
        }

        private void CreateChessPiece(ChessPiece piece, ChessFigures figure)
        {

            GameObject inst = Instantiate(_prefabData.GetPrefab(figure, piece.Color), transform);
            ChessPieceMono mono = inst.AddComponent<ChessPieceMono>();
            inst.transform.position = mono.BoardToWorldPosition(piece.Position);
            mono.Init(piece);
            mono.OnChessPieceSelected += OnChessPieceSelected;
            mono.OnChessPieceDeselected += OnChessPieceDeselected;
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
            foreach (Vector2Int move in validMoves)
            {
                Vector3 position = obj.BoardToWorldPosition(move);
                GameObject highlightObject = Instantiate(_validMovesHighlight, position + Vector3.up * 0.001f, _validMovesHighlight.transform.rotation);
                ValidMoveHighlight highlight = highlightObject.AddComponent<ValidMoveHighlight>();
                highlight.BoardPosition = move;
                highlight.OnValidMoveClick += OnValidMoveClick;
                _validMovesHighlights.Add(highlight);
            }
        }

        private void OnValidMoveClick(ValidMoveHighlight obj)
        {
            Debug.Log("Valid move click");
            _selectedPiece.ChessPiece.Move(obj.BoardPosition);
            ClearHighlight();
            //next turn
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
