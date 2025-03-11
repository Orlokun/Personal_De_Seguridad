using DialogueSystem;
using DialogueSystem.Units;
using GameDirection.ComplianceDataManagement;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement.GameRequests.RequestsManager;
using GamePlayManagement.GameRequests.RewardsPenalties;
using GamePlayManagement.ProfileDataModules;
using GamePlayManagement.ProfileDataModules.ItemSuppliers;

namespace GamePlayManagement
{
    public interface IPlayerGameProfile : IGameBasedPlayerData
    {
        public IRequestsModuleManager GetRequestsModuleManager();
        public IComplianceManager GetComplianceManager { get; }
        public IItemSuppliersModule GetActiveItemSuppliersModule();
        public IJobsSourcesModule GetActiveJobsModule();
        public ICalendarModule GetProfileCalendar();
        public ILifestyleModule GetLifestyleModule();
        public IPlayerGameStatusModule GetStatusModule();
        public IPlayerInventoryModule GetInventoryModule();
        public void UpdateProfileData();
        public int GeneralOmniCredits { get;}
        public void UpdateDataEndOfDay();
        public IWorkDayObject GetCurrentWorkday();
        void PlayerLost(EndingTypes organSale);
        void AddFondnessToActiveSupplier(ITrustRewardData trustRewardData);
        void UpdateSupplierTrustLevel(DialogueSpeakerId supplierId, int trustLevel);
    }
}