namespace Chess
{
    public class WhiteTurnState : IGameState
    {
        private readonly ChessPieceColor _color = ChessPieceColor.White;
        public ChessPieceColor TurnColor => _color;

        public void Enter()
        {
            
        }

        public void Exit()
        {
            
        }
    }
}
