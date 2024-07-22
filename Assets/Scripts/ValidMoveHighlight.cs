using System;
using UnityEngine;

namespace Chess
{
    public class ValidMoveHighlight : MonoBehaviour
    {
        private Vector2Int _boardPosition;
        public event Action<ValidMoveHighlight> OnValidMoveClick;
        public Vector2Int BoardPosition { get => _boardPosition; set => _boardPosition = value; }
        private void OnMouseUpAsButton()
        {
            OnValidMoveClick?.Invoke(this);
        }
    }
}
