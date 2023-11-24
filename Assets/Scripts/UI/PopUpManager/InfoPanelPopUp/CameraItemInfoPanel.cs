using DataUnits.ItemScriptableObjects;
using TMPro;
using UnityEngine;

namespace UI.PopUpManager.InfoPanelPopUp
{
    public class CameraItemInfoPanel : ItemInfoPanelObject
    {
        [SerializeField] private TMP_Text rangeObj;
        [SerializeField] private TMP_Text peopleInSightObj;
        [SerializeField] private TMP_Text clarityObject;
        [SerializeField] private TMP_Text persuasivenessObject;

        public override void SetAndDisplayInfoPanelData(IItemObject itemToDisplay)
        {
            base.SetAndDisplayInfoPanelData(itemToDisplay);
            var specialStats = itemToDisplay.ItemStats.GetStats();
            rangeObj.text = specialStats[0].ToString();
            peopleInSightObj.text = specialStats[1].ToString();
            clarityObject.text = specialStats[2].ToString();
            persuasivenessObject.text = specialStats[3].ToString();
        }
    }
}