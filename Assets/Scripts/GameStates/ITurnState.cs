namespace Chess
{
    public interface ITurnState : IGameState
    {
        public ChessPieceColor TurnColor { get; }
        public ITurnState NextTurn { get; }

        public void SetNextTurnState(ITurnState nextTurn);
    }
}
