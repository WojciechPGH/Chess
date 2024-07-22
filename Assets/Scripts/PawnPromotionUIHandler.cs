using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Chess
{
    public class PawnPromotionUIHandler : MonoBehaviour
    {
        private UIDocument _UIPromotion;
        private PawnPiece _pawn;
        private VisualElement _root;
        private readonly string _btnClassName = "PromotionBtn";

        public event Action<PawnPiece, ChessFigures> OnPawnPromoted;
        private void Awake()
        {
            _UIPromotion = GetComponent<UIDocument>();
        }

        public void OnPawnPromotion(PawnPiece pawn)
        {
            _UIPromotion.enabled = true;
            _pawn = pawn;
            _root = _UIPromotion.rootVisualElement;
            RegisterCallbacks();
        }

        private void RegisterCallbacks()
        {
            UQueryBuilder<Button> btns = _root.Query<Button>(className: _btnClassName);
            btns.ForEach(btn => { btn.RegisterCallback<ClickEvent>(OnButtonClick); });
        }

        private void OnButtonClick(ClickEvent evt)
        {
            Button b = evt.target as Button;
            Debug.Log("Click " + b.name);
            ChessFigures figure = b.name switch
            {
                "BtnKnight" => ChessFigures.Knight,
                "BtnBishop" => ChessFigures.Bishop,
                "BtnRook" => ChessFigures.Rook,
                "BtnQueen" => ChessFigures.Queen,
                _ => ChessFigures.Pawn,
            };
            OnPawnPromoted?.Invoke(_pawn, figure);
            _pawn = null;
            _UIPromotion.enabled = false;
        }
    }
}
