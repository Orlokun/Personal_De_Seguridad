using UnityEngine;

namespace UI.PopUpManager.InfoPanelPopUp
{
    public interface IRewardObjectPrefab
    {
        void SetImageIcon(Sprite icon);
        void SetRewardAmount(int amount);
    }
}