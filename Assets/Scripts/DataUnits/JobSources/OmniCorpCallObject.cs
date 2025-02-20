using System;
using DialogueSystem.Units;
using GamePlayManagement;
using UnityEngine;

namespace DataUnits.JobSources
{
    [Serializable]
    public class OmniCorpCallObject : ScriptableObject, ICallableSupplier
    {
        public OmniCorpCallObject()
        {
            SpeakerIndex = DialogueSpeakerId.Omnicorp;
        }
        public string SpeakerName => "Omnicorp";
        public DialogueSpeakerId SpeakerIndex { get; set; }
        public void ReceivePlayerCall(IPlayerGameProfile playerProfile)
        {
            
        }

        public int StoreHighestUnlockedDialogue => 0;
        public string SpriteName { get; set; }
    }
}