using System.Collections.Generic;
using System.Linq;
using DialogueSystem.Interfaces;
using DialogueSystem.Units;
using UnityEngine;
using Utils;

namespace DialogueSystem
{
    [CreateAssetMenu(menuName = "Dialogue/DialogueWithMovementAndDecision")]
    public class DialogueWithTargetAndDialogueDecision : BaseDialogueObject, IDialogueWithCameraTargets, IDialogueDecision
    {
        [SerializeField] protected List<string> decisionPossibilities;
        [SerializeField] protected int decisionIndex;
        [SerializeField] protected List<KvPair<int, Transform>> TargetsInDialogues;
            
        public List<string> DecisionPossibilities { get; set; }
        public int DecisionIndex { get; set; }
        public MyIntEvent MyIntEvent { get; }

        public List<KvPair<int, Transform>> MyTargetsInDialogues
        {
            get => TargetsInDialogues;
            set => TargetsInDialogues = value;
        }
        
        public bool DialogueLineActivatesCamera(int index)
        {
            return TargetsInDialogues.Any(x => x.Key == index);
        }
    }
}