using DataUnits.ItemSources;
using DataUnits.JobSources;
using TMPro;
using UnityEngine;
using Utils;

namespace UI.PopUpManager.InfoPanelPopUp
{
    public class ItemSupplierInfoPanel : BaseSupplierInfoPanel
    {
        [SerializeField] private TMP_Text relianceObj;
        [SerializeField] private TMP_Text kindnessObj;
        [SerializeField] private TMP_Text qualityObj;
        [SerializeField] private TMP_Text omniCreditsObj;
        public override void SetAndDisplayInfoPanelData(ISupplierBaseObject supplierData)
        {
            base.SetAndDisplayInfoPanelData(supplierData);
            var itemSupplierObject = (IItemSupplierDataObject) supplierData;
            supplierImage.sprite = IconsSpriteData.GetSpriteForItemIcon(itemSupplierObject.SpriteName);
            supplierDescription.text = itemSupplierObject.StoreDescription;
            relianceObj.text = itemSupplierObject.Reliance.ToString();
            kindnessObj.text = itemSupplierObject.Kindness.ToString();
            qualityObj.text = itemSupplierObject.Quality.ToString();
            omniCreditsObj.text = itemSupplierObject.OmniCredits.ToString();
            supplierName.text = itemSupplierObject.StoreName;
        }
    }
}