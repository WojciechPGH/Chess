using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class ValidMoveHighlight : MonoBehaviour
    {
        public event Action<ValidMoveHighlight> OnValidMoveClick;

        private void OnMouseUpAsButton()
        {
            OnValidMoveClick?.Invoke(this);
        }
    }
}
