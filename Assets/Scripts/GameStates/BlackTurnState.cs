namespace Chess
{
    public class BlackTurnState : ITurnState
    {
        private readonly ChessPieceColor _color;
        private ITurnState _nextTurn = null;

        public BlackTurnState(ChessPieceColor color)
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
