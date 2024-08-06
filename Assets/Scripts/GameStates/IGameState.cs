using UnityEngine;
using UnityEngine.SceneManagement;

namespace Chess
{
    public interface IGameState
    {
        public void Enter();
        public void Exit();
    }

    public class PawnPromotionState : IGameState
    {
        private PawnPromotionUIHandler _promotionUIHandler;
        private PawnPiece _pawn;
        private PawnPromotionState() { }
        public PawnPromotionState(PawnPromotionUIHandler promotionUIHandler, PawnPiece pawn)
        {
            _promotionUIHandler = promotionUIHandler;
            _pawn = pawn;
        }

        public void Enter()
        {
            _promotionUIHandler.OnPawnPromotion(_pawn);
        }
        public void Exit()
        {
        }
    }

    public class GameStartState : IGameState
    {
        public void Enter()
        {
            SceneManager.LoadScene(Helper.GameSceneName);
        }

        public void Exit()
        {
        }
    }

    public class GameMenuState : IGameState
    {
        public void Enter()
        {
        }

        public void Exit()
        {
        }
    }
    public class GameCameraRotateState : IGameState
    {
        public void Enter()
        {
        }

        public void Exit()
        {
        }
    }


    public class GameEndState : IGameState
    {
        private GameEndUIHandler _gameEndUIHandler;
        private string _gameEndText;

        public GameEndState(GameEndUIHandler gameEndUIHandler, string gameEndText)
        {
            _gameEndUIHandler = gameEndUIHandler;
            _gameEndText = gameEndText;
        }

        public void Enter()
        {
            _gameEndUIHandler.OnGameEnd(_gameEndText);
        }

        public void Exit()
        {
        }
    }

    public class GameExitState : IGameState
    {
        public void Enter()
        {
            Application.Quit();
        }

        public void Exit()
        {
        }
    }

    public class MainMenuState : IGameState
    {
        public void Enter()
        {
            if (SceneManager.GetActiveScene().name != Helper.MainMenuSceneName)
                SceneManager.LoadScene(Helper.MainMenuSceneName);
        }

        public void Exit()
        {
        }
    }
}
