using System;
using System.Linq;
using DialogueSystem.Interfaces;
using GameDirection;
using GamePlayManagement.BitDescriptions.Suppliers;
using Random = UnityEngine.Random;

namespace DataUnits.JobSources
{
    [Serializable]
    public class CountPetrolkSupplierObject : JobSupplierObject
    {
        private CountPetrolkDialogueStates _mDialogueState;
        public CountPetrolkDialogueStates DialogueState => _mDialogueState;
        


        public override void LocalInitialize(JobSupplierBitId id)
        {
            base.LocalInitialize(id);
            _mDialogueState = CountPetrolkDialogueStates.WaitingForHire;
        }
        
        protected override async void BuildResponseAndAnswer()
        {
            base.BuildResponseAndAnswer();
            if (_dialogueModule.ImportantDialogues.Any(x => x.Value.TimesActivatedCount == 0 && x.Value.GetDialogueAssignedStatus == (int)DialogueState))
            {
                var importantDialogue = _dialogueModule.ImportantDialogues.FirstOrDefault(x => x.Value.TimesActivatedCount == 0 && x.Value.GetDialogueAssignedStatus == (int)DialogueState).Value;
                AnswerPhoneWithDialogueReady(importantDialogue);
                return;
            }
            var insistenceDialoguesCount = _dialogueModule.InsistenceDialogues.Count;
            Random.InitState(DateTime.Now.Millisecond);
            var randomInsistenceIndex = Random.Range(1, insistenceDialoguesCount+1);
            var resultDialogue = _dialogueModule.InsistenceDialogues[randomInsistenceIndex];
            AnswerPhoneWithDialogueReady(resultDialogue);
        }
        

        private void AnswerPhoneWithDialogueReady(IDialogueObject answer)
        {
            PhoneCallOperator.Instance.PlayAnswerSound();
            GameDirector.Instance.GetDialogueOperator.StartNewDialogue(answer);
        }

    }
}