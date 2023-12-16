using DataUnits.GameCatalogues;
using DialogueSystem.Interfaces;
using GameDirection;
using GamePlayManagement.BitDescriptions.Suppliers;
using UI.PopUpManager;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueEventsOperator : IDialogueEventsOperator
    {
        public delegate void HirePlayer(JobSupplierBitId supplierId);
        public event HirePlayer OnHirePlayer;
       
        /// <summary>
        /// Constructor
        /// </summary>
        public DialogueEventsOperator()
        {
            OnHirePlayer += GameDirector.Instance.ManageNewJobHiredEvent;
            OnHirePlayer += JobFoundFeedbackEvent;
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

        private void JobFoundFeedbackEvent(JobSupplierBitId newJobSupplier)
        {
            var jobSupplierName = BaseJobsCatalogue.Instance.GetJobSupplierObject(newJobSupplier).StoreName;
            var bannerObject = (IBannerObjectController)PopUpOperator.Instance.ActivatePopUp(BitPopUpId.LARGE_HORIZONTAL_BANNER);
            var bannerText = $"New Job Found in: {jobSupplierName}";
            bannerObject.ToggleBannerForSeconds(bannerText, 4);
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
                case "PlayerHired":
                    LaunchPlayerHiredEvent(eventCodes[1]);
                    break;
                case "PlayerFired":
                    break;
                case "ItemUnlocked":
                    break;
            }
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
}