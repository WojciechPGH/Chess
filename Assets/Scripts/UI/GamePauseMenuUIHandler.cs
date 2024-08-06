using UnityEngine;
using UnityEngine.UIElements;

namespace Chess
{
    public class GamePauseMenuUIHandler : MonoBehaviour
    {
        private UIDocument _pauseMenu;
        private VisualElement _root;
        private readonly string _continueBtnName = "continueBtn";
        private readonly string _exitBtnName = "exitBtn";
        private bool _paused = false;
        private StateMachine _stateMachine;

        private bool Paused
        {
            get => _paused;
            set
            {
                if (_paused != value)
                {
                    _paused = value;
                    ShowMenu();
                }
            }
        }
        void Start()
        {
            _pauseMenu = GetComponent<UIDocument>();
            _stateMachine = StateMachine.Instance;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Paused = !Paused;
            }
        }

        private void ShowMenu()
        {
            _pauseMenu.enabled = Paused;
            if (Paused)
            {
                _stateMachine.AddState(new GameMenuState());
                _root = _pauseMenu.rootVisualElement;
                Button continueBtn = _root.Q<Button>(_continueBtnName);
                continueBtn.RegisterCallback<ClickEvent>(ContinueButtonClick);
                Button exitBtn = _root.Q<Button>(_exitBtnName);
                exitBtn.RegisterCallback<ClickEvent>(ExitButtonClick);
            }
            else
            {
                _stateMachine.RemoveState();
            }
        }

        private void ContinueButtonClick(ClickEvent evt)
        {
            if (Paused)
            {
                Paused = false;
                _pauseMenu.enabled = false;
                _stateMachine.RemoveState();
            }
        }

        private void ExitButtonClick(ClickEvent evt)
        {
            _stateMachine.ReplaceState(new MainMenuState());
        }
    }
}
