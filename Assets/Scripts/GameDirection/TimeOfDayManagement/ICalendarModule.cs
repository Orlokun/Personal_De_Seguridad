using GamePlayManagement.ProfileDataModules;
using Players_NPC.NPC_Management.Customer_Management;

namespace GameDirection.TimeOfDayManagement
{
    public interface ICalendarModule : IProfileModule
    {
        public IWorkDayObject GetCurrentWorkDayObject();
        public IWorkDayObject GetNextWorkDayObject();
        public PartOfDay GetCurrentPartOfDay { get; }
    }
}