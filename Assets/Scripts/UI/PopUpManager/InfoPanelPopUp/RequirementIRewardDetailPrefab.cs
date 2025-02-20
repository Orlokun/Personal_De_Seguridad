using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PopUpManager.InfoPanelPopUp
{
    public class RequirementIRewardDetailPrefab : MonoBehaviour, IRewardObjectPrefab
    {
        [SerializeField] protected Image rewardIcon;
        [SerializeField] protected TMP_Text rewardAmount;
        public void SetImageIcon(Sprite icon)
        {
            rewardIcon.sprite = icon;
        }

        public void SetRewardAmount(int amount)
        {
            rewardAmount.text = amount.ToString();
        }
    }
}