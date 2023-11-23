using DataUnits.ItemScriptableObjects;
using TMPro;
using UnityEngine;

public class GuardItemInfoPanel : ItemInfoPanelObject
{
    [SerializeField] private TMP_Text IntelligenceObj;
    [SerializeField] private TMP_Text KindnessObj;
    [SerializeField] private TMP_Text ProactivityObj;
    [SerializeField] private TMP_Text AggressiveObj;
    [SerializeField] private TMP_Text StrengthObj;
    [SerializeField] private TMP_Text AgilityObj;

    public override void SetAndDisplayInfoPanelData(IItemObject itemToDisplay)
    {
        base.SetAndDisplayInfoPanelData(itemToDisplay);
        var specialStats = itemToDisplay.ItemStats.GetStats();
        IntelligenceObj.text = specialStats[0].ToString();
        KindnessObj.text = specialStats[1].ToString();
        ProactivityObj.text = specialStats[2].ToString();
        AggressiveObj.text = specialStats[3].ToString();
        StrengthObj.text = specialStats[4].ToString();
        AgilityObj.text = specialStats[5].ToString();
    }
}