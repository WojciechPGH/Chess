using UnityEngine.SceneManagement;

namespace Chess
{
    public interface IGameState
    {
        public void Enter();
        public void Exit();
    }

    public class InitState : IGameState
    {
        private string _sceneName = "GameScene";
        public void Enter()
        {
            if (SceneManager.GetActiveScene().name != _sceneName)
                SceneManager.LoadScene(_sceneName);
        }

        public void Exit() { }
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
        public void Exit() { }
    }
}
