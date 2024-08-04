namespace Chess
{
    public enum ChessFigures
    {
        King = 1,
        Queen,
        Rook,
        Bishop,
        Knight,
        Pawn
    }

    public enum ChessPieceColor
    {
        White = 1,
        Black = 2
    }

    public static class Helper
    {
        public static string MainMenuSceneName => "MainMenuScene";
        public static string GameSceneName => "GameScene";
        public static ChessPieceColor GetOpositeColor(ChessPieceColor color)
        {
            return color == ChessPieceColor.White ? ChessPieceColor.Black : ChessPieceColor.White;
        }
    }
}