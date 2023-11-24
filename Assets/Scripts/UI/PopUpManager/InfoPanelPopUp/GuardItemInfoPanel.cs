using DataUnits.ItemScriptableObjects;
using TMPro;
using UnityEngine;

namespace UI.PopUpManager.InfoPanelPopUp
{
    public class GuardItemInfoPanel : ItemInfoPanelObject
    {
        [SerializeField] private TMP_Text intelligenceObj;
        [SerializeField] private TMP_Text kindnessObj;
        [SerializeField] private TMP_Text proactivityObj;
        [SerializeField] private TMP_Text aggressiveObj;
        [SerializeField] private TMP_Text strengthObj;
        [SerializeField] private TMP_Text agilityObj;

        public override void SetAndDisplayInfoPanelData(IItemObject itemToDisplay)
        {
            base.SetAndDisplayInfoPanelData(itemToDisplay);
            var specialStats = itemToDisplay.ItemStats.GetStats();
            intelligenceObj.text = specialStats[0].ToString();
            kindnessObj.text = specialStats[1].ToString();
            proactivityObj.text = specialStats[2].ToString();
            aggressiveObj.text = specialStats[3].ToString();
            strengthObj.text = specialStats[4].ToString();
            agilityObj.text = specialStats[5].ToString();
        }
    }
}