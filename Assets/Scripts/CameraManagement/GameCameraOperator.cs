using System;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace CameraManagement
{
    public enum OfficeCameraStates
    {
        GeneralDesktop = 0,
        Tablet = 1,
        Phone = 2,
        Cigar = 3
    }
    
    public enum GameCameraState
    {
        Office,
        Level,
        IsometricFollow,
        InDialogue,
        MainMenu
    }
    
    /// <summary>
    /// MonoBehavior Class in charge of managing the Camera Objects in the scene
    /// </summary>
    public class GameCameraOperator : MonoBehaviour, IGameCameraOperator
    {
        private static GameCameraOperator instance;
        public static IGameCameraOperator Instance => instance;
        
        
        /// <summary>
        /// Parents for camera objects
        /// </summary>
        private Transform _mLevelCamerasParent;
        private Transform _officeCamerasParent;
        
        [SerializeField] private Transform dialogueCamerasParent;


        private Dictionary<int, GameObject> _mOfficeCameras;
        private Dictionary<int, GameObject> _mLevelCameras;
        //Currently active Cameras
        
        
        private Tuple<int, GameCameraState> _mLastCameraState;
        private GameCameraState _mCurrentCameraState = GameCameraState.MainMenu;
        private int _mCurrentCameraIndex;
        private bool _mInitialized = false;
        
        
        
        #region Public Functions
        /*public void SetDialogueFollowObjects(Transform targetsInDialogue)
        {
            var dialogueFollowCamera = _mActiveCameras[0];
            CinemachineCamera dialogueCamera;

            if (!dialogueFollowCamera.TryGetComponent(out dialogueCamera))
            {
                return;
            }
            dialogueCamera.Follow = targetsInDialogue;
            dialogueCamera.LookAt = targetsInDialogue;
        }*/
        public void SetLevelCamerasParent(Transform camerasParentObject)
        {
            _mLevelCamerasParent = camerasParentObject;
            PopulateCameras(GameCameraState.Level);
        }
        public void SetOfficeCamerasParent(Transform camerasParentObject)
        {
            _officeCamerasParent = camerasParentObject;
            PopulateCameras(GameCameraState.Office);
        }
        
        public void ReturnToLastState()
        {
            var lastState = _mLastCameraState;
            if (lastState == null)
            {
                return;
            }
            
            ChangeCameraState(lastState.Item2);
            ActivateNewCamera(lastState.Item2, lastState.Item1);
        }
        public void ActivateNewCamera(GameCameraState requestState, int indexCamera)
        {
            if(!IsValidRequest(requestState,indexCamera))
            {
                return;
            }

            switch (requestState)
            {
                case GameCameraState.Level:
                    ActivateLevelCamera(indexCamera);
                    break;
                case GameCameraState.Office:
                    ActivateOfficeCamera(indexCamera);
                    break;
            }
        }

        private void ActivateLevelCamera(int indexCamera)
        {
            for (var i = 0; i < _mLevelCameras.Count; i++)
            {
                var isSelectedCamera = i == indexCamera;
                _mLevelCameras[i].SetActive(isSelectedCamera);
                //Save current camera index for state record
                if (isSelectedCamera)
                {
                    _mCurrentCameraIndex = i;
                }
                //Do not save Dialogue state in order to return from it later
                if (isSelectedCamera && _mCurrentCameraState != GameCameraState.InDialogue)
                { 
                    _mLastCameraState = new Tuple<int, GameCameraState>(i, _mCurrentCameraState);
                }
            }        
        }
        private void ActivateOfficeCamera(int indexCamera)
        {
            for (var i = 0; i < _mOfficeCameras.Count; i++)
            {
                var isSelectedCamera = i == indexCamera;
                _mOfficeCameras[i].SetActive(isSelectedCamera);
                //Save current camera index for state record
                if (isSelectedCamera)
                {
                    _mCurrentCameraIndex = i;
                }
                //Do not save Dialogue state in order to return from it later
                if (isSelectedCamera && _mCurrentCameraState != GameCameraState.InDialogue)
                { 
                    _mLastCameraState = new Tuple<int, GameCameraState>(i, _mCurrentCameraState);
                }
            }        
        }
        

        public void ActivateCameraIndex(int indexCamera)
        {
            ActivateNewCamera(_mCurrentCameraState, indexCamera);
            UIController.Instance.SyncUIStatusWithCameraState(_mCurrentCameraState, indexCamera);
        }

        public bool IsCameraManagerReady()
        {
            return _mInitialized;
        }
        public int MaxNumberOfCameras()
        {
            switch (_mCurrentCameraState)
            {
                case GameCameraState.Level:
                    return _mLevelCameras.Count -1;
                case GameCameraState.Office:
                    return _mOfficeCameras.Count -1;    
                default:
                    Debug.LogError("Camera State not found. MaxNumberOfCameras must return a valid value. Returning 0");
                    return 0;
            }
        }
        public GameCameraState ActiveState()
        {
            return _mCurrentCameraState;
        }
        public void ChangeCameraState(GameCameraState newCameraState)   
        {
            if (_mCurrentCameraState == newCameraState)
            {
                return;
            }
            SaveLastState(newCameraState);
            TurnAnyActiveCameraOff();
            _mCurrentCameraState = newCameraState;
        }
        
        private void SaveLastState(GameCameraState newCameraState)
        {
            if (newCameraState != GameCameraState.InDialogue && _mCurrentCameraState != GameCameraState.MainMenu)
            {
                _mLastCameraState = GetCurrentCameraState();
            }
        }
        public Tuple<int, GameCameraState> GetCurrentCameraState()
        {
            try
            {
                return new Tuple<int, GameCameraState>(_mCurrentCameraIndex, _mCurrentCameraState);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion

        
        
        
        #region Initialization
        private void Awake()
        {
            if (instance != null)
            {
                Destroy(this);
            }
            DontDestroyOnLoad(this);
            instance = this;
            _mCurrentCameraState = GameCameraState.MainMenu;
            _mCurrentCameraIndex = 0;
            _mInitialized = true;
        }
    
        #region Camera Objects Management
        private void TurnAnyActiveCameraOff()
        {
            if (_mLevelCameras == null || _mLevelCameras.Count == 0 || _mOfficeCameras == null || _mOfficeCameras.Count == 0)
            {
                return;
            }
            foreach (var activeCamera in _mLevelCameras)
            {
                if (activeCamera.Value.activeInHierarchy)
                {
                    activeCamera.Value.SetActive(false);
                }
            }
            foreach (var officeCamera in _mOfficeCameras)
            {
                if (officeCamera.Value.activeInHierarchy)
                {
                    officeCamera.Value.SetActive(false);
                }
            }
        }

        private void TurnLevelCameraParentsOff()
        {
            //Deactivate Level Camera Game Objects
            for (var i = 0; i < _mLevelCamerasParent.childCount;i++)
            {
                _mLevelCamerasParent.GetChild(i).gameObject.SetActive(false);
            }
        }
        private void TurnOfficeCameraParentsOff()
        {
            //Deactivate Office Camera Game Objects
            for (var i = 0; i < _officeCamerasParent.childCount;i++)
            {
                _officeCamerasParent.GetChild(i).gameObject.SetActive(false);
            }
        }
        
        private void TurnDialogueCameraParentsOff()
        {
            //Deactivate Dialogue Camera Game Objects
            for (var i = 0; i < dialogueCamerasParent.childCount;i++)
            {
                dialogueCamerasParent.GetChild(i).gameObject.SetActive(false);
            }
        }
        private void TurnCameraObjectsOff()
        {
            TurnLevelCameraParentsOff();
            TurnOfficeCameraParentsOff();
            TurnDialogueCameraParentsOff();
        }
        private void PopulateCameras(GameCameraState incomingCameras)
        {
            switch (incomingCameras)
            {
                case GameCameraState.Level:
                    PopulateLevelCameraData(_mLevelCamerasParent);
                    break;
                case GameCameraState.Office:
                    PopulateOfficeCamerasData(_officeCamerasParent);
                    break;
            }
        }

        private void PopulateOfficeCamerasData(Transform officeCamerasParent)
        {
            if (_mOfficeCameras == null)
            {
                _mOfficeCameras = new Dictionary<int, GameObject>();
            }
            for (var i = 0; i < officeCamerasParent.childCount; i++)
            {
                var cameraObject = officeCamerasParent.GetChild(i).gameObject;
                _mOfficeCameras.Add(i, cameraObject);
            }        
        }

        private void PopulateLevelCameraData(Transform levelCamerasParent)
        {
            if (_mLevelCameras == null)
            {
                _mLevelCameras = new Dictionary<int, GameObject>();
            }
            for (var i = 0; i < levelCamerasParent.childCount; i++)
            {
                var cameraObject = levelCamerasParent.GetChild(i).gameObject;
                _mLevelCameras.Add(i, cameraObject);
            }
        }

        public void ClearLevelCameras()
        {
            _mLevelCamerasParent = null;
            _mLevelCameras.Clear();
        }

        #endregion

        #endregion
        private bool IsValidRequest(GameCameraState requestState,int indexCamera)
        {
            switch (requestState)
            {
                case GameCameraState.Level:
                    return IsValidLevelRequest(indexCamera);
                case GameCameraState.Office:
                    return IsValidOfficeRequest(indexCamera);
            }

            return true;
        }

        private bool IsValidOfficeRequest(int indexCamera)
        {
            if (_mOfficeCameras == null || _mOfficeCameras.Count == 0)
            {
                Debug.LogWarning("[GameCameraManager.IsValidRequest] No Cameras In Scene");
                return false;
            }
            if (!_mOfficeCameras.ContainsKey(indexCamera))
            {
                Debug.LogWarning("[GameCameraManager.IsValidRequest] Camera must be added in _mActiveCameras first");
                return false;
            }
            return true;
        }
        private bool IsValidLevelRequest(int indexCamera)
        {
            if (_mLevelCameras == null || _mLevelCameras.Count == 0)
            {
                Debug.LogWarning("[GameCameraManager.IsValidRequest] No Cameras In Scene");
                return false;
            }
            if (!_mLevelCameras.ContainsKey(indexCamera))
            {
                Debug.LogWarning("[GameCameraManager.IsValidRequest] Camera must be added in _mActiveCameras first");
                return false;
            }
            return true;
        }
    }
}