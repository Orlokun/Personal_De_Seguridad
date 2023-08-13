using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace DialogueSystem.Interfaces
{
    public interface IDialogueWithCameraTargets : IDialogueObject
    {
        public List<KvPair<int, Transform>> MyTargetsInDialogues { get; set; }
        public bool DialogueLineActivatesCamera(int index);
    }
}