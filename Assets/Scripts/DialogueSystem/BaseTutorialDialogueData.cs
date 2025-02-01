using System.Collections;
using System.Collections.Generic;
using DataUnits.GameCatalogues.JsonCatalogueLoaders;
using DialogueSystem.Interfaces;
using DialogueSystem.Units;
using GameDirection;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Utils;

namespace DialogueSystem
{
    public class BaseTutorialDialogueData : IBaseTutorialDialogueData
    {
        private TutorialDialogueBaseData _mTutorialBaseData;
        private Dictionary<int, IDialogueObject> _mTutorialDialogues;
        
        public BaseTutorialDialogueData()
        {
            var tutorialDialogueUrl = DataSheetUrls.GetTutorialDialogueUrl();
            GameDirector.Instance.ActCoroutine(DownloadDialogueData(tutorialDialogueUrl));
        }
        private IEnumerator DownloadDialogueData(string url)
        {
            var webRequest = UnityWebRequest.Get(url);
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(
                    $"tutorial dialogue data must be reachable. Error: {webRequest.result}. {webRequest.error}");
            }
            else
            {
                var sourceJson = webRequest.downloadHandler.text;
                LoadTutorialDialogues(sourceJson);
            }
        }

        private void LoadTutorialDialogues(string sourceJson)
        {
            //Debug.Log($"[JobSupplier.LoadImportantDialoguesFromJson] Begin request");
            _mTutorialBaseData = JsonConvert.DeserializeObject<TutorialDialogueBaseData>(sourceJson);
            //Debug.Log($"Finished parsing. Is Job Supplier Dialogue null?: {_mInsistenceDialoguesData == null}. {_mInsistenceDialoguesData}");
            _mTutorialDialogues = new Dictionary<int, IDialogueObject>();
            
            int lastDialogueObjectIndex = 0;
            IDialogueObject currentDialogueObject;
            for (var i = 1; i < _mTutorialBaseData.values.Count; i++)
            {
                var isDialogueGroupId = int.TryParse(_mTutorialBaseData.values[i][0], out var currentDialogueObjectIndex);
                
                if (!isDialogueGroupId)
                {
                    Debug.LogWarning($"[BaseTutorialDialogueData.LoadTutorialDialogues] Dialogues for tutorial must have node Index");
                }
                
                if (lastDialogueObjectIndex != currentDialogueObjectIndex || i == 1)
                {
                    lastDialogueObjectIndex = currentDialogueObjectIndex;
                    
                    currentDialogueObject = ScriptableObject.CreateInstance<DialogueObject>();
                    
                    _mTutorialDialogues.Add(lastDialogueObjectIndex, currentDialogueObject);
                }
                
                var hasDialogueNodeId = int.TryParse(_mTutorialBaseData.values[i][1], out var dialogueLineIndex);
                if (!hasDialogueNodeId)
                {
                    Debug.LogWarning($"[BaseTutorialDialogueData.LoadTutorialDialogues]  Dialogues for tutorial must have an index");
                    return;
                }
                
                int.TryParse(_mTutorialBaseData.values[i][2], out var speakerId);

                var dialogueLineText = _mTutorialBaseData.values[i][3];
                var cameraArgs = _mTutorialBaseData.values[i][4].Split(',');
                var hasCameraTarget = cameraArgs.Length > 1;
                
                var eventNameId = _mTutorialBaseData.values[i][5];
                var hasEventId = eventNameId != "0";
                
                var linksToString = _mTutorialBaseData.values[i][6].Split(',');
                var linksToInts = DialogueProcessor.ProcessLinksStrings(linksToString);
                var linksToFinish = linksToInts[0] == 0;
                var hasChoices = linksToInts.Length > 1;

                //Highlight event
                var hasHighlightEvent = _mTutorialBaseData.values[i][7] != "0";
                var emptyString = new string[1] {"0"};
                var highlightEvent = hasHighlightEvent ? _mTutorialBaseData.values[i][7].Split(',') : emptyString;
                
                var dialogueNode = new DialogueNodeData(currentDialogueObjectIndex, dialogueLineIndex, speakerId, dialogueLineText,
                    hasCameraTarget, cameraArgs, hasChoices, hasEventId, eventNameId, linksToInts, hasHighlightEvent, highlightEvent);
                _mTutorialDialogues[currentDialogueObjectIndex].DialogueNodes.Add(dialogueNode);
            }
            
        }

        public IDialogueObject GetTutorialDialogue(int dialogueIndex)
        {
            if (_mTutorialDialogues.TryGetValue(dialogueIndex, out var dialogue))
            {
                return dialogue;
            }
            else
            {
                Debug.LogWarning($"[BaseTutorialDialogueData.GetTutorialDialogue] Dialogue with index {dialogueIndex} not found");
                return null;
            }
        }
    }

    public interface IBaseTutorialDialogueData
    {
        IDialogueObject GetTutorialDialogue(int dialogueIndex);
    }
}