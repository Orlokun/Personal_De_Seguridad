using CameraManagement;
using GameDirection;
using UI;
using UnityEngine;

namespace InputManagement.Keyboard
{
    public class GamePerspectiveManagement : MonoBehaviour
    {
        private IGeneralInputStateManager _mInputStateManager;
        private IHighLvlGameStateManager _highLvlGameState;
        private IGameCameraManager _mGameCameraManager;
        private IUIController _mUIController;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            _mGameCameraManager = GameCameraManager.Instance;
            _mInputStateManager = GeneralInputStateManager.Instance;
            _highLvlGameState = GameDirector.Instance;
            _mUIController = UIController.Instance;
        }
        private void Update()
        {
            ManagePauseInput();
            ManagePerspectiveGameplayInput();
        }
        
        private void ManagePerspectiveGameplayInput()
        {
            if (!Input.GetKeyDown(KeyCode.Tab) || _mInputStateManager.CurrentInputGameState != InputGameState.InGame)
            {
                return;
            }
            
            var newState = _mGameCameraManager.ActiveState() == GameCameraState.Office
                ? GameCameraState.Level
                : GameCameraState.Office;

            _mGameCameraManager.ChangeCameraState(newState);
            _mGameCameraManager.ActivateNewCamera(newState, 0);
            if (_mInputStateManager.CurrentInputGameState != InputGameState.InDialogue)
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
            var currentGameState = _mInputStateManager.CurrentInputGameState;
            var isCurrentPause = currentGameState == InputGameState.Pause;
                
            var newGameState = isCurrentPause ? _mInputStateManager.LastInputGameState : InputGameState.Pause;
            Debug.Log($"Pausing game == {newGameState == InputGameState.Pause}");

            GeneralInputStateManager.Instance.SetGamePlayState(newGameState);
        }
    }
}
