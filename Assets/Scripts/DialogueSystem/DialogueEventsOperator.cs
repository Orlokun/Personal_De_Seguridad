using DataUnits.GameCatalogues;
using DataUnits.JobSources;
using DialogueSystem.Interfaces;
using GameDirection;
using GamePlayManagement.BitDescriptions.SupplierChallenges;
using GamePlayManagement.BitDescriptions.Suppliers;
using UI.PopUpManager;
using UnityEngine;
using Utils;

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
        
        
        private void ItemSupplierUnlocked(BitItemSupplier newItemSupplier)
        {
            var jobSupplierName = BaseItemSuppliersCatalogue.Instance.GetItemSupplierData(newItemSupplier).StoreName;
            var bannerObject = (IBannerObjectController)PopUpOperator.Instance.ActivatePopUp(BitPopUpId.LARGE_HORIZONTAL_BANNER);
            var bannerText = $"New Item Supplier Unlocked: {jobSupplierName}";
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
                case "JobSupplierRequest":
                    LaunchSupplierRequestEvent(eventCodes[1], eventCodes[2], 
                        eventCodes[3], eventCodes[4], eventCodes[5]);
                    break;
                
                case "UnlockItemSupplier":
                    LaunchUnlockItemSupplierEvent(eventCodes[1]);
                    break;
            }
        }

        private void LaunchUnlockItemSupplierEvent(string eventCode)
        {
            var hasSupplierId = int.TryParse(eventCode, out var supplierIdInt);
            if (!hasSupplierId)
            {
                Debug.LogError("[LaunchUnlockItemSupplierEvent] Event must have the item supplier id");
            }
            var itemSupplierBitId = (BitItemSupplier) supplierIdInt;
            var playerProfile = GameDirector.Instance.GetActiveGameProfile;
            var isItemSupplierActive = BitOperator.IsActive(playerProfile.GetActiveItemSuppliersModule().UnlockedItemSuppliers, (int) itemSupplierBitId);
            if (isItemSupplierActive)
            {
                return;
            }
            playerProfile.GetActiveItemSuppliersModule().UnlockSupplier(itemSupplierBitId);
            ItemSupplierUnlocked(itemSupplierBitId);
        }

        private void LaunchSupplierRequestEvent(string supplierStringId, string requestTypeString, string requestLogicString, 
            string requestParameterTypeString, string parameterValueString)
        {
            
            var hasSupplierId = int.TryParse(supplierStringId, out var supplierIdInt);
            var hasRequestTypeId = int.TryParse(requestTypeString, out var requestIdInt);
            var hasLogicType = int.TryParse(requestTypeString, out var requestLogicOperator);
            var hasParameterType = int.TryParse(requestParameterTypeString, out var requestParameterType);
            var hasRequestParameterValue = int.TryParse(parameterValueString, out var requestParameterValue);
            
            
            var supplierBitId = (JobSupplierBitId) supplierIdInt;
            if (!hasSupplierId || !hasRequestTypeId || !hasLogicType || !hasParameterType || !hasRequestParameterValue)
            {
                Debug.LogError("[LaunchSupplierRequestEvent] Request Must have all parameters available");
            }
            //
            var playerProfile = GameDirector.Instance.GetActiveGameProfile;
            var isJobSupplierActive = BitOperator.IsActive(playerProfile.GetActiveJobsModule().UnlockedJobSuppliers, (int) supplierBitId);
            if (!isJobSupplierActive)
            {
                return;
            }
            var challengeObject = Factory.CreateChallengeObject(supplierBitId, (RequestChallengeType)requestIdInt, 
                (RequestChallengeLogicOperator)requestLogicOperator, (ConsideredParameter)requestParameterType, requestParameterValue);
            var jobSupplier = (INPCRequesterModule)BaseJobsCatalogue.Instance.GetJobSupplierObject(supplierBitId);
            jobSupplier.ActivateChallenge(challengeObject);
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

    public interface IJobSupplierChallengeObject
    {
        public bool IsCompleted { get; }
        public void CompleteChallenge();
    }
}