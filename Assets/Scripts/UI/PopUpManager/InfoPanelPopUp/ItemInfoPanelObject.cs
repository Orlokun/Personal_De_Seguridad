using System.Globalization;
using DataUnits.ItemScriptableObjects;
using DataUnits.JobSources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PopUpManager.InfoPanelPopUp
{
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
        [SerializeField] private Button mCloseInfoPopUpButton;
    
        private IItemObject _mItemObject;
    
    
        public virtual void SetAndDisplayInfoPanelData(IItemObject itemToDisplay)
        {
            _mItemObject = itemToDisplay;
            itemName.text = _mItemObject.ItemName;
            itemCost.text = _mItemObject.Cost.ToString(CultureInfo.InvariantCulture);
            itemActions.text = _mItemObject.ItemAmount.ToString(CultureInfo.InvariantCulture);
            itemDescription.text = _mItemObject.ItemDescription;
            itemImage.sprite = itemToDisplay.ItemIcon;
            mCloseInfoPopUpButton.onClick.AddListener(ClosePanel);
        }
        private void ClosePanel()
        {
            PopUpOperator.RemovePopUp(PopUpId);
        }
    }

    public interface ISupplierInfoPanel : IPopUpObject
    {
        public void SetAndDisplayInfoPanelData(ISupplierBaseObject supplierToDisplay);
    }
}