using UnityEngine;
using UnityEngine.UI;

namespace UI.PopUpManager.InfoPanelPopUp
{
    public class NotEnoughCreditsPopUp : PopUpObject
    {
        [SerializeField] private Button mOkButton;

        private void Awake()
        {
            mOkButton.onClick.AddListener(ClosePopUp);
        }

        private void ClosePopUp()
        {
            PopUpOperator.RemovePopUp(PopUpId);
        }
    }
}