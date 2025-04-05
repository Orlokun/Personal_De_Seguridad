using System;
using System.Collections.Generic;
using GamePlayManagement.ItemManagement.Guards;
using GamePlayManagement.Players_NPC;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.AttitudeStates;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.AttitudeStates.BaseCharacter;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.MovementStates;
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
        
        [SerializeField] private GameObject introLight1;
        [SerializeField] private GameObject introLight2;

        [SerializeField] private GameObject mIntroShooterCharacter;
        [SerializeField] private Transform _mStartPositionOne;
        [SerializeField] private Transform _mStartPositionTwo;
        [SerializeField] private Transform _mStartPositionThree;
        [SerializeField] private Transform _mEndPositionOne;
        [SerializeField] private Transform _mEndPositionTwo;
        [SerializeField] private Transform _mEndPositionThree;

        private void Awake()
        {
            mIntroShooterCharacter.SetActive(false);
        }

        public void InstantiateCharacterFirstSection()
        {
            mIntroShooterCharacter.transform.position = _mStartPositionOne.position;

            var guardObject = FindFirstObjectByType<BaseGuardGameObject>(FindObjectsInactive.Exclude);
            if(!guardObject)
            {
                Debug.LogError("No guard in scene");
                return;
            }
            mIntroShooterCharacter.SetActive(true);
            var characterController = mIntroShooterCharacter.GetComponent<IBaseCharacterInScene>();
            characterController.SetMovementDestination(guardObject.transform.position);
            characterController.ChangeMovementState<WalkingState>();
            characterController.ChangeAttitudeState<WalkingTowardsPositionState>();

            characterController.WalkingDestinationReached += ShootGuardCommand;
        }
        
        public void ToggleSceneCharacter(bool state)
        {
            mIntroShooterCharacter.SetActive(state);
        }
        
        private void ShootGuardCommand()
        {
            var characterController = mIntroShooterCharacter.GetComponent<IBaseCharacterInScene>();
            characterController.WalkingDestinationReached -= ShootGuardCommand;
            characterController.ChangeAttitudeState<ShootTargetState>();
        }

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

        public void ToggleIntroSceneLights(bool state)
        {
            introLight1.SetActive(state);
            introLight2.SetActive(state);
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
        public void ShootGuard()
        {
            var guardInScene = FindFirstObjectByType<BaseGuardGameObject>();
            if(guardInScene is null)
            {
                Debug.LogError("No guard in scene");
                return;
            }
            guardInScene.ChangeAttitudeState<DeathShotState>();
        }


    }
    

    public interface IIntroSceneInGameManager
    {
        
        public void ToggleBeacon(bool state);
        public void ToggleCeoCameras(bool state);
        public void ToggleGuardPlacementCameras(bool state);
        public void ToggleCameraPlacementCameras(bool state);
        public void ToggleIntroSceneLevelObjects(bool state);
        public void ToggleIntroSceneLights(bool state);
        public void ToggleSceneCharacter(bool state);

        public void ToggleCompleteScene(bool state);
        public Transform GetCamerasItemsParent { get; }
        public Transform GetCamerasInLevelParent { get; }
        public Transform GetCamerasInOfficeParent { get; }
        void ShootGuard();
        void InstantiateCharacterFirstSection();
    }
}