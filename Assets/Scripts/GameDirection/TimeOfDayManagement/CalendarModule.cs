using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace GameDirection.TimeOfDayManagement
{
    public class CalendarModule : ICalendarManagement
    {
        private Dictionary<DayBitId, WorkDayObject> _dayPassed = new Dictionary<DayBitId, WorkDayObject>();
        private Dictionary<DayBitId, IWorkDayObject> _AllWorkDays = new Dictionary<DayBitId, IWorkDayObject>();
        private DayBitId _currentDay;
        private PartOfDay _timeOfDay;

        public CalendarModule(DayBitId loadCurrentDay, PartOfDay loadPartOfDay)
        {
            _currentDay = loadCurrentDay;
            _timeOfDay = loadPartOfDay;
            PopulateNewWorkdays();
        }
        private void PopulateNewWorkdays()
        {
            _AllWorkDays = new Dictionary<DayBitId, IWorkDayObject>();
            for (var i = 1; i <= (int) DayBitId.DayFifteen; i *= 2)
            {
                var id = (DayBitId) i;
                var workday = Factory.CreateWorkday(id);
                _AllWorkDays.Add(id, workday);
            }
            Debug.Log($"[PopulateNewWorkdays] Done. Workdays count: {_AllWorkDays.Count}");
        }
        
        public void FinishCurrentDay()
        {
            
        }

        public IWorkDayObject GetCurrentWorkDayObject()
        {
            if (!_dayPassed.ContainsKey(_currentDay))
            {
                Debug.LogError("Current day should be in Days passed Data dict");
                return null;
            }
            return _dayPassed[_currentDay];
        }
    }
}