using System;
using System.Threading.Tasks;
using GameDirection;
using GameDirection.TimeOfDayManagement;
using TMPro;
using UnityEngine;

namespace UI.PopUpManager.OfficeRelatedPopUps
{
    public class CigarConfirmationPopUp : ConfirmationPopUp
    {
        [SerializeField]private TMP_Dropdown mDayTimeTarget;
        [SerializeField]private TMP_Text mHealthPrice;
        [SerializeField]private TMP_Text mCreditsPrice;

        private PartOfDay currentPartOfDay;
        private Tuple<int, int> currentSmokeValue;

        private void Start()
        {
            currentPartOfDay = GameDirector.Instance.GetActiveGameProfile.GetProfileCalendar().GetCurrentPartOfDay;
            UpdatePopUpValues();
        }

        public override void ConfirmAction()
        {
            if (mDayTimeTarget == null)
            {
                return;
            }
            var dropDownVal = GetDropDownValue();
            DoCigarAction(dropDownVal);
            //. Pop up clear
            Debug.Log("[CigarConfirmationPopUp] Action confirmed");
        }

        private Tuple<int, int> GetSmokeCost(PartOfDay newPartOfDay)
        {
            var diff = newPartOfDay - currentPartOfDay;
            var price = diff * 1000;
            var health = (int)(diff * 1.5);
            if (health == 0)
                health++;
            return new Tuple<int, int>(price,health);
        }
        private async void DoCigarAction(PartOfDay newPartOfDay)
        {
            //2.FadeOut
            PopUpOperator.RemovePopUp(PopUpId);
            GameDirector.Instance.GetGeneralBackgroundFader.GeneralCurtainAppear();
            GameDirector.Instance.ChangeHighLvlGameState(HighLevelGameStates.InCutScene);
            //3. Play lighter sound
            await Task.Delay(2500);
            //4. Charge action cost
            GameDirector.Instance.GetActiveGameProfile.GetStatusModule().DoCigarAction(currentSmokeValue);
            GameDirector.Instance.GetUIController.UpdateInfoUI();
            //ManageProfileEvent
            //GameDirector.Instance.GetActiveGameProfile.OnSmokeAction();
            //6. Go to time of day from dropdown
            ClockManagement.Instance.SetClockAtDaytime(newPartOfDay);
            GameDirector.Instance.GetGeneralBackgroundFader.GeneralCurtainDisappear();
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
                case "End of Day":
                    newPartOfDay = PartOfDay.EndOfDay;
                    break;
            }
            if (newPartOfDay <= ClockManagement.Instance.GetCurrentPartOfDay())
            {
                return ClockManagement.Instance.GetCurrentPartOfDay();
            }
            return newPartOfDay;
        }

        public void OnDropdownChange()
        {
            UpdatePopUpValues();
        }

        private void UpdatePopUpValues()
        {
            var newPartOfDay = GetDropDownValue();
            currentSmokeValue = GetSmokeCost(newPartOfDay);
            mCreditsPrice.text = currentSmokeValue.Item1.ToString();
            mHealthPrice.text = currentSmokeValue.Item2.ToString();
        }
        public override void CancelAction()
        {
            GameDirector.Instance.GetClockInDayManagement.PlayPauseClock(true);
            PopUpOperator.RemovePopUp(PopUpId);
        }
    }
}