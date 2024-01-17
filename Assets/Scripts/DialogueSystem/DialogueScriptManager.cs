using DialogueSystem.Interfaces;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueScriptManager : MonoBehaviour
    {
        private IDecisionObject _currentDecisionObject;

        public void SetDialogue(IDecisionObject @new)
        {
            _currentDecisionObject = @new;
        }
        private void Start()
        {
            if (_currentDecisionObject != null)
            {
                _currentDecisionObject.MyIntEvent.AddListener(SubscribeDialogue);
            }
        }
        private void SubscribeDialogue(int i)
        {
            
        }
    }
}