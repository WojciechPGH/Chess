using System;
using UnityEngine;

namespace Chess
{
    public class ChessPieceMono : MonoBehaviour
    {
        private ChessPiece _piece;
        private Outline _outline;
        private bool _isSelected = false;
        private bool _isMouseOver = false;
        private Color _selectedColor = Color.blue;
        private Color _mouseOverColor = Color.HSVToRGB(113f / 255f, 1f, 1f);
        public ChessPiece ChessPiece { get { return _piece; } }
        public event Action<ChessPieceMono> OnChessPieceSelected;
        public event Action<ChessPieceMono> OnChessPieceDeselected;

        private void Start()
        {
            _outline = gameObject.AddComponent<Outline>();
            _outline.OutlineColor = _mouseOverColor;
            _outline.OutlineWidth = 4f;
            _outline.enabled = false;
        }

        private void OnDestroy()
        {
            if (_piece != null)
            {
                _piece.OnMove -= OnPieceMove;
                _piece.OnCapture -= OnPieceCapture;
            }
        }

        private void OnMouseEnter()
        {
            _outline.enabled = true;
            _isMouseOver = true;
        }

        private void OnMouseExit()
        {
            if (_isSelected == false)
            {
                _outline.enabled = false;
            }
            _isMouseOver = false;
        }

        private void OnMouseUpAsButton()
        {
            _isSelected = !_isSelected;
            if (_isSelected)
            {
                OnChessPieceSelected?.Invoke(this);
            }
            else
            {
                OnChessPieceDeselected?.Invoke(this);
            }
            CheckSelected();
        }

        public void Init(ChessPiece piece)
        {
            _piece = piece;
            _piece.OnMove += OnPieceMove;
            _piece.OnCapture += OnPieceCapture;
            _piece.OnDestroy += OnPieceDestroy;
        }

        private void OnPieceDestroy(ChessPiece obj)
        {
            Destroy(gameObject);
        }

        private void OnPieceCapture(ChessPiece obj)
        {
            Destroy(gameObject);
        }

        private void OnPieceMove(ChessPiece obj, Vector2Int previousPosition)
        {
            transform.position = BoardToWorldPosition(obj.Position);
        }

        private void CheckSelected()
        {
            _outline.enabled = _isSelected | _isMouseOver;
            _outline.OutlineColor = _isSelected ? _selectedColor : _mouseOverColor;
        }

        public void Deselect()
        {
            _isSelected = false;
            CheckSelected();
        }

        public Vector3 BoardToWorldPosition(Vector2Int boardPosition)
        {
            return new Vector3(boardPosition.x, 0f, boardPosition.y) * 2f;
        }
    }
}
