using GamePlayManagement.ProfileDataModules;
namespace GameDirection.TimeOfDayManagement
{
    public interface ICalendarManagement : IProfileModule
    {
        public void FinishCurrentDay();
    }
}