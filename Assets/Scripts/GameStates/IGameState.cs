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
        public void Enter() { }
        public void Exit() { }
    }
}
