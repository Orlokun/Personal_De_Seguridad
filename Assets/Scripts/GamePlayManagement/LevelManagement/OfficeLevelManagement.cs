using System;
using CameraManagement;
using UnityEngine;

namespace GameManagement.LevelManagement
{
    public class GameMapLevelManagement : MonoBehaviour
    {
        protected IGameCameraManager _mGameCameraManager;
        private void Awake()
        {
            _mGameCameraManager = GameCameraManager.Instance;
        }
    }
    
    public class OfficeLevelManagement : GameMapLevelManagement
    {
        [SerializeField] private Transform levelCamerasParent;

        private void Start()
        {
            _mGameCameraManager.SetOfficeCamerasParent(levelCamerasParent);
            _mGameCameraManager.ChangeCameraState(GameCameraState.Office);
            _mGameCameraManager.ActivateNewCamera(GameCameraState.Office, (int)OfficeCameraStates.GeneralDesktop);
        }
    }
}