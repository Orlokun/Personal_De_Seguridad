using System.Collections.Generic;
using System.Linq;
using DialogueSystem.Interfaces;
using DialogueSystem.Units;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class DialogueChoicesGameOperator : MonoBehaviour, IDialogueChoicesGameOperator
    {
        [SerializeField] private GameObject ChoicePrefab;
        private Dictionary<int, GameObject> _mInstantiatedChoiceButtons;
        public void ToggleActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void DisplayDialogueChoices(IDialogueNode dialogueNode, IDialogueObject completeDialogue, IDialogueOperator dialogueOperator)
        {
            if (!dialogueNode.HasChoice || dialogueNode.LinkNodes.Length <=1)
            {
                Debug.LogError("Dialogue node must have choices set at this point of the process. Confirm choice process");
            }
            _mInstantiatedChoiceButtons = new Dictionary<int, GameObject>();
            foreach (var linkNode in dialogueNode.LinkNodes)
            {
                var choiceButtonObject = Instantiate(ChoicePrefab, gameObject.transform);
                var choiceButtonComponent = choiceButtonObject.transform.GetChild(0).GetComponent<TMP_Text>();
                _mInstantiatedChoiceButtons.Add(linkNode,choiceButtonObject);
                var choiceButton = choiceButtonObject.GetComponent<Button>();
            
                var buttonDestinyNode = completeDialogue.DialogueNodes[linkNode].LinkNodes.First();
                choiceButtonComponent.text = completeDialogue.DialogueNodes[linkNode].DialogueLine;
                choiceButton.onClick.AddListener(OnButtonClick);
                choiceButton.onClick.AddListener(delegate { dialogueOperator.WriteNextDialogueNode(completeDialogue, buttonDestinyNode); });
                choiceButton.onClick.AddListener(CloseDialogueChoices);
            }
        }

        private void OnButtonClick()
        {
            Debug.Log("Button was clicked");
        }
    
        private void CloseDialogueChoices()
        {
            foreach (var mInstantiatedChoiceButton in _mInstantiatedChoiceButtons)
            {
                Destroy(mInstantiatedChoiceButton.Value); 
            }   
            _mInstantiatedChoiceButtons.Clear();
            gameObject.SetActive(false);
        }
    }
}