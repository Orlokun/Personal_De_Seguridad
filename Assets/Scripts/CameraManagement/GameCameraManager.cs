using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;

namespace CameraManagement
{
    public enum OfficeCameraStates
    {
        GeneralDesktop = 0,
        Notebook = 1,
        Phone = 2,
    }
    
    public enum GameCameraState
    {
        Office,
        Level,
        IsometricFollow,
        InDialogue
    }
    
    public class GameCameraManager : MonoBehaviour, IGameCameraManager
    {
        private static GameCameraManager _instance;
        public static GameCameraManager Instance => _instance;
        /// <summary>
        /// Parents for camera objects
        /// </summary>
        [SerializeField] private Transform levelCamerasParent;
        [SerializeField] private Transform officeCamerasParent;
        [SerializeField] private Transform dialogueCamerasParent;
        
        private Transform isometricFollowTarget;
        //Currently active Cameras
        private Dictionary<int, GameObject> _mActiveCameras;
        private Tuple<int, GameCameraState> _mLastCameraState;
        private GameCameraState _currentCameraState = GameCameraState.Level;
        private bool _mInitialized = false;

        #region Public Functions
        public void SetDialogueFollowObjects(Transform targetsInDialogue)
        {
            var dialogueFollowCamera = _mActiveCameras[0];
            CinemachineVirtualCamera dialogueCamera;

            if (!dialogueFollowCamera.TryGetComponent(out dialogueCamera))
            {
                return;
            }
            dialogueCamera.Follow = targetsInDialogue;
            dialogueCamera.LookAt = targetsInDialogue;
        }

        public void ReturnToLastState()
        {
            var lastState = _mLastCameraState;
            ChangeCameraState(lastState.Item2);
            ActivateNewCamera(lastState.Item2, lastState.Item1);
        }
        public void ActivateNewCamera(GameCameraState requestState, int indexCamera)
        {
            if(!IsValidRequest(requestState,indexCamera))
            {
                return;
            }
        
            for (var i = 0; i < _mActiveCameras.Count; i++)
            {
                var isSelectedCamera = i == indexCamera;
                _mActiveCameras[i].SetActive(isSelectedCamera);
                //Do not save Dialogue state in order to return from it later
                if (isSelectedCamera && _currentCameraState != GameCameraState.InDialogue)
                {
                    _mLastCameraState = new Tuple<int, GameCameraState>(i, _currentCameraState);
                }
            }
        }
        public bool IsCameraManagerReady()
        {
            return _mInitialized;
        }
        public int MaxNumberOfCameras()
        {
            return _mActiveCameras.Count -1;
        }
        public GameCameraState ActiveState()
        {
            return _currentCameraState;
        }
        public void ChangeCameraState(GameCameraState newCameraState)   
        {
            if (_currentCameraState == newCameraState)
            {
                return;
            }
            if (newCameraState != GameCameraState.InDialogue)
            {
                _mLastCameraState = GetCurrentCameraState();
            }
            TurnAnyActiveCameraOff();
            _currentCameraState = newCameraState;
            PopulateCameras();
        }
        public Tuple<int, GameCameraState> GetCurrentCameraState()
        {
            var cameraIndex = _mActiveCameras.SingleOrDefault(x => x.Value.activeInHierarchy).Key;
            var currentCameraState = _currentCameraState;
            return new Tuple<int, GameCameraState>(cameraIndex, currentCameraState);
        }
        #endregion

        #region Initialization
        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(this);
            }
            _instance = this;
            _currentCameraState = GameCameraState.Level;
            TurnCameraObjectsOff();
            PopulateCameras();
            _mInitialized = true;
        }
    
        #region Camera Objects Management
        private void TurnAnyActiveCameraOff()
        {
            foreach (var activeCamera in _mActiveCameras)
            {
                if (activeCamera.Value.activeInHierarchy)
                {
                    activeCamera.Value.SetActive(false);
                }
            }
        }
        private void TurnCameraObjectsOff()
        {
            //Deactivate Level Camera Game Objects
            for (var i = 0; i < levelCamerasParent.childCount;i++)
            {
                levelCamerasParent.GetChild(i).gameObject.SetActive(false);
            }
            //Deactivate Office Camera Game Objects
            for (var i = 0; i < officeCamerasParent.childCount;i++)
            {
                officeCamerasParent.GetChild(i).gameObject.SetActive(false);
            }
            //Deactivate Dialogue Camera Game Objects
            for (var i = 0; i < dialogueCamerasParent.childCount;i++)
            {
                dialogueCamerasParent.GetChild(i).gameObject.SetActive(false);
            }
        }
        private void PopulateCameras()
        {
            switch (_currentCameraState)
            {
                case GameCameraState.Level:
                    RepopulateActiveCamerasDictionary(levelCamerasParent);
                    break;
                case GameCameraState.Office:
                    RepopulateActiveCamerasDictionary(officeCamerasParent);
                    break;
                case GameCameraState.IsometricFollow:
                    Debug.LogWarning("[PopulateCameras] Isometric Follow Cameras must be handled");
                    break;
                case GameCameraState.InDialogue:
                    RepopulateActiveCamerasDictionary(dialogueCamerasParent);
                    break;
            }
        }
        #endregion

        private void RepopulateActiveCamerasDictionary(Transform camerasParent)
        {
            _mActiveCameras = new Dictionary<int, GameObject>();
            for (var i = 0; i < camerasParent.childCount; i++)
            {
                _mActiveCameras.Add(i, camerasParent.GetChild(i).gameObject);    
            }
        }
        
        #endregion
        private bool IsValidRequest(GameCameraState requestState,int indexCamera)
        {
            if (requestState != _currentCameraState)
            {
                Debug.LogWarning("[GameCameraManager.IsValidRequest] Request state and current camera state do not match");
                return false;
            }
            if (_mActiveCameras == null || _mActiveCameras.Count == 0)
            {
                Debug.LogWarning("[GameCameraManager.IsValidRequest] No Cameras In Scene");
                return false;
            }
            if (!_mActiveCameras.ContainsKey(indexCamera))
            {
                Debug.LogWarning("[GameCameraManager.IsValidRequest] Camera must be added in _mActiveCameras first");
                return false;
            }
            return true;
        }
    }
}