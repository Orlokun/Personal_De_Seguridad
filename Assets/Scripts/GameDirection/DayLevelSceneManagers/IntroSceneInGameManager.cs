using System.Collections.Generic;
using UnityEngine;

namespace GameDirection.DayLevelSceneManagers
{
    public class IntroSceneInGameManager : MonoBehaviour, IIntroSceneInGameManager
    {
        [SerializeField] private Camera mCeoCamera;
        [SerializeField] private Camera mGuardPlacementZoneCam;
        [SerializeField] private Camera mCameraPlacementZoneCam;
        
        
        [SerializeField] private BeaconOperator mBeaconOperator;
        [SerializeField] private List<GameObject> mScreenObjects;
        [SerializeField] private GameObject mIntroScene;
    
        [SerializeField] private Transform mCamerasItemsParent;
        [SerializeField] private Transform mCamerasInLevelParent;
        [SerializeField] private Transform mCamerasInOfficeParent;
        
        
        public void ToggleBeacon(bool state)
        {
            mBeaconOperator.gameObject.SetActive(state);
        }

        public void ToggleCeoCameras(bool state)
        {
            if (state)
            {
                mGuardPlacementZoneCam.gameObject.SetActive(false);
                mCameraPlacementZoneCam.gameObject.SetActive(false);
            }

            mCeoCamera.gameObject.SetActive(state);
            foreach (var screenObject in mScreenObjects)
            {
                screenObject.SetActive(state);
            }
        }
        
        public void ToggleGuardPlacementCameras(bool state)
        {
            if(state)
            {
                mCeoCamera.gameObject.SetActive(false);
                mCameraPlacementZoneCam.gameObject.SetActive(false);
            }
            mGuardPlacementZoneCam.gameObject.SetActive(state);
            foreach (var screenObject in mScreenObjects)
            {
                screenObject.SetActive(state);
            }
        }
        
        public void ToggleCameraPlacementCameras(bool state)
        {
            if (state)
            {
                mGuardPlacementZoneCam.gameObject.SetActive(false);
                mCeoCamera.gameObject.SetActive(false);
            }

            
            mCameraPlacementZoneCam.gameObject.SetActive(state);
            foreach (var screenObject in mScreenObjects)
            {
                screenObject.SetActive(state);
            }
        }

        public void ToggleIntroSceneLevelObjects(bool state)
        {
            mIntroScene.SetActive(state);
        }

        public void ToggleCompleteScene(bool state)
        {
            ToggleIntroSceneLevelObjects(state);
            ToggleBeacon(state);
            ToggleCeoCameras(state);
        }

        public Transform GetCamerasItemsParent => mCamerasItemsParent;
        public Transform GetCamerasInLevelParent => mCamerasInLevelParent;
        
        public Transform GetCamerasInOfficeParent => mCamerasInOfficeParent;
    }
    

    public interface IIntroSceneInGameManager
    {
        
        public void ToggleBeacon(bool state);
        public void ToggleCeoCameras(bool state);
        public void ToggleGuardPlacementCameras(bool state);
        public void ToggleCameraPlacementCameras(bool state);
        public void ToggleIntroSceneLevelObjects(bool state);
        public void ToggleCompleteScene(bool state);
        public Transform GetCamerasItemsParent { get; }
        public Transform GetCamerasInLevelParent { get; }
        public Transform GetCamerasInOfficeParent { get; }
    }
}