using InputManagement;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PausePanel : MonoBehaviour
    {
        [SerializeField] private Button returnButton;
        [SerializeField] private Button exitButton;

        private void Start()
        {
            returnButton.onClick.AddListener(ReturnToGame);
            exitButton.onClick.AddListener(ExitGame);
        }

        private void ExitGame()
        {
            Application.Quit();
        }

        private void ReturnToGame()
        {
            var lastState = GeneralInputStateManager.Instance.LastInputGameState;
            GeneralInputStateManager.Instance.SetGamePlayState(lastState);
        }
    }
}
