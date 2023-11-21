using System;
using System.Threading.Tasks;
using GameDirection;
using GameDirection.TimeOfDayManagement;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.PopUpManager.OfficeRelatedPopUps
{
    public class CigarConfirmationPopUp : ConfirmationPopUp
    {
        [SerializeField]private TMP_Dropdown mDayTimeTarget;
        private void Start()
        {
            //GameDirector.Instance.GetClockInDayManagement.PlayPauseClock(false);
        }

        public override void ConfirmAction()
        {
            //1. Get Value from Dropdown            //DONE
            if (mDayTimeTarget == null)
            {
                return;
            }
            var newPartOfDay = GetDropDownValue();
            DoCigarAction(newPartOfDay);
            //. Pop up clear
            Debug.Log("[CigarConfirmationPopUp] Action confirmed");
        }

        private async void DoCigarAction(PartOfDay newPartOfDay)
        {
            //2.FadeOut
            PopUpOperator.RemovePopUp(PopUpId);
            GameDirector.Instance.GetGeneralBackgroundFader.GeneralCameraFadeOut();
            GameDirector.Instance.ChangeHighLvlGameState(HighLevelGameStates.InCutScene);
            //3. Play lighter sound
            //4. Charge action cost
            await Task.Delay(2500);

            //5. Go to time of day from dropdown
            ClockManagement.Instance.SetClockAtDaytime(newPartOfDay);
            GameDirector.Instance.GetGeneralBackgroundFader.GeneralFadeIn();
            GameDirector.Instance.ChangeHighLvlGameState(HighLevelGameStates.InGame);
            ClockManagement.Instance.PlayPauseClock(true);

        }

        private PartOfDay GetDropDownValue()
        {
            var selectedIndex = mDayTimeTarget.value;
            var selectedDayTime = mDayTimeTarget.options[selectedIndex].text;
            var newPartOfDay = ClockManagement.Instance.GetCurrentPartOfDay();
            switch (selectedDayTime)
            {
                case "Morning":
                    newPartOfDay = PartOfDay.Morning;
                    break;
                case "Noon":
                    newPartOfDay = PartOfDay.Noon;
                    break;
                case "Afternoon":
                    newPartOfDay = PartOfDay.Afternoon;
                    break;
                case "Evening":
                    newPartOfDay = PartOfDay.Evening;
                    break;
                case "End Of Day":
                    newPartOfDay = PartOfDay.EndOfDay;
                    break;
            }
            if (newPartOfDay <= ClockManagement.Instance.GetCurrentPartOfDay())
            {
                return ClockManagement.Instance.GetCurrentPartOfDay();
            }
            return newPartOfDay;
        }
        private void SetDropDownValue()
        {
            
        }
        public override void CancelAction()
        {
            GameDirector.Instance.GetClockInDayManagement.PlayPauseClock(true);
            PopUpOperator.RemovePopUp(PopUpId);
        }

        private void OnDisable()
        {
        }
    }
}