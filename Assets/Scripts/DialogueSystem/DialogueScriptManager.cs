using System;
using DialogueSystem.Interfaces;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueScriptManager : MonoBehaviour
    {
        private IDialogueDecision _currentDialogueDecision;

        public void SetDialogue(IDialogueDecision newDialogue)
        {
            _currentDialogueDecision = newDialogue;
        }
        private void Start()
        {
            if (_currentDialogueDecision != null)
            {
                _currentDialogueDecision.MyIntEvent.AddListener(SubscribeDialogue);
            }
        }
        private void SubscribeDialogue(int i)
        {
            
        }
    }
}