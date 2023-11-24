using DataUnits.ItemScriptableObjects;
using TMPro;
using UnityEngine;

namespace UI.PopUpManager.InfoPanelPopUp
{
    public class WeaponItemInfoPanel : ItemInfoPanelObject
    {
        [SerializeField] private TMP_Text qualityObj;
        [SerializeField] private TMP_Text damageObject;
        [SerializeField] private TMP_Text rangeObject;
        [SerializeField] private TMP_Text persuasivenessObject;

        public override void SetAndDisplayInfoPanelData(IItemObject itemToDisplay)
        {
            base.SetAndDisplayInfoPanelData(itemToDisplay);
            var specialStats = itemToDisplay.ItemStats.GetStats();
            qualityObj.text = specialStats[1].ToString();
            damageObject.text = specialStats[2].ToString();
            rangeObject.text = specialStats[3].ToString();
            persuasivenessObject.text = specialStats[4].ToString();
        }
    }
}