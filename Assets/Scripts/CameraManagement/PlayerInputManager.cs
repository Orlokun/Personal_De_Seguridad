using GameDirection;
using InputManagement;
using UI;
using UnityEngine;

namespace CameraManagement
{
    /// <summary>
    /// MonoBehaviour class in charge of reading input
    /// </summary>
    public class PlayerInputManager : MonoBehaviour, IPlayerInputManager
    {
        private static IPlayerInputManager mInstance;
        public static IPlayerInputManager Instance => mInstance;
        
        private IGeneralGameInputManager _mGameInputManager;
        private IHighLvlGameStateManager _highLvlGameState;
        private IGameCameraOperator _mGameCameraManager;
        private IUIController _mUIController;
        
        
        private int _currentCameraIndex;

        private void Awake()
        {
            CheckSingletonInstance();
            DontDestroyOnLoad(this);
        }

        private void CheckSingletonInstance()
        {
            if (mInstance == null)
            {
                mInstance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        private void Start()
        {
            _mGameCameraManager = GameCameraOperator.Instance;
            _mGameInputManager = IGeneralGameGameInputManager.Instance;
            _highLvlGameState = GameDirector.Instance;
            _mUIController = UIController.Instance;
            IGeneralGameGameInputManager.Instance.OnGameStateChange += OnGameStateChange;
        }
        private void Update()
        {
            ManagePauseInput();
            if (!inputEnabled)
            {
                return;
            }
            
            //Camera Input Section
            ManagePerspectiveGameplayInput();
            var hasLrInput = ProcessLeftRightInput();
            if(!hasLrInput)
            {
                hasLrInput = CheckNumberInput();
            }
            if (hasLrInput)
            {
                _mGameCameraManager.ActivateCameraIndex(_currentCameraIndex);
            }
            //End Camera Input Section
        }

        private void ManagePerspectiveGameplayInput()
        {
            if (!Input.GetKeyDown(KeyCode.Tab) || _mGameInputManager.CurrentInputGameState == InputGameState.Pause || !_mGameCameraManager.AreLevelCamerasActive)
            {
                return;
            }
            ToggleCameraPerspective();
        }

        private void ToggleCameraPerspective()
        {
            var newState = _mGameCameraManager.ActiveState() == GameCameraState.Office
                ? GameCameraState.Level
                : GameCameraState.Office;

            SetCameraPerspectiveState(newState);
        }
        
        public void SetNewCameraState(GameCameraState newState)
        {
            SetCameraPerspectiveState(newState);
        }
        private void SetCameraPerspectiveState(GameCameraState newState)
        {
            Debug.Log($"Setting new camera state {newState}");
            _mGameCameraManager.ChangeCameraState(newState);
            _mGameCameraManager.ActivateCameraWithIndex(newState, 0);
            if (_mGameInputManager.CurrentInputGameState != InputGameState.InDialogue)
            {
                _mUIController.ReturnToBaseGamePlayCanvasState();
            }
            UIController.Instance.SyncUIStatusWithCameraState(newState, 0);

        }
        private void ManagePauseInput()
        {
            if (!Input.GetKeyDown(KeyCode.Escape))
            {
                return;
            }
            Debug.Log("Starting Pause Process");
            var currentGameState = _mGameInputManager.CurrentInputGameState;
            var isCurrentPause = currentGameState == InputGameState.Pause;
                
            var newGameState = isCurrentPause ? _mGameInputManager.LastInputGameState : InputGameState.Pause;
            Debug.Log($"Pausing game == {newGameState == InputGameState.Pause}");

            IGeneralGameGameInputManager.Instance.SetGamePlayState(newGameState);
        }
        
        private bool ProcessLeftRightInput()
        {
            var isCameraInput = false;
            
            if (Input.GetKeyDown(KeyCode.Q))
            {
                AddCameraIndex();
                isCameraInput = true;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                ReduceCameraIndex();
                isCameraInput = true;
            }
            return isCameraInput;
        }
        private void AddCameraIndex()
        {
            if (_currentCameraIndex == _mGameCameraManager.MaxNumberOfCameras())
            {
                _currentCameraIndex = 0;
            }
            else
            {
                _currentCameraIndex++;
            }
        }
        private void ReduceCameraIndex()
        {
            if (_currentCameraIndex == 0)
            {
                _currentCameraIndex = _mGameCameraManager.MaxNumberOfCameras();
            }
            else
            {
                _currentCameraIndex--;
            }
        }

        private bool inputEnabled = true;
        private void OnGameStateChange(InputGameState newGameState)
        {
            inputEnabled = newGameState == InputGameState.InGame;
        }

        private bool CheckNumberInput()
        {
            switch (_mGameCameraManager.ActiveState())
            {
                case GameCameraState.Level:
                    return GetLevelCameraInputNumber();
                case GameCameraState.Office:
                    return GetOfficeCameraInputNumber();
                case GameCameraState.IsometricFollow:
                    return false;
            }
            return false;
        }
        
        //TODO: IMPROVE THE WAY INPUTS ARE SETUP
        private bool GetOfficeCameraInputNumber()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _currentCameraIndex = 0;
                return true;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _currentCameraIndex = 1;
                return true;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                _currentCameraIndex = 2;
                return true;
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                _currentCameraIndex = 3;
                return true;
            }
            return false;
        }

        private bool GetLevelCameraInputNumber()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _currentCameraIndex = _mGameCameraManager.MaxNumberOfCameras();
                return true;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _currentCameraIndex = _mGameCameraManager.MaxNumberOfCameras() - 1;
                return true;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                _currentCameraIndex = _mGameCameraManager.MaxNumberOfCameras() - 2;
                return true;
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                _currentCameraIndex = _mGameCameraManager.MaxNumberOfCameras() - 3;
                return true;
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                _currentCameraIndex = _mGameCameraManager.MaxNumberOfCameras() - 4;
                return true;
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                _currentCameraIndex = _mGameCameraManager.MaxNumberOfCameras() - 5;
                return true;
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                _currentCameraIndex = _mGameCameraManager.MaxNumberOfCameras() - 6;
                return true;
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                _currentCameraIndex = _mGameCameraManager.MaxNumberOfCameras() - 7;
                return true;
            }
            return false;
        }

        private void OnDestroy()
        {
            IGeneralGameGameInputManager.Instance.OnGameStateChange -= OnGameStateChange;
        }

        public void CoordinateInputIndex(int incomingIndex)
        {
            _currentCameraIndex = incomingIndex;
        }
    }
}
