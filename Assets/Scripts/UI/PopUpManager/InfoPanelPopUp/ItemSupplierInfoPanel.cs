using DataUnits.ItemSources;
using DataUnits.JobSources;
using GamePlayManagement.BitDescriptions;
using GamePlayManagement.ProfileDataModules.ItemSuppliers.Stores;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace UI.PopUpManager.InfoPanelPopUp
{
    public class ItemSupplierInfoPanel : BaseSupplierInfoPanel
    {
        [SerializeField] private TMP_Text relianceObj;
        [SerializeField] private TMP_Text kindnessObj;
        [SerializeField] private TMP_Text qualityObj;
        [SerializeField] private TMP_Text totalPriceObj;
        [SerializeField] private TMP_Text stockRefill;

        [SerializeField] private Transform guardsPanel;
        [SerializeField] private Transform camerasPanel;
        [SerializeField] private Transform weaponsPanel;
        [SerializeField] private Transform trapsPanel;
        [SerializeField] private Transform othersPanel;
        
        [SerializeField] private GameObject itemInStorePrefab;
        public override void SetAndDisplayInfoPanelData(ISupplierBaseObject supplierData)
        {
            base.SetAndDisplayInfoPanelData(supplierData);
            
            var itemSupplierObject = (IItemSupplierDataObject) supplierData;
            supplierImage.sprite = IconsSpriteData.GetSpriteForItemIcon(itemSupplierObject.SpriteName);
            relianceObj.text = itemSupplierObject.Reliance.ToString();
            kindnessObj.text = itemSupplierObject.Kindness.ToString();
            qualityObj.text = itemSupplierObject.Quality.ToString();
            totalPriceObj.text = itemSupplierObject.RefillStockPeriod.ToString();
            supplierName.text = itemSupplierObject.StoreName;
            ProcessItemsPanel(itemSupplierObject.SupplierShop);
        }

        private void ProcessItemsPanel(IItemSupplierShop supplierShop)
        {
            foreach (var item in supplierShop.GetAllSupplierItems)
            {
                var itemType = item.GetItemType;
                switch (itemType)
                {
                    case BitItemType.GUARD_ITEM_TYPE:
                        var guardObject = Instantiate(itemInStorePrefab, guardsPanel);
                        var guardController = guardObject.GetComponent<UIItemInStoreObject>();
                        guardController.Initialize(item.GetItemData);
                        break;
                    case BitItemType.CAMERA_ITEM_TYPE:
                        var cameraObject = Instantiate(itemInStorePrefab, camerasPanel);
                        var cameraObjectController = cameraObject.GetComponent<UIItemInStoreObject>();
                        cameraObjectController.Initialize(item.GetItemData);
                        break;
                    case BitItemType.WEAPON_ITEM_TYPE:
                        var weaponObject = Instantiate(itemInStorePrefab, weaponsPanel);
                        var weaponObjectController = weaponObject.GetComponent<UIItemInStoreObject>();
                        weaponObjectController.Initialize(item.GetItemData);
                        break;
                    case BitItemType.TRAP_ITEM_TYPE:
                        var trapObject = Instantiate(itemInStorePrefab, trapsPanel);
                        var trapObjectController = trapObject.GetComponent<UIItemInStoreObject>();
                        trapObjectController.Initialize(item.GetItemData);
                        break;
                    case BitItemType.OTHERS_ITEM_TYPE:
                        var otherObject = Instantiate(itemInStorePrefab, othersPanel);
                        var otherObjectController = otherObject.GetComponent<UIItemInStoreObject>();
                        otherObjectController.Initialize(item.GetItemData);
                        break;
                    default:
                        return;
                }
            }
        }
    }
}