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
        [SerializeField] private GameObject lockedItemInStorePrefab;
        public override void SetAndDisplayInfoPanelData(ISupplierBaseObject supplierData)
        {
            base.SetAndDisplayInfoPanelData(supplierData);
            
            var itemSupplierObject = (IItemSupplierDataObject) supplierData;
            supplierImage.sprite = IconsSpriteData.GetSpriteForItemIcon(itemSupplierObject.SpriteName);
            relianceObj.text = itemSupplierObject.Reliance.ToString();
            kindnessObj.text = itemSupplierObject.Kindness.ToString();
            qualityObj.text = itemSupplierObject.Quality.ToString();
            stockRefill.text = itemSupplierObject.RefillStockPeriod.ToString();
            totalPriceObj.text = "0";
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
                         
                        var guardObject = item.IsLocked ? Instantiate(lockedItemInStorePrefab, guardsPanel) :Instantiate(itemInStorePrefab, guardsPanel);
                        var guardController = guardObject.GetComponent<UIItemInStoreObject>();
                        guardController.Initialize(item.GetItemData);
                        break;
                    case BitItemType.CAMERA_ITEM_TYPE:
                        var cameraObject = item.IsLocked ? Instantiate(lockedItemInStorePrefab, camerasPanel) : Instantiate(itemInStorePrefab, camerasPanel);
                        if(item.IsLocked) break;
                        var cameraObjectController = cameraObject.GetComponent<UIItemInStoreObject>();
                        cameraObjectController.Initialize(item.GetItemData);
                        break;
                    case BitItemType.WEAPON_ITEM_TYPE:
                        var weaponObject = item.IsLocked ? Instantiate(lockedItemInStorePrefab, weaponsPanel) : Instantiate(itemInStorePrefab, weaponsPanel);
                        if(item.IsLocked) break;
                        var weaponObjectController = weaponObject.GetComponent<UIItemInStoreObject>();
                        weaponObjectController.Initialize(item.GetItemData);
                        break;
                    case BitItemType.TRAP_ITEM_TYPE:
                        var trapObject = item.IsLocked ? Instantiate(lockedItemInStorePrefab, trapsPanel) : Instantiate(itemInStorePrefab, trapsPanel);
                        if(item.IsLocked) break;
                        var trapObjectController = trapObject.GetComponent<UIItemInStoreObject>();
                        trapObjectController.Initialize(item.GetItemData);
                        break;
                    case BitItemType.OTHERS_ITEM_TYPE:
                        var otherObject = item.IsLocked ? Instantiate(lockedItemInStorePrefab, othersPanel) : Instantiate(itemInStorePrefab, othersPanel);
                        if(item.IsLocked) break;
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