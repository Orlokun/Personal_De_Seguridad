using System.Collections;
using CameraManagement;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement.BitDescriptions.Suppliers;
using UnityEngine;

namespace GamePlayManagement.LevelManagement
{
    public abstract class GameMapLevelManagement : MonoBehaviour
    {
        protected IGameCameraOperator _mGameCameraManager;
        protected IClockManagement _mClockManager;
        
        protected JobSupplierBitId MJobSupplierId;
        protected virtual void Awake()
        {
            _mGameCameraManager = GameCameraOperator.Instance;
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
}