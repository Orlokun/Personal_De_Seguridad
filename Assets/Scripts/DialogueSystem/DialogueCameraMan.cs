using System.Linq;
using CameraManagement;
using DialogueSystem.Interfaces;
using DialogueSystem.Units;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueCameraMan : IDialogueCameraMan
    {
        public void ManageDialogueCameraBehavior(IDialogueObject currentDialogue, int dialogueLineIndex)
        {
            if (!currentDialogue.ContainsBehavior(DialogueBehaviors.DialogueWithCamera))
            {
                Debug.Log("[ManageDialogueCameraBehavior] No camera management in Dialogue Behaviors");
                return;
            }
            
            var dialogueWithCameraTargets = (IDialogueWithCameraTargets) currentDialogue;
            if (!dialogueWithCameraTargets.DialogueLineActivatesCamera(dialogueLineIndex + 1))
            {
                Debug.Log("[ManageDialogueCameraBehavior] Current Line does not contain camera behavior");
                return;
            }
            
            var targetInDialogue =
                dialogueWithCameraTargets.MyTargetsInDialogues.SingleOrDefault(x => x.Key == dialogueLineIndex + 1);
            if (targetInDialogue == null)
            {
                Debug.LogError("[ManageDialogueCameraBehavior] Dialogue Line with camera set must have a Target Transform assigned");
                return;
            }
            GameCameraManager.Instance.ChangeCameraState(GameCameraState.InDialogue);
            GameCameraManager.Instance.SetDialogueFollowObjects(targetInDialogue.Value);
            GameCameraManager.Instance.ActivateNewCamera(GameCameraState.InDialogue, 0);

        }
    }
}