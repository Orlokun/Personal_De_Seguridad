using System.Globalization;
using DataUnits.ItemScriptableObjects;
using TMPro;
using UI.PopUpManager;
using UnityEngine;
using UnityEngine.UI;

public interface IItemInfoPanel : IPopUpObject
{
    public void SetAndDisplayInfoPanelData(IItemObject itemToDisplay);

}

public class ItemInfoPanelObject : PopUpObject, IItemInfoPanel
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemCost;
    [SerializeField] private TMP_Text itemActions;
    [SerializeField] private TMP_Text itemDescription;
    private IItemTypeStats _mItemStats;
    private IItemObject _mItemObject;
    
    public virtual void SetAndDisplayInfoPanelData(IItemObject itemToDisplay)
    {
        _mItemObject = itemToDisplay;
        itemName.text = _mItemObject.ItemName;
        itemCost.text = _mItemObject.Cost.ToString(CultureInfo.InvariantCulture);
        itemActions.text = _mItemObject.ItemActions.ToString(CultureInfo.InvariantCulture);
        itemDescription.text = _mItemObject.ItemDescription;
        itemImage.sprite = itemToDisplay.ItemIcon;
        _mItemStats = itemToDisplay.ItemStats;
    }
}
