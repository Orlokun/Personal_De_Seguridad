using System.Collections.Generic;
using DialogueSystem.Interfaces;
using UnityEngine;
using Utils;

namespace DialogueSystem.Units
{
    [CreateAssetMenu(menuName = "Dialogue/DialogueDecision")]
    public class DialogueWithDecision : BaseDialogueObject, IDialogueDecision
    {
        [SerializeField] protected List<string> possibilitiesList;
        [SerializeField] protected int decisionIndex;

        public List<string> DecisionPossibilities
        {
            get => possibilitiesList; 
            set => possibilitiesList = value;
        }

        public int DecisionIndex
        {
            get => decisionIndex; 
            set => decisionIndex = value;
        }

        //Decision Making Event Handling
        public MyIntEvent MyIntEvent { get; private set; }
        private void OnEnable()
        {
            MyIntEvent ??= new MyIntEvent();
        }
    }
}