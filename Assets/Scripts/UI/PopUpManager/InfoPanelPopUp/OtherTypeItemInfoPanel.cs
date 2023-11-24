using DataUnits.ItemScriptableObjects;
using TMPro;
using UnityEngine;

namespace UI.PopUpManager.InfoPanelPopUp
{
    public class OtherTypeItemInfoPanel : ItemInfoPanelObject
    {
        [SerializeField] private TMP_Text effectivenessObject;
        [SerializeField] private TMP_Text damageObject;
        [SerializeField] private TMP_Text rangeObject;
        [SerializeField] private TMP_Text persuasivenessObject;

        public override void SetAndDisplayInfoPanelData(IItemObject itemToDisplay)
        {
            base.SetAndDisplayInfoPanelData(itemToDisplay);
            var specialStats = itemToDisplay.ItemStats.GetStats();
            effectivenessObject.text = specialStats[0].ToString();
            damageObject.text = specialStats[1].ToString();
            rangeObject.text = specialStats[2].ToString();
            persuasivenessObject.text = specialStats[3].ToString();
        }
    }
}