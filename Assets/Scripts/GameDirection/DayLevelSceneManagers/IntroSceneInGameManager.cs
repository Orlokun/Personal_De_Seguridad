using System.Collections.Generic;
using UnityEngine;

namespace GameDirection.DayLevelSceneManagers
{
    public class IntroSceneInGameManager : MonoBehaviour, IIntroSceneInGameManager
    {
        [SerializeField] private Camera mCeoCamera;
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
            mCeoCamera.gameObject.SetActive(state);
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
        
        public Transform GetCamerasInOfficeParent => mCamerasInLevelParent;
    }
    

    public interface IIntroSceneInGameManager
    {
        
        public void ToggleBeacon(bool state);
        public void ToggleCeoCameras(bool state);
        public void ToggleIntroSceneLevelObjects(bool state);
        public void ToggleCompleteScene(bool state);
        public Transform GetCamerasItemsParent { get; }
        public Transform GetCamerasInLevelParent { get; }
        public Transform GetCamerasInOfficeParent { get; }
    }
}