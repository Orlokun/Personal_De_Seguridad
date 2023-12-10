using UnityEngine;

namespace GamePlayManagement.LevelManagement
{
    public class EdenLevelManagement : GameMapLevelManagement
    {
        [SerializeField] private Transform levelCamerasParent;

        protected override void Awake()
        {
            base.Awake();
            _mGameCameraManager.SetLevelCamerasParent(levelCamerasParent);
            SetCamerasInactive();
        }

        private void SetCamerasInactive()
        {
            foreach (Transform cameraObject in levelCamerasParent)
            {
                cameraObject.gameObject.SetActive(false);
            }
        }
    }
}