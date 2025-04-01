using CameraManagement;
using UnityEngine;

namespace GamePlayManagement.LevelManagement
{
    public class OfficeLevelManagement : GameMapLevelManagement
    {
        [SerializeField] private Transform levelCamerasParent;

        private void Start()
        {
            _mGameCameraManager.SetOfficeCamerasParent(levelCamerasParent);
            _mGameCameraManager.ChangeCameraState(GameCameraState.Office);
            _mGameCameraManager.ActivateCameraWithIndex(GameCameraState.Office, (int)OfficeCameraStates.GeneralDesktop);
        }
    }
}