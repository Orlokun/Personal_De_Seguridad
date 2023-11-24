using DataUnits.JobSources;
using TMPro;
using UnityEngine;
using Utils;

namespace UI.PopUpManager.InfoPanelPopUp
{
    public class JobSupplierInfoPanel : BaseSupplierInfoPanel
    {
        [SerializeField] private TMP_Text sanityObj;
        [SerializeField] private TMP_Text kindnessObj;
        [SerializeField] private TMP_Text violenceObj;
        [SerializeField] private TMP_Text intelligenceObj;
        [SerializeField] private TMP_Text moneyObj;
        public override void SetAndDisplayInfoPanelData(ISupplierBaseObject supplierData)
        {
            base.SetAndDisplayInfoPanelData(supplierData);
            var jobSupplierObject = (IJobSupplierObject) supplierData;
            sanityObj.text = jobSupplierObject.Sanity.ToString();
            kindnessObj.text = jobSupplierObject.Kindness.ToString();
            violenceObj.text = jobSupplierObject.Violence.ToString();
            intelligenceObj.text = jobSupplierObject.Intelligence.ToString();
            moneyObj.text = jobSupplierObject.Money.ToString();
            supplierDescription.text = jobSupplierObject.StoreDescription;
            supplierName.text = jobSupplierObject.StoreOwnerName;
            supplierImage.sprite = IconsSpriteData.GetSpriteForItemIcon(jobSupplierObject.SpriteName);
            supplierAge.text = jobSupplierObject.StoreOwnerAge.ToString();
        }
    }
}