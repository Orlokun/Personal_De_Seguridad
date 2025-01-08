using GamePlayManagement.ProfileDataModules;

namespace GameDirection.TimeOfDayManagement
{
    public interface ICalendarModule : IProfileModule
    {
        public void SetCurrentWorkDayObject(DayBitId newDay);

        public IWorkDayObject GetCurrentWorkDayObject();
        public IWorkDayObject GetNextWorkDayObject();
        public PartOfDay GetCurrentPartOfDay { get; }
    }
}