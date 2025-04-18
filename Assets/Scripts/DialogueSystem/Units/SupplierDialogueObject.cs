﻿using System.Collections.Generic;
using DialogueSystem.Interfaces;
using UnityEngine;

namespace DialogueSystem.Units
{
    [CreateAssetMenu(menuName = "Dialogue/DialogueObject")]
    public class SupplierDialogueObject : DialogueObject, ISupplierDialogueObject
    {
        public List<IDialogueObject> DialogueNodes { get; set; }
        private int _mDialogueStatus;
        public int GetDialogueAssignedStatus => _mDialogueStatus;
        public void SetDialogueStatus(int status)
        {
            _mDialogueStatus = status;
        }
    }
}