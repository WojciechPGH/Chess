namespace Chess
{
    public class WhiteTurnState : ITurnState
    {
        private readonly ChessPieceColor _color;
        private ITurnState _nextTurn = null;

        public WhiteTurnState(ChessPieceColor color)
        {
            _color = color;
        }

        public ChessPieceColor TurnColor => _color;
        public ITurnState NextTurn => _nextTurn;
        public void SetNextTurnState(ITurnState nextTurn)
        {
            _nextTurn ??= nextTurn;
        }
        public void Enter() { }

        public void Exit() { }
    }
}