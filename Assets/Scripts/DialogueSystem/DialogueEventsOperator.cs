using DialogueSystem.Interfaces;
using GameDirection;
using GamePlayManagement.BitDescriptions.Suppliers;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueEventsOperator : IDialogueEventsOperator
    {
        public delegate void HirePlayer(JobSupplierBitId supplierId);
        public event HirePlayer OnHirePlayer;
        
        
        public delegate void UnlockItemSupplier(BitItemSupplier itemSupplier);
        public event UnlockItemSupplier OnUnlockItemSupplier;

        /// <summary>
        /// Constructor
        /// </summary>
        public DialogueEventsOperator()
        {
            OnHirePlayer += GameDirector.Instance.ManageNewJobHiredEvent;
            OnUnlockItemSupplier += GameDirector.Instance.ManageNewItemSupplierUnlockedEvent;
        }
        
        
        public void HandleDialogueEvent(string eventCompleteCode)
        {
            var eventCodes = SplitEventCodes(eventCompleteCode);
            ProcessEventType(eventCodes);
        }

        public void LaunchHireEvent(JobSupplierBitId jobSupplierId)
        {
            LaunchPlayerHiredEvent(jobSupplierId);
        }
        
        private string[] SplitEventCodes(string completeEventCode)
        {
            var codes = completeEventCode.Split(',');
            return codes;
        }
        private void ProcessEventType(string[] eventCodes)
        {
            switch (eventCodes[0])
            {
                //Player hired only uses one event argument. The id of the job supplier. 
                case "PlayerHired":
                    LaunchPlayerHiredEvent(eventCodes[1]);
                    break;
                case "PlayerFired":
                    break;
                case "ItemUnlocked":
                    break;
                case "OrganSale":
                    LaunchPlayerOrganSaleEvent();
                    break;
                //Supplier request uses 5 event arguments. 
                //1. The job supplier id            ex: 1 = countPetrolk
                //2. RequestTypeId                  ex: 4 = NotUse
                //3. RequestLogicType               ex: 3 = EqualsTo
                //4. RequestParameter Type          ex: 2 = race
                //5. RequestParameter itself        ex: 8 = robot
                case "Request":
                    LaunchSupplierRequestEvent(eventCodes[1], eventCodes[2]);
                    break;
                case "UnlockItemSupplier":
                    LaunchUnlockItemSupplierEvent(eventCodes[1]);
                    break;
                case "LaunchTutorial":
                    LaunchTutorialProcessEvent(eventCodes[1]);
                    break;
            }
        }

        private void LaunchTutorialProcessEvent(string tutorialIndex)
        {
            var hasTutorialIndex = int.TryParse(tutorialIndex, out var tutorialIndexInt);
            if (hasTutorialIndex)
            {
                GameDirector.Instance.StartTutorialProcess(tutorialIndexInt);
            }
        }

        private void LaunchUnlockItemSupplierEvent(string eventCode)
        {
            var hasSupplierId = int.TryParse(eventCode, out var supplierIdInt);
            if (!hasSupplierId)
            {
                Debug.LogError("[LaunchUnlockItemSupplierEvent] Event must have the item supplier id");
                return;
            }
            
            var itemSupplierBitId = (BitItemSupplier) supplierIdInt;
            OnUnlockItemSupplier?.Invoke(itemSupplierBitId);
        }

        private void LaunchSupplierRequestEvent(string speakerId, string requestId)
        {
            GameDirector.Instance.GetActiveGameProfile.GetRequestsModuleManager();
        }


        private void LaunchPlayerOrganSaleEvent()
        {
            GameDirector.Instance.PlayerLost(EndingTypes.ORGAN_SALE);
        }

        private void LaunchPlayerHiredEvent(string jobSupplier)
        {
            var getJobSupplierId = int.TryParse(jobSupplier, out var supplierId);
            if (!getJobSupplierId)
            {
                Debug.LogError("[LaunchPlayerHiredEvent] Event must have the job supplier id");
            }
            var jobSupplierBitId = (JobSupplierBitId) supplierId;
            OnHirePlayer?.Invoke(jobSupplierBitId);
        }
        private void LaunchPlayerHiredEvent(JobSupplierBitId jobSupplier)
        {
            OnHirePlayer?.Invoke(jobSupplier);
        }
    }
    public enum EndingTypes
    {
        ORGAN_SALE
    }


}