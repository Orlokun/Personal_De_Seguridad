using System.Collections;
using CameraManagement;
using GameDirection.TimeOfDayManagement;
using UnityEngine;

namespace GamePlayManagement.LevelManagement
{
    public abstract class GameMapLevelManagement : MonoBehaviour
    {
        protected IGameCameraManager _mGameCameraManager;
        protected IClockManagement _mClockManager;
        protected virtual void Awake()
        {
            _mGameCameraManager = GameCameraManager.Instance;
            _mClockManager = ClockManagement.Instance;
            _mClockManager.OnPassTimeOfDay += CheckOpenCloseHours;
        }

        protected void CheckOpenCloseHours(PartOfDay dayTime)
        {
            var isOpeningTime = dayTime == PartOfDay.Morning;
            var isClosingTime = dayTime == PartOfDay.EndOfDay;
            if (!isOpeningTime && !isClosingTime)
            {
                return;
            }
            ManageClientSpawning(isOpeningTime);
        }

        protected IEnumerator WaitAndDeactivate()
        {
            yield return new WaitForSeconds(15);
            ManageClientSpawning(false);
        }
        
        protected virtual void ManageClientSpawning(bool isActive)
        {

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