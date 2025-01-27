using GameDirection;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ContinueGameButton : MonoBehaviour
    {
        private Button _mContinueButton;

        private void Awake()
        {
            _mContinueButton = GetComponent<Button>();
            _mContinueButton.onClick.AddListener(OnContinueButtonClicked);
        }

        private void OnContinueButtonClicked()
        {
            GameDirector.Instance.ContinueGame();
        }
    }
}
