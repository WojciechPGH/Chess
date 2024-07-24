namespace Chess
{
    public class BlackTurnState : IGameState
    {
        private readonly ChessPieceColor _color = ChessPieceColor.Black;
        public ChessPieceColor TurnColor => _color;
        public void Enter()
        {

        }

        public void Exit()
        {

        }
    }
}
