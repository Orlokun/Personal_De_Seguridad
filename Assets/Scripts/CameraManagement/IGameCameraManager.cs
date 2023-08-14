using System;
using UnityEngine;

namespace CameraManagement
{
    public interface IGameCameraManager
    {
        public void ActivateNewCamera(GameCameraState requestState, int indexCamera);
        public int MaxNumberOfCameras();
        public GameCameraState ActiveState();
        public void ChangeCameraState(GameCameraState newCameraState);
        public void SetDialogueFollowObjects(Transform targetInDialogue);
        public void SetLevelCamerasParent(Transform camerasParentObject);
        public void SetOfficeCamerasParent(Transform camerasParentObject);
        public Tuple<int, GameCameraState> GetCurrentCameraState();
    }
}