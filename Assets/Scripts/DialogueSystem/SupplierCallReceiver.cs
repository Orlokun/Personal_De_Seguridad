using System.Collections.Generic;
using DialogueSystem.Interfaces;
using GamePlayManagement.BitDescriptions.Suppliers;
using UnityEngine;

namespace DialogueSystem
{
    public class JobSupplierCallReceiver : SupplierCallReceiver
    {
        [SerializeField] private JobSupplierBitId supplierId;

        public override void ReceiveCall()
        {
            base.ReceiveCall();
        }

        public JobSupplierCallReceiver(int supplierId, List<IDialogueObject> iDialogueObjects) : base(supplierId, iDialogueObjects)
        {
            
        }
        
        
    }

    public abstract class SupplierCallReceiver
    {
        private int supplierId;
        public List<IDialogueObject> MainDialogueObjects;
        protected Dictionary<int, IDialogueObject> _mDialogueObjects;
        protected int awaitingCallIndex;
        protected bool _isAvailable;

        protected SupplierCallReceiver(int supplierId, List<IDialogueObject> iDialogueObjects)
        {
            SetDialogueObjects(iDialogueObjects);
        }

        private void SetDialogueObjects(List<IDialogueObject> iDialogueObjects)
        {
            _mDialogueObjects = new Dictionary<int, IDialogueObject>();
            for (var i = 0; i < iDialogueObjects.Count; i++)
            {
                _mDialogueObjects.Add(i, iDialogueObjects[i]);
            }
        }

        public virtual void ReceiveCall()
        {
            if (supplierId == 0)
            {
                Debug.LogWarning("Supplier Id must be set");
                return;
            }
            
            if (!_isAvailable)
            {
                return;
            }
            if (!AreCallConditionsMet())
            {
                return;
            }
            
        }

        protected virtual bool AreCallConditionsMet()
        {
            return true;
        }

        public virtual void ToggleEnabledStatus(bool isAvailable)
        {
            _isAvailable = isAvailable;
        }
    }
}