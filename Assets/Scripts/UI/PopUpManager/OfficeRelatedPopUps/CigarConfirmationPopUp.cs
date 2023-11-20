using System;
using GameDirection;
using UnityEngine;

namespace UI.PopUpManager.OfficeRelatedPopUps
{
    public class CigarConfirmationPopUp : ConfirmationPopUp
    {
        private void Start()
        {
            GameDirector.Instance.GetClockInDayManagement.PlayPauseClock(false);
        }

        public override void ConfirmAction()
        {
            //1. Get Value from Dropdown
            //2. UI fade out
            //3. Play lighter sound
            //4. Charge action cost
            //5. Go to time of day from dropdown
            //. Pop up clear
            Debug.Log("[CigarConfirmationPopUp] Action confirmed");
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