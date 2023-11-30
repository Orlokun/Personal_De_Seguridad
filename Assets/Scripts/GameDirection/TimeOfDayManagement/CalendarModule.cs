using System.Collections.Generic;
using GamePlayManagement;
using UnityEngine;
using Utils;

namespace GameDirection.TimeOfDayManagement
{
    public class CalendarModule : ICalendarModule
    {
        private IPlayerGameProfile _activePlayer;
        private IClockManagement _clockManagement;
        private Dictionary<DayBitId, WorkDayObject> _dayPassed = new Dictionary<DayBitId, WorkDayObject>();
        private Dictionary<DayBitId, IWorkDayObject> _AllWorkDays = new Dictionary<DayBitId, IWorkDayObject>();
        private IWorkDayObject _currentDayObject;
        private DayBitId _currentDayId;
        private PartOfDay _currentTimeOfDay;

        public CalendarModule(DayBitId loadCurrentDayId, PartOfDay loadPartOfTimeOfDay, IClockManagement clockManagement)
        {
            _currentDayId = loadCurrentDayId;
            _currentTimeOfDay = loadPartOfTimeOfDay;
            _clockManagement = clockManagement;
            PopulateNewWorkdays();
            _currentDayObject = _AllWorkDays[_currentDayId];
            _clockManagement.OnPassTimeOfDay += AddNewTimeOfDay;
        }
        
        //TODO: Constructor with profile data saved from previous games
        
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
        
        public IWorkDayObject GetCurrentWorkDayObject()
        {
            if (_currentDayId != _currentDayObject.BitId)
            {
                Debug.LogError("Current day Ids must be consistent.");
                return null;
            }
            return _currentDayObject;
        }

        public void StartNightManagement()
        {
            throw new System.NotImplementedException();
        }

        private void AddNewTimeOfDay(PartOfDay newDayTime)
        {
            if (_currentDayObject == null)
            {
                Debug.LogError("[CalendarModule.AddNewTimeOfDay] Current Day object must be set");
                return;
            }
            _AllWorkDays[_currentDayId].UpdatePartOfDay(newDayTime);
        }

        public void SetProfile(IPlayerGameProfile currentPlayerProfile)
        {
            _activePlayer = currentPlayerProfile;
        }
    }
}