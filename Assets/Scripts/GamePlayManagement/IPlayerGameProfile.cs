using System;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement.ProfileDataModules;
using GamePlayManagement.ProfileDataModules.ItemSuppliers;

namespace GamePlayManagement
{
    public interface IPlayerGameProfile
    {
        public int GameDifficulty { get; }
        public DateTime GameCreationDate { get; }
        public Guid GameId { get; }
        public IItemSuppliersModule GetActiveItemSuppliersModule();
        public IJobsSourcesModule GetActiveJobsModule();
        public ICalendarModule GetProfileCalendar();
        public ILifestyleModule GetLifestyleModule();
        public IPlayerGameStatusModule GetStatusModule();
        public void UpdateProfileData();
        public int GeneralOmniCredits { get;}
        public void UpdateDataEndOfDay();
        public IWorkDayObject GetCurrentWorkday();
    }
}