using System;
using UnityEngine;

namespace CameraManagement
{
    public interface IGameCameraOperator
    {
        public void ActivateCameraWithIndex(GameCameraState requestState, int indexCamera);
        public void ActivateCameraWithTarget(GameCameraState requestState, string targetName);
        public void ActivateIsometricFollowCamera(GameCameraState requestState);
        public void ActivateCameraIndex(int indexCamera);
        public int MaxNumberOfCameras();
        public GameCameraState ActiveState();
        public void ChangeCameraState(GameCameraState newCameraState);
        public void SetLevelCamerasParent(Transform camerasParentObject);
        public void SetOfficeCamerasParent(Transform camerasParentObject);
        public void ReturnToLastState();
        public Tuple<int, GameCameraState> GetCurrentCameraState();
        public bool AreLevelCamerasActive { get; }
        void LoadMainOfficeCameras();
        void ClearLevelCameras();
    }
}