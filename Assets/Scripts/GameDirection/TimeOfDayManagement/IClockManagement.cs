namespace GameDirection.TimeOfDayManagement
{
    public interface IClockManagement
    {
        public void SetClockAtDaytime(PartOfDay partOfDay);
        public void PlayPauseClock(bool isPlay);
        public PartOfDay GetCurrentPartOfDay();
        public void AdvanceToNextPartOfDay();
        public event ClockManagement.PassTimeOfDay OnPassTimeOfDay;
        public event ClockManagement.PassMinute OnPassMinute;

    }
}