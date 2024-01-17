using GameDirection;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.CustomerInterfaces;
using UnityEngine;

namespace GamePlayManagement.LevelManagement
{
    public class EdenLevelManagement : GameMapLevelManagement
    {
        [SerializeField] private Transform levelCamerasParent;
        private ICustomersInSceneManager _customerSpawner;
        private IPlayerGameProfile _mPlayerProfile;

        protected override void Awake()
        {
            base.Awake();
            _mGameCameraManager.SetLevelCamerasParent(levelCamerasParent);
            SetCamerasInactive();
            _customerSpawner = FindObjectOfType<CustomersInSceneManager>(true);
            _customerSpawner.ToggleSpawning(false);
            _mPlayerProfile = GameDirector.Instance.GetActiveGameProfile;
        }

        protected override void ManageClientSpawning(bool isActive)
        {
            if (_customerSpawner == null)
            {
                Debug.LogWarning("[EdenLevelManager.ManageClientSpawning] Customer Spawner must be available for toggle");
                return;
            }
            _customerSpawner.ToggleSpawning(isActive);
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