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
}