using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataUnits.GameRequests;
using DialogueSystem.Interfaces;
using DialogueSystem.Phone;
using DialogueSystem.Units;
using GameDirection;
using GamePlayManagement.BitDescriptions.Suppliers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DataUnits.JobSources
{
    [Serializable]
    public class CountPetrolkSupplierObject : JobSupplierObject
    {
        private CountPetrolkDialogueStates _mDialogueState;
        public CountPetrolkDialogueStates DialogueState => _mDialogueState;


        private int _mOvercomedDialogueStates;

        private List<IGameRequest> _mChallenges;
        
        
        
        public override void LocalInitialize(JobSupplierBitId id, DialogueSpeakerId speakerId)
        {
            base.LocalInitialize(id, speakerId);
            _mDialogueState = CountPetrolkDialogueStates.WaitingForHire;
        }
        
        protected override async void BuildResponseAndAnswer()
        {
            base.BuildResponseAndAnswer();
            Debug.LogWarning("[StartAnswerBuildingProcess] Store can be called");
            Random.InitState(DateTime.Now.Millisecond);
            var randomWaitTime = Random.Range(500, 4500);
            await Task.Delay(randomWaitTime);
            if (_dialogueModule.ImportantDialogues.Any(x => x.Value.TimesActivatedCount == 0 && x.Value.GetDialogueAssignedStatus == (int)DialogueState))
            {
                var importantDialogue = _dialogueModule.ImportantDialogues.FirstOrDefault(x => x.Value.TimesActivatedCount == 0 && x.Value.GetDialogueAssignedStatus == (int)DialogueState).Value;
                AnswerPhoneWithDialogueReady(importantDialogue);
                return;
            }
            var insistenceList = _dialogueModule.InsistenceDialogues.Select(x=>x.Value).Where(x => x.GetDialogueAssignedStatus == (int)DialogueState).ToList();
            var insistenceDialoguesCount = insistenceList.Count;
            Random.InitState(DateTime.Now.Millisecond);
            var randomInsistenceIndex = Random.Range(0, insistenceDialoguesCount);
            var resultDialogue = insistenceList[randomInsistenceIndex];
            AnswerPhoneWithDialogueReady(resultDialogue);

        }
        private void AnswerPhoneWithDialogueReady(IDialogueObject answer)
        {
            PhoneCallOperator.Instance.PlayAnswerSound();
            GameDirector.Instance.GetDialogueOperator.StartNewDialogue(answer);
        }

        public override void PlayerHired()
        {
            _mOvercomedDialogueStates += (int)DialogueState; 
            _mDialogueState = CountPetrolkDialogueStates.RequiresMindProtection;
        }
        
        public override void ActivateRequest(IGameRequest request)
        {
            Debug.Log("CountPetrolkSupplierObject: ActivateChallenge");
        }

        public override void PlayerLostResetData()
        {
            base.PlayerLostResetData();
            _mDialogueState = CountPetrolkDialogueStates.WaitingForHire;
        }
    }
}