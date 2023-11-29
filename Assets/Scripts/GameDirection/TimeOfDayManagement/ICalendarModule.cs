using GamePlayManagement.ProfileDataModules;
namespace GameDirection.TimeOfDayManagement
{
    public interface ICalendarModule : IProfileModule
    {
        public IWorkDayObject GetCurrentWorkDayObject();

        public void StartNightManagement();
    }
}