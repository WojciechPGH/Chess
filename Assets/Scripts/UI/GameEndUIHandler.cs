using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Chess
{
    public class GameEndUIHandler : MonoBehaviour
    {
        private UIDocument _UIGameEnd;
        private VisualElement _root;
        private readonly string _exitBtnName = "exitBtn";
        private readonly string _restartBtnName = "restartBtn";
        private readonly string _labelName = "endgameLabel";

        private void Awake()
        {
            _UIGameEnd = GetComponent<UIDocument>();
        }

        public void OnGameEnd(string gameEndText)
        {
            _UIGameEnd.enabled = true;
            _root = _UIGameEnd.rootVisualElement;
            RegisterCallbacks(gameEndText);
        }

        private void RegisterCallbacks(string gameEndText)
        {
            Label gameEndLabel = _root.Q<Label>(_labelName);
            gameEndLabel.text = gameEndText;
            Button exitBtn = _root.Q<Button>(_exitBtnName);
            exitBtn.RegisterCallback<ClickEvent>(OnExitBtnClick);
            Button restartBtn = _root.Q<Button>(_restartBtnName);
            restartBtn.RegisterCallback<ClickEvent>(OnRestartBtnClick);
        }

        private void OnRestartBtnClick(ClickEvent evt)
        {
            SceneManager.LoadScene(Helper.GameSceneName);
        }

        private void OnExitBtnClick(ClickEvent evt)
        {
            SceneManager.LoadScene(Helper.MainMenuSceneName);
        }
    }
}
