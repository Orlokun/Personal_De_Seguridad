using CameraManagement;
using UI;
using UnityEngine;

namespace InputManagement
{
    public class CameraViewInputManager : MonoBehaviour
    {
        private IGameCameraManager _mGameCameraManager;
        private IUIController _mUIController;
        private int _currentCameraIndex;
        private void Start()
        {
            GeneralInputStateManager.Instance.OnGameStateChange += OnGameStateChange;
            _mGameCameraManager = GameCameraManager.Instance;
            _mUIController = UIController.Instance;
            //_mGameCameraManager.ChangeCameraState(GameCameraState.Level);
            //_mGameCameraManager.ActivateNewCamera(GameCameraState.Level, _currentCameraIndex);
        }
    
        private void Update()
        {
            #region Camera Input
            if (GeneralInputStateManager.Instance.CurrentInputGameState == InputGameState.InGame)
            {
                var isCameraInput = false;
                
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    if (_currentCameraIndex == _mGameCameraManager.MaxNumberOfCameras())
                    {
                        _currentCameraIndex = 0;
                    }
                    else
                    {
                        _currentCameraIndex++;
                    }
                    isCameraInput = true;
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (_currentCameraIndex == 0)
                    {
                        _currentCameraIndex = _mGameCameraManager.MaxNumberOfCameras();
                    }
                    else
                    {
                        _currentCameraIndex--;
                    }
                    isCameraInput = true;
                }
                
                //Checks if Top keyboard numbers have been used
                if (!isCameraInput)
                {
                    isCameraInput = CheckNumberInput();
                }
                if (_currentCameraIndex > _mGameCameraManager.MaxNumberOfCameras())
                {
                    return;
                }
            
                if (isCameraInput)
                {
                    var currentCameraState = _mGameCameraManager.ActiveState();
                    if (currentCameraState == GameCameraState.Office)
                    {
                        _mUIController.UpdateOfficeUIElement(_currentCameraIndex);
                    }
                    _mGameCameraManager.ActivateNewCamera(currentCameraState ,_currentCameraIndex);
                }
            }
            #endregion
        }

        private void OnGameStateChange(InputGameState newGameState)
        {
            enabled = newGameState == InputGameState.InGame;
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
            GeneralInputStateManager.Instance.OnGameStateChange -= OnGameStateChange;
        }
    }
}