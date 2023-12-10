using System;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement.ProfileDataModules;
using GamePlayManagement.ProfileDataModules.ItemSuppliers;
using UI;

namespace GamePlayManagement
{
    public interface IPlayerGameProfile
    {
        public DateTime GameCreationDate { get; }
        public Guid GameId { get; }
        public IItemSuppliersModule GetActiveItemSuppliersModule();
        public IJobsSourcesModule GetActiveJobsModule();
        public ICalendarModule GetProfileCalendar();
        public ILifestyleModule GetLifestyleModule();
        public void UpdateProfileData();
        public int GeneralOmniCredits { get; set; }
        public void UpdateDataEndOfDay();
    }
}