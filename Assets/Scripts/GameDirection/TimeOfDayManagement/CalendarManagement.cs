using System.Collections.Generic;

namespace GameDirection.TimeOfDayManagement
{
    public class CalendarManagement : ICalendarManagement
    {
        private Dictionary<DayBitId, WorkDayObject> _daysPassed = new Dictionary<DayBitId, WorkDayObject>();
        private DayBitId _currentDay;
        private PartOfDay _timeOfDay;

        public CalendarManagement(DayBitId loadCurrentDay, PartOfDay loadPartOfDay)
        {
            _currentDay = loadCurrentDay;
            _timeOfDay = loadPartOfDay;
        }
    }
}