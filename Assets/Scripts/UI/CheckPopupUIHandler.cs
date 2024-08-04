using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Chess
{
    public class CheckPopupUIHandler : MonoBehaviour
    {
        [SerializeField]
        private float _popupTime = 2f;
        [SerializeField]
        private float _fadeoutTime = 1.0f;
        private UIDocument _UICheckPopup;
        private VisualElement _root;

        private void Awake()
        {
            _UICheckPopup = GetComponent<UIDocument>();
        }

        public void ShowPopup()
        {
            _UICheckPopup.enabled = true;
            _root = _UICheckPopup.rootVisualElement;
            _root.style.opacity = 1f;
            StartCoroutine(FadeOut());
        }

        private IEnumerator FadeOut()
        {
            float opacity = 1f;
            yield return new WaitForSeconds(_popupTime);
            while (opacity > 0f)
            {
                _root.style.opacity = opacity;
                opacity -= _fadeoutTime * Time.deltaTime;
                yield return null;
            }
            _UICheckPopup.enabled = false;
        }
    }
}
