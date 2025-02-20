using TMPro;
using UnityEngine;

namespace UI.PopUpManager.InfoPanelPopUp
{
    public class TrustIRewardDetailPrefab : RequirementIRewardDetailPrefab, ITrustRewardObjectPrefab
    {
        [SerializeField] private TMP_Text trusteeName;
        public void SetTrusteeName(string name)
        {
            trusteeName.text = name;
        }
    }
}