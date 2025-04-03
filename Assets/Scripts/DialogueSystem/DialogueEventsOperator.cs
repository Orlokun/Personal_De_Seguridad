using System;
using System.Threading.Tasks;
using DataUnits;
using DataUnits.JobSources;
using DialogueSystem.Interfaces;
using DialogueSystem.Units;
using GameDirection;
using GamePlayManagement.BitDescriptions.Suppliers;
using UI;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueEventsOperator : IDialogueEventsOperator
    {
        public delegate void HirePlayer(JobSupplierBitId supplierId);
        public event HirePlayer OnHirePlayer;
        
        
        public delegate Task UnlockItemSupplier(BitItemSupplier itemSupplier);
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
                    case "StartIntroTimer":
                    GameDirector.Instance.StartIntroTimerEvent();
                    break;
                case "SimpleFeedback":
                    var isFeedback = Enum.TryParse(eventCodes[1], out GeneralFeedbackId feedbackId);
                    if (isFeedback)
                    {
                        FeedbackManager.Instance.StartReadingFeedback(feedbackId);
                    }
                    break;
                //Player hired only uses one event argument. The id of the job supplier. 
                case "PlayerHired":
                    LaunchPlayerHiredEvent(eventCodes[1]);
                    break;
                case "PlayerFired":
                    break;
                case "ItemUnlocked":
                    LaunchUnlockedItemEvent(eventCodes[1], eventCodes[2]);
                    break;
                case "OrganSale":
                    LaunchPlayerOrganSaleEvent();
                    break;
                //Argument One is speaker Id, Argument two is request Id
                case "Request":
                    LaunchSupplierRequestEvent(eventCodes[1], eventCodes[2]);
                    break;
                case "UnlockItemSupplier":
                    LaunchUnlockItemSupplierEvent(eventCodes[1]);
                    break;
                case "LaunchTutorial":
                    LaunchTutorialProcessEvent(eventCodes[1]);
                    break;
                case "Trust": 
                    LaunchTrustChangeEvent(eventCodes[1], eventCodes[2]);
                    break;
                default:
                    Debug.LogWarning($"Event Type: {eventCodes[0]} not recognized. Confirm writing.");
                    break;
            }
        }



        private void LaunchTrustChangeEvent(string speakerId, string trustAmount)
        {
            var supplierHasSpeakerId = int.TryParse(speakerId, out var supplierIdInt);
            if (supplierHasSpeakerId)
            {
                var supplierSpeakerId = (DialogueSpeakerId)supplierIdInt;
                var speakerData = (IFondnessCharacter)GameDirector.Instance.GetSpeakerData(supplierSpeakerId);
                speakerData.ReceiveFondness(int.Parse(trustAmount));
                var speakerCallableData = (ICallableSupplier)speakerData;
                FeedbackManager.Instance.StartTrustFeedback(speakerCallableData, int.Parse(trustAmount));
            }
        }

        private void LaunchUnlockedItemEvent(string supplierId, string itemId)
        {
            var supplierIdBit = int.TryParse(supplierId, out var supplierIdInt);
            if(supplierIdBit)
            {
                var supplierBitId = (BitItemSupplier) supplierIdInt;
                GameDirector.Instance.GetActiveGameProfile.GetActiveItemSuppliersModule().UnlockItemInSupplier(supplierBitId, int.Parse(itemId));
                GameDirector.Instance.GetActiveGameProfile.UpdateProfileData();
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
            var hasSpeakerId = Enum.TryParse(speakerId, out DialogueSpeakerId speaker);
            var hasRequestId = int.TryParse(requestId, out var requestIdInt);
            GameDirector.Instance.GetActiveGameProfile.GetRequestsModuleManager().HandleIncomingRequestActivation(speaker, requestIdInt);
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
                return;
            }
            var jobSupplierBitId = (JobSupplierBitId) supplierId;
            OnHirePlayer?.Invoke(jobSupplierBitId);
        }
        private void LaunchPlayerHiredEvent(JobSupplierBitId jobSupplier)
        {
            OnHirePlayer?.Invoke(jobSupplier);
        }
    }
}