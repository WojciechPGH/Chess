using UnityEngine;
using UnityEngine.UIElements;

namespace Chess
{
    public class MainMenuUIHandler : MonoBehaviour
    {
        private UIDocument _UIMainMenu;
        private VisualElement _root;
        private readonly string _startBtnName = "startGameBtn";
        private readonly string _exitBtnName = "exitBtn";


        private void Start()
        {
            InitMainMenu();
        }

        private void InitMainMenu()
        {
            _UIMainMenu = GetComponent<UIDocument>();
            _root = _UIMainMenu.rootVisualElement;
            Button start = _root.Q<Button>(_startBtnName);
            start.RegisterCallback<ClickEvent>(StartButtonClick);
            Button exit = _root.Q<Button>(_exitBtnName);
            exit.RegisterCallback<ClickEvent>(ExitButtonClick);
        }


        private void StartButtonClick(ClickEvent evt)
        {
            StateMachine.Instance.ReplaceState(new GameStartState());
        }
        private void ExitButtonClick(ClickEvent evt)
        {
            StateMachine.Instance.ReplaceState(new GameExitState());
        }
    }
}
