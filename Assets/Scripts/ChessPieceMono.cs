using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class ChessPieceMono : MonoBehaviour
    {
        private ChessPiece _piece;
        private Outline _outline;
        private bool _isSelected = false;
        private Color _selectedColor = Color.blue;
        private Color _mouseOverColor = Color.HSVToRGB(113f / 255f, 1f, 1f);
        public ChessPiece ChessPiece { get { return _piece; } }
        public event Action<ChessPieceMono> OnChessPieceSelected;

        private void Start()
        {
            _outline = gameObject.AddComponent<Outline>();
            _outline.OutlineColor = _mouseOverColor;
            _outline.OutlineWidth = 4f;
            _outline.enabled = false;
        }

        private void OnMouseEnter()
        {
            _outline.enabled = true;
        }

        private void OnMouseExit()
        {
            if (_isSelected == false)
            {
                _outline.enabled = false;
            }
        }

        private void OnMouseUpAsButton()
        {
            _isSelected = true;
            _outline.OutlineColor = _selectedColor;
            OnChessPieceSelected?.Invoke(this);
        }

        public void Init(ChessPiece piece)
        {
            _piece = piece;
        }

        public void Deselect()
        {
            _isSelected = false;
            _outline.OutlineColor = _mouseOverColor;
            _outline.enabled = false;
        }
    }
}
