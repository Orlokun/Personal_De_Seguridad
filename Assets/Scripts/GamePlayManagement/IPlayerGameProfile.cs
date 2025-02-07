using System;
using DataUnits.GameRequests;
using DialogueSystem;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement.ProfileDataModules;
using GamePlayManagement.ProfileDataModules.ItemSuppliers;

namespace GamePlayManagement
{
    public interface IPlayerGameProfile : IGameBasedPlayerData
    {
        public IRequestsModuleManager GetRequestsModuleManager();
        public IItemSuppliersModule GetActiveItemSuppliersModule();
        public IJobsSourcesModule GetActiveJobsModule();
        public ICalendarModule GetProfileCalendar();
        public ILifestyleModule GetLifestyleModule();
        public IPlayerGameStatusModule GetStatusModule();
        public void UpdateProfileData();
        public int GeneralOmniCredits { get;}
        public void UpdateDataEndOfDay();
        public IWorkDayObject GetCurrentWorkday();
        void PlayerLost(EndingTypes organSale);
        void ResetData();
    }

    public interface IGameBasedPlayerData
    {
        public int GameDifficulty { get; }
        public DateTime GameCreationDate { get; }
        public Guid GameId { get; }
    }
}