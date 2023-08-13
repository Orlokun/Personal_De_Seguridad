using System.Collections.Generic;
using System.Linq;
using DialogueSystem.Interfaces;
using UnityEngine;

namespace DialogueSystem.Units
{
    [CreateAssetMenu(menuName = "Dialogue/DialogueWithCameraChange")]
    public class DialogueWithCameraTarget : BaseDialogueObject, IDialogueWithCameraTargets
    {
        [SerializeField] protected List<KvPair<int, Transform>> TargetsInDialogues;
        
        public List<KvPair<int, Transform>> MyTargetsInDialogues
        {
            get => TargetsInDialogues;
            set => TargetsInDialogues = value;
        }

        public bool DialogueLineActivatesCamera(int index)
        {
            return TargetsInDialogues.Any(x => x.Key == index);
        }

        public void ActivateCamera(int dialogueIndex)
        {

        }
    }
}