using System;
using System.Collections;
using TMPro;
using UI.PopUpManager;
using UnityEngine;
using Utils;
namespace GameDirection.TimeOfDayManagement
{
    public enum DayBitId
    {
        DayOne = 1,
        DayTwo = 2,
        DayThree = 4,
        DayFour = 8,
        DayFive = 16,
        DaySix = 32,
        DaySeven = 64,
        DayEight = 128,
        DayNine = 256,
        DayTen = 512,
        Day,Eleven = 1024,
        DayTwelve = 2048,
        DayThirteen = 4096,
        DayFourteen = 8192,
        DayFifteen = 16384
    }

    public class JournalManagement
    {
        private DayBitId _currentDay;
        private PartOfDay _timeOfDay;
        private IPlayerProfileManager _mActivePlayerProfile;

        public JournalManagement(DayBitId loadCurrentDay, PartOfDay loadPartOfDay, IPlayerProfileManager activePlayerProfile)
        {
            _currentDay = loadCurrentDay;
            _timeOfDay = loadPartOfDay;
            _mActivePlayerProfile = activePlayerProfile;
        }
    }

    public interface IClockManagement
    {
        public void SetClockAtDaytime(PartOfDay partOfDay);
        public void PlayPauseClock(bool isPlay);
        public PartOfDay GetCurrentPartOfDay();
        public void AdvanceToNextPartOfDay();
    }

    [RequireComponent(typeof(TMP_Text))]
    public class ClockManagement : MonoBehaviour, IInitialize, IClockManagement
    {
        private static IClockManagement _mInstance;
        public static IClockManagement Instance => _mInstance;

        #region Time of Day Members
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
        
        private const int NightStartHour = 21;
        private const int NightStartMinute = 0;
        
        private const int DayFinishHour = 19;
        private const int DayFinishMinute = 0;
        #endregion

        private bool isTimeAdvancing = false;
        private int _mCurrentHour = 0;
        private int _mCurrentMinute = 0;
        private bool _mIsInitialized;
        private PartOfDay _mCurrentPartOfDay;

        [SerializeField]
        private TMP_Text _clockText;

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
        private void OnEnable()
        {
            if (isTimeAdvancing || !_mIsInitialized)
            {
                return;
            }
            StartCoroutine(UpdateClock());
        }
        #endregion

        private IEnumerator UpdateClock()
        {
            while (isTimeAdvancing)
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
        }

        private void CheckIfChangesTimeOfDay()
        {
            if (_mCurrentHour == DayStartHour && _mCurrentMinute == DayStartMinute)
            {
                Debug.Log("[CheckIfChangesTimeOfDay] Early Morning");
                _mCurrentPartOfDay = PartOfDay.EarlyMorning;
                var bannerObject = (IBannerObjectController)PopUpOperator.Instance.ActivatePopUp(BitPopUpId.LARGE_HORIZONTAL_BANNER);
                bannerObject.ToggleBannerForSeconds("Early Morning", 3);
                return;
            }
            if (_mCurrentHour == MorningStartHour && _mCurrentMinute == MorningStartMinute)
            {
                Debug.Log("[CheckIfChangesTimeOfDay] Morning");
                _mCurrentPartOfDay = PartOfDay.Morning;
                var bannerObject = (IBannerObjectController)PopUpOperator.Instance.ActivatePopUp(BitPopUpId.LARGE_HORIZONTAL_BANNER);
                bannerObject.ToggleBannerForSeconds("Morning", 3);
                return;
            }
            if (_mCurrentHour == NoonStartHour && _mCurrentMinute == NoonStartHour)
            {
                Debug.Log("[CheckIfChangesTimeOfDay] Noon");
                _mCurrentPartOfDay = PartOfDay.Noon;
                var bannerObject = (IBannerObjectController)PopUpOperator.Instance.ActivatePopUp(BitPopUpId.LARGE_HORIZONTAL_BANNER);
                bannerObject.ToggleBannerForSeconds("Noon", 3);
                return;
            }
            if (_mCurrentHour == AfternoonStartHour && _mCurrentMinute == AfternoonStartMinute)
            {
                Debug.Log("[CheckIfChangesTimeOfDay] Afternoon");
                _mCurrentPartOfDay = PartOfDay.Afternoon;
                var bannerObject = (IBannerObjectController)PopUpOperator.Instance.ActivatePopUp(BitPopUpId.LARGE_HORIZONTAL_BANNER);
                bannerObject.ToggleBannerForSeconds("Afternoon", 4);   
                return;
            }
            if (_mCurrentHour == NightStartHour && _mCurrentMinute == NightStartMinute)
            {
                Debug.Log("[CheckIfChangesTimeOfDay] Finish Day");                
                _mCurrentPartOfDay = PartOfDay.Night;
                var bannerObject = (IBannerObjectController)PopUpOperator.Instance.ActivatePopUp(BitPopUpId.LARGE_HORIZONTAL_BANNER);
                bannerObject.ToggleBannerForSeconds("Night", 4);
                return;
            }
        }

        public void PlayPauseClock(bool isPlay)
        {
            isTimeAdvancing = isPlay;
            if (isTimeAdvancing)
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

        private void StartFinishDay()
        {
            Debug.Log("Finishing Day");   
        }

        public void SetClockAtDaytime(PartOfDay partOfDay)
        {
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
                case PartOfDay.Night:
                    _mCurrentHour = NightStartHour;
                    _mCurrentMinute = NightStartMinute;
                    ProcessMinutesAndHourTexts(_mCurrentHour, _mCurrentMinute);
                    break;
            }
        }

        private void ProcessMinutesAndHourTexts(int currentHour, int currentMinute)
        {
            var currentHourText = "";
            var currentMinuteText = "";

            currentMinuteText = currentMinute > 9 ? currentMinute.ToString() : "0" + currentMinute.ToString();
            currentHourText = currentHour > 9 ? currentHour.ToString() : "0" + currentHour.ToString();

            _clockText.text = currentHourText + ":" + currentMinuteText;
        }
    }
}