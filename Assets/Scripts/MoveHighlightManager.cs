using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class MoveHighlightManager
    {
        private ChessManager _manager;
        private ChessPieceMono _selectedPiece;
        private List<ValidMoveHighlight> _validMovesHighlights;
        private GameObject _validMovesHighlight;

        public MoveHighlightManager(ChessManager manager, GameObject validMovesHighlight)
        {
            _manager = manager;
            _validMovesHighlight = validMovesHighlight;
            _validMovesHighlights = new List<ValidMoveHighlight>();
        }

        public void RegisterCallbacks(ChessPieceMono chessPieceMono)
        {
            chessPieceMono.OnChessPieceSelect += ChessPieceSelect;
            chessPieceMono.OnChessPieceDeselect += ChessPieceDeselect;
        }

        private void ChessPieceSelect(ChessPieceMono chessPiece)
        {
            ClearHighlights();
            _selectedPiece = chessPiece;

            List<Vector2Int> validMoves = _manager.GetValidMoves(_selectedPiece.ChessPiece);
            Vector3 slightUp = Vector3.up * 0.001f;
            foreach (Vector2Int move in validMoves)
            {
                Vector3 position = chessPiece.BoardToWorldPosition(move) + slightUp;
                GameObject highlightObject = Object.Instantiate(_validMovesHighlight, position, _validMovesHighlight.transform.rotation);
                ValidMoveHighlight highlight = highlightObject.AddComponent<ValidMoveHighlight>();
                highlight.BoardPosition = move;
                highlight.OnValidMoveClick += OnValidMoveClick;
                _validMovesHighlights.Add(highlight);
            }
        }
        private void ChessPieceDeselect(ChessPieceMono mono)
        {
            ClearHighlights();
        }

        private void OnValidMoveClick(ValidMoveHighlight highlight)
        {
            _manager.ValidMoveClick(highlight, _selectedPiece.ChessPiece);
            ClearHighlights();
        }

        public void ClearHighlights()
        {
            if (_validMovesHighlights.Count > 0)
            {
                for (int i = 0; i < _validMovesHighlights.Count; i++)
                {
                    _validMovesHighlights[i].OnValidMoveClick -= OnValidMoveClick;
                    Object.Destroy(_validMovesHighlights[i].gameObject);
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
