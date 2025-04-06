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
        public delegate void IntroSceneReadDestination(); 
        public event IntroSceneReadDestination WalkingDestinationReached;

        
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
        [SerializeField] private Transform mStartPositionOne;
        [SerializeField] private Transform mStartPositionTwo;
        [SerializeField] private Transform mHidePositionThree;
        [SerializeField] private Transform mStartPositionThree;
        [SerializeField] private Transform mEndPositionTwo;
        [SerializeField] private Transform mEndPositionThree;
        
        [SerializeField] private Transform mPlayerFinalTarget;

        
        
        private void Awake()
        {
            mIntroShooterCharacter.SetActive(false);
        }

        public void InstantiateCharacterFirstSection()
        {
            mIntroShooterCharacter.transform.position = mStartPositionOne.position;

            var guardObject = FindFirstObjectByType<BaseGuardGameObject>(FindObjectsInactive.Exclude);
            if(!guardObject)
            {
                Debug.LogError("No guard in scene");
                return;
            }
            mIntroShooterCharacter.SetActive(true);
            var characterController = mIntroShooterCharacter.GetComponent<IBaseCharacterInScene>();
            characterController.SetMovementDestination(guardObject.transform.position);
            characterController.ChangeMovementState<RunningState>();
            characterController.ChangeAttitudeState<WalkingTowardsPositionState>();

            characterController.WalkingDestinationReached += ShootTargetCommand;
        }

        public void InstantiateCharacterSecondSections()
        {
            mIntroShooterCharacter.transform.position = mStartPositionTwo.position;
            mIntroShooterCharacter.SetActive(true);
            var characterController = mIntroShooterCharacter.GetComponent<IBaseCharacterInScene>();
            characterController.SetMovementDestination(mEndPositionTwo.position);
            characterController.ChangeMovementState<RunningState>();
            characterController.ChangeAttitudeState<WalkingTowardsPositionState>();
            characterController.WalkingDestinationReached += ShootTargetCommand;
        }

        public void InstantiateCharacterThirdSections()
        {
            mIntroShooterCharacter.transform.position = mStartPositionThree.position;
            mIntroShooterCharacter.SetActive(true);
            var characterController = mIntroShooterCharacter.GetComponent<IBaseCharacterInScene>();
            characterController.SetRotateTowardsYOnly(mPlayerFinalTarget.position);
            characterController.SetMovementDestination(mPlayerFinalTarget.position);
            characterController.ChangeMovementState<WalkingState>();
            characterController.ChangeAttitudeState<WalkingTowardsPositionState>();
            characterController.WalkingDestinationReached += FinalDialogueSequence;
        }

        public void MoveCharacterToHidePosition()
        {
            mIntroShooterCharacter.transform.position = mHidePositionThree.position;
        }

        public void ToggleSceneCharacter(bool state)
        {
            mIntroShooterCharacter.SetActive(state);
        }
        
        public void ShootTargetCommand()
        {
            var characterController = mIntroShooterCharacter.GetComponent<IBaseCharacterInScene>();
            characterController.WalkingDestinationReached -= ShootTargetCommand;
            characterController.ChangeAttitudeState<ShootTargetState>();
        }

        private void FinalDialogueSequence()
        {
            var characterController = mIntroShooterCharacter.GetComponent<IBaseCharacterInScene>();
            characterController.ChangeAttitudeState<IdleAttitudeState>();
            WalkingDestinationReached?.Invoke();
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
        public void ShootTargetCommand();

        void InstantiateCharacterFirstSection();
        void InstantiateCharacterSecondSections();
        void InstantiateCharacterThirdSections();
        void MoveCharacterToHidePosition();
        
        public event IntroSceneInGameManager.IntroSceneReadDestination WalkingDestinationReached;
    }
}