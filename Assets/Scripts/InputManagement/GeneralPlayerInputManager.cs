using CameraManagement;
using UI;
using UnityEngine;

namespace InputManagement
{
    public enum InputGameState
    {
        InGame,
        Pause,
        InDialogue
    }

    public class GeneralPlayerInputManager : MonoBehaviour
    {
        private IGeneralGameStateManager _mGameStateManager;
        private IGameCameraManager _mGameCameraManager;
        private IUIController _mUIController;
        private void Start()
        {
            _mGameCameraManager = GameCameraManager.Instance;
            _mGameStateManager = GeneralGamePlayStateManager.Instance;
            _mUIController = UIController.Instance;
        }
        private void Update()
        {
            ManagePauseInput();
            ManageGameplayStateInput();
        }
        private void ManageGameplayStateInput()
        {
            if (!Input.GetKeyDown(KeyCode.Tab) || _mGameStateManager.CurrentInputGameState != InputGameState.InGame)
            {
                return;
            }

            var newState = _mGameCameraManager.ActiveState() == GameCameraState.Office
                ? GameCameraState.Level
                : GameCameraState.Office;
            
            _mGameCameraManager.ChangeCameraState(newState);
            _mGameCameraManager.ActivateNewCamera(newState, 0);
            if (_mGameStateManager.CurrentInputGameState != InputGameState.InDialogue)
            {
                _mUIController.ReturnToBaseGamePlayCanvasState();
            }
        }
        private void ManagePauseInput()
        {
            if (!Input.GetKeyDown(KeyCode.Escape))
            {
                return;
            }
            Debug.Log("Starting Pause Process");
            var currentGameState = _mGameStateManager.CurrentInputGameState;
            var isCurrentPause = currentGameState == InputGameState.Pause;
                
            var newGameState = isCurrentPause ? _mGameStateManager.LastInputGameState : InputGameState.Pause;
            Debug.Log($"Pausing game == {newGameState == InputGameState.Pause}");

            GeneralGamePlayStateManager.Instance.SetGamePlayState(newGameState);
        }
    }
}
