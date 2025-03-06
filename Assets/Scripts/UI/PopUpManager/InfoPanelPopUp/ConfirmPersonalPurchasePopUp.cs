using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PopUpManager.InfoPanelPopUp
{
    public class ConfirmPersonalPurchasePopUp : PopUpObject, IConfirmPersonalPurchasePopUp
    {
        [SerializeField] private Button mConfirmButton;
        [SerializeField] private Button mCloseButton;
        [SerializeField] private Button mCancelButton;
        
        [SerializeField] private TMP_Text mPersonalExpense;
        
        public delegate void ConfirmPurchaseButtonPressed();
        public event ConfirmPurchaseButtonPressed OnPurchaseConfirmed;
        public void SetLostValue(int lostValue)
        {
            mPersonalExpense.text = lostValue.ToString();
        }

        private void Awake()
        {
            mConfirmButton.onClick.AddListener(ConfirmPurchase);
            mCloseButton.onClick.AddListener(ClosePopUp);
            mCancelButton.onClick.AddListener(ClosePopUp);
        }

        private void ClosePopUp()
        {
            PopUpOperator.RemovePopUp(BitPopUpId.USE_PERSONAL_BUDGET);
        }

        private void ConfirmPurchase()
        {
            OnPurchaseConfirmed.Invoke();
            ClosePopUp();
        }
    }
}