using System.Collections;
using TMPro;
using UI.PopUpManager;
using UnityEngine;
using Utils;
namespace GameDirection.TimeOfDayManagement
{
    public enum DayBitId
    {
        Day_01 = 1 << 0,
        Day_02 = 1 << 1,
        Day_03 = 1 << 2,
        Day_04 = 1 << 3,
        Day_05 = 1 << 4,
        Day_06 = 1 << 5,
        Day_07 = 1 << 6,
        Day_08 = 1 << 7,
        Day_09 = 1 << 8,
        Day_10 = 1 << 9,
        Day_11 = 1 << 10,
        Day_12 = 1 << 11,
        Day_13 = 1 << 12,
        Day_14 = 1 << 13,
        Day_15 = 1 << 14
    }

    public interface IClockManagement
    {
        public void SetClockAtDaytime(PartOfDay partOfDay);
        public void PlayPauseClock(bool isPlay);
        public PartOfDay GetCurrentPartOfDay();
        public void AdvanceToNextPartOfDay();
        public event ClockManagement.PassTimeOfDay OnPassTimeOfDay;
        public event ClockManagement.PassMinute OnPassMinute;

    }

    [RequireComponent(typeof(TMP_Text))]
    public class ClockManagement : MonoBehaviour, IInitialize, IClockManagement
    {
        private static IClockManagement _mInstance;
        public static IClockManagement Instance => _mInstance;

        private IGameDirector _mDirector;
        public delegate void PassTimeOfDay(PartOfDay dayTime);
        public event PassTimeOfDay OnPassTimeOfDay;        
        public delegate void PassMinute(int hour, int minute);
        public event PassMinute OnPassMinute;
        
        #region Time of Day Members
        private const string EarlyMorningName = "Early Morning";
        private const string MorningName = "Morning";
        private const string NoonName = "Noon";
        private const string AfternoonName = "Afternoon";
        private const string EveningName = "Evening";
        private const string EndOfDayName = "End Of Day";
        private const int DayStartHour = 7;
        private const int DayStartMinute = 0;
        
        private const int MorningStartHour = 9;
        private const int MorningStartMinute = 30;
        
        private const int NoonStartHour = 12;
        private const int NoonStartMinute = 0;
        
        private const int AfternoonStartHour = 15;
        private const int AfternoonStartMinute = 0;
        
        private const int EveningStartHour = 18;
        private const int EveningStartMinute = 0;
        
        private const int DayFinishHour = 20;
        private const int DayFinishMinute = 0;
        #endregion

        private bool _isTimeAdvancing = false;
        private int _mCurrentHour = 0;
        private int _mCurrentMinute = 0;
        private bool _mIsInitialized;
        private PartOfDay _mCurrentPartOfDay;

        [SerializeField]
        private TMP_Text clockText;

        #region Init
        private void Awake()
        {
            Initialize();
        }
        public bool IsInitialized => _mIsInitialized;
        public void Initialize()
        {
            if (_mIsInitialized)
            {
                return;
            }
            if (_mInstance != null)
            {
                Destroy(this);
            }
            OnInitialize();
        }
        private void OnInitialize()
        {
            _mInstance = this;
            gameObject.SetActive(false);
            _mIsInitialized = true;
        }
        private void Start()
        {
            _mDirector = GameDirector.Instance;
        }
        #endregion

        private IEnumerator UpdateClock()
        {
            while (_isTimeAdvancing)
            {
                yield return new WaitForSeconds(.8f);
                AdvanceGameMinute();
                //Debug.Log($"[Clock Tick] {_clockText.text}");
            }
        }

        private void AdvanceGameMinute()
        {
            CheckIfChangesTimeOfDay();
            _mCurrentMinute++;
            if (_mCurrentMinute >= 60)
            {
                _mCurrentHour++;
                _mCurrentMinute = 0;
            }

            if (_mCurrentHour == DayFinishHour)
            {
                Debug.Log("DAY FINISHED");
            }
            ProcessMinutesAndHourTexts(_mCurrentHour, _mCurrentMinute);
            OnPassMinute?.Invoke(_mCurrentHour,_mCurrentMinute);
        }

        private void CheckIfChangesTimeOfDay()
        {
            if (_mCurrentHour == DayStartHour && _mCurrentMinute == DayStartMinute)
            {
                StartNewTimeOfDay(PartOfDay.EarlyMorning, EarlyMorningName, 4);
                return;
            }
            if (_mCurrentHour == MorningStartHour && _mCurrentMinute == MorningStartMinute)
            {
                StartNewTimeOfDay(PartOfDay.Morning, MorningName, 4);
                return;
            }
            if (_mCurrentHour == NoonStartHour && _mCurrentMinute == NoonStartMinute)
            {
                StartNewTimeOfDay(PartOfDay.Noon, NoonName, 4);
                return;
            }
            if (_mCurrentHour == AfternoonStartHour && _mCurrentMinute == AfternoonStartMinute)
            {
                StartNewTimeOfDay(PartOfDay.Afternoon, AfternoonName, 4);
                return;
            }
            if (_mCurrentHour == EveningStartHour && _mCurrentMinute == EveningStartMinute)
            {
                StartNewTimeOfDay(PartOfDay.Evening, EveningName, 4);
                return;
            }
            if (_mCurrentHour == DayFinishHour && _mCurrentMinute == DayFinishMinute)
            {
                StartFinishDay();
                return;
            }
        }
        private void StartFinishDay()
        {
            Debug.Log("[CheckIfChangesTimeOfDay] Finish Day");                
            PlayPauseClock(false);
            StartNewTimeOfDay(PartOfDay.EndOfDay, EndOfDayName, 4);
            _mDirector.FinishWorkday();
        }
        public void PlayPauseClock(bool isPlay)
        {
            if (isPlay && _isTimeAdvancing)
            {
                return;
            }
            
            _isTimeAdvancing = isPlay;
            if (_isTimeAdvancing)
            {
                StartCoroutine(UpdateClock());
            }
        }
        public PartOfDay GetCurrentPartOfDay()
        {
            return _mCurrentPartOfDay;
        }
        public void AdvanceToNextPartOfDay()
        {
            if (_mCurrentPartOfDay == PartOfDay.Afternoon)
            {
                StartFinishDay();   
                return;
            }
            _mCurrentPartOfDay++;
        }

        private void StartNewTimeOfDay(PartOfDay partOfDay, string timeOfDayName, float bannerTimeDuration)
        {
            Debug.Log($"[CheckIfChangesTimeOfDay] {timeOfDayName}");
            _mCurrentPartOfDay = partOfDay;
            var bannerObject = (IBannerObjectController)PopUpOperator.Instance.ActivatePopUp(BitPopUpId.LARGE_HORIZONTAL_BANNER);
            bannerObject.ToggleBannerForSeconds(timeOfDayName, bannerTimeDuration);
            OnPassTimeOfDay?.Invoke(_mCurrentPartOfDay);
            //TODO: Auto save should be implemented
        }
        


        public void SetClockAtDaytime(PartOfDay partOfDay)
        {
            if (partOfDay == _mCurrentPartOfDay && partOfDay != PartOfDay.EarlyMorning)
            {
                return;
            }
            _mCurrentPartOfDay = partOfDay;
            switch (partOfDay)
            {
                case PartOfDay.EarlyMorning:
                    _mCurrentHour = DayStartHour;
                    _mCurrentMinute = DayStartMinute;
                    ProcessMinutesAndHourTexts(_mCurrentHour, _mCurrentMinute);
                    break;
                case PartOfDay.Morning:
                    _mCurrentHour = MorningStartHour;
                    _mCurrentMinute = MorningStartMinute;
                    ProcessMinutesAndHourTexts(_mCurrentHour, _mCurrentMinute);
                    break;
                case PartOfDay.Noon:
                    _mCurrentHour = NoonStartHour;
                    _mCurrentMinute = NoonStartMinute;
                    ProcessMinutesAndHourTexts(_mCurrentHour, _mCurrentMinute);
                    break;
                case PartOfDay.Afternoon:
                    _mCurrentHour = AfternoonStartHour;
                    _mCurrentMinute = AfternoonStartMinute;
                    ProcessMinutesAndHourTexts(_mCurrentHour, _mCurrentMinute);
                    break;
                case PartOfDay.Evening:
                    _mCurrentHour = EveningStartHour;
                    _mCurrentMinute = EveningStartMinute;
                    ProcessMinutesAndHourTexts(_mCurrentHour, _mCurrentMinute);
                    break;
                case PartOfDay.EndOfDay:
                    _mCurrentHour = DayFinishHour;
                    _mCurrentMinute = DayFinishMinute;
                    ProcessMinutesAndHourTexts(_mCurrentHour, _mCurrentMinute);
                    //StartFinishDay();   
                    break;
                default:
                    return;
            }
        }

        private void ProcessMinutesAndHourTexts(int currentHour, int currentMinute)
        {
            var currentHourText = "";
            var currentMinuteText = "";

            currentMinuteText = currentMinute > 9 ? currentMinute.ToString() : "0" + currentMinute.ToString();
            currentHourText = currentHour > 9 ? currentHour.ToString() : "0" + currentHour.ToString();

            clockText.text = currentHourText + ":" + currentMinuteText;
        }
    }
}