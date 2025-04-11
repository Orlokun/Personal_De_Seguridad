using System;
using System.Collections.Generic;
using System.Linq;
using DataUnits.ItemScriptableObjects;
using DataUnits.ItemSources;
using DataUnits.JobSources;
using GameDirection;
using GamePlayManagement.BitDescriptions;
using GamePlayManagement.ProfileDataModules.ItemSuppliers.Stores;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.PopUpManager.InfoPanelPopUp
{
    public class ItemSupplierInfoPanel : BaseSupplierInfoPanel, ISupplierShopPanel
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

        [SerializeField] private Button mClearCartButton;
        [SerializeField] private Button mConfirmCartButton;
        
        [SerializeField] private GameObject itemInStorePrefab;
        [SerializeField] private GameObject lockedItemInStorePrefab;
        
        private List<IUIItemInStoreObject> _mUIItemsInShop = new List<IUIItemInStoreObject>();
        
        private List<IItemObject> _mItemCart = new List<IItemObject>();
        

        private int _mAccumulatedCartPrice = 0;
        private IItemSupplierShop _mSupplierShop;
        public override void SetAndDisplayInfoPanelData(ISupplierBaseObject supplierData)
        {
            mClearCartButton.onClick.AddListener(RestartCart);
            mConfirmCartButton.onClick.AddListener(StartShopConfirmationProcess);
            base.SetAndDisplayInfoPanelData(supplierData);
            
            var itemSupplierObject = (IItemSupplierDataObject) supplierData;
            _mSupplierShop = itemSupplierObject.SupplierShop;
            supplierImage.sprite = IconsSpriteData.GetSpriteForItemIcon(itemSupplierObject.SpriteName);
            relianceObj.text = itemSupplierObject.Reliance.ToString();
            kindnessObj.text = itemSupplierObject.Kindness.ToString();
            qualityObj.text = itemSupplierObject.Quality.ToString();
            stockRefill.text = itemSupplierObject.RefillStockPeriod.ToString();
            totalPriceObj.text = _mAccumulatedCartPrice.ToString();
            supplierName.text = itemSupplierObject.StoreName;
            itemSupplierObject.SupplierShop.SaveTempLastStock();
            ProcessItemsPanel(_mSupplierShop);
        }

        private void StartShopConfirmationProcess()
        {
            if(_mAccumulatedCartPrice == 0) return;
            var isEmployed = GameDirector.Instance.GetActiveGameProfile.GetActiveJobsModule().CurrentEmployer != 0;
            if (!isEmployed)
            {
                ProcessUnemployedPurchase();
                return;
            }
            ProcessHiredPurchase();
        }

        private void ProcessHiredPurchase()
        {
            var currentBudget = GameDirector.Instance.GetActiveGameProfile.GetActiveJobsModule()
                .CurrentEmployerData().JobSupplierData.Budget;
            var currentPlayerCredits = GameDirector.Instance.GetActiveGameProfile.GetStatusModule().PlayerOmniCredits;
            if (_mAccumulatedCartPrice > currentBudget)
            {
                if (_mAccumulatedCartPrice - currentBudget <= currentPlayerCredits)
                {
                    var personalChoicePopUp = (IConfirmPersonalPurchasePopUp)PopUpOperator.ActivatePopUp(BitPopUpId.USE_PERSONAL_BUDGET);
                    var personalExpense = _mAccumulatedCartPrice - currentBudget;
                    personalChoicePopUp.SetLostValue(personalExpense);
                    personalChoicePopUp.OnPurchaseConfirmed += ReceivePurchaseConfirmation;
                    return;
                }
                PopUpOperator.ActivatePopUp(BitPopUpId.NOT_ENOUGH_CREDIT);
                return;
            }
            DoPurchaseProcess();
        }
        public void ReceivePurchaseConfirmation()
        {
            DoPurchaseProcess();
        }

        private void DoPurchaseProcess()
        {
            _mSupplierShop.ConfirmPurchase();
            if (GameDirector.Instance.GetActiveGameProfile.GetActiveJobsModule().CurrentEmployer != 0)
            {
                ProcessPurchaseWithEmployerBudget();
                RestartCart();         
                UIController.Instance.UpdateInfoUI();
                return;
            }
            ProcessNoJobPurchase();
            RestartCart();         
            UIController.Instance.UpdateInfoUI();
        }

        private void ProcessNoJobPurchase()
        {
            var playerProfile = GameDirector.Instance.GetActiveGameProfile;
            var inventoryModule = playerProfile.GetInventoryModule();
            var statusModule = playerProfile.GetStatusModule();
            foreach (var itemObject in _mItemCart)
            {
                inventoryModule.AddItemToInventory(itemObject, 1);
                statusModule.ReceiveOmniCredits(-itemObject.Cost);
            }
        }

        private void ProcessPurchaseWithEmployerBudget()
        {
            var playerProfile = GameDirector.Instance.GetActiveGameProfile;
            var inventoryModule = playerProfile.GetInventoryModule();
            var statusModule = playerProfile.GetStatusModule();
            var employerData = playerProfile.GetActiveJobsModule().CurrentEmployerData();
            
            var purchaseDelta = 0;
            var isUsingPlayerBudget = false;
            foreach (var itemObject in _mItemCart)
            {
                if (!isUsingPlayerBudget)
                {
                    var currentBudget = employerData.JobSupplierData.Budget;
                    if(currentBudget - itemObject.Cost < 0)
                    {
                        purchaseDelta = Math.Abs(currentBudget - itemObject.Cost);
                        employerData.JobSupplierData.Budget = 0;
                        statusModule.ReceiveOmniCredits(-purchaseDelta);
                        isUsingPlayerBudget = true;
                        inventoryModule.AddItemToInventory(itemObject, 1);
                        continue;   
                    }
                    inventoryModule.AddItemToInventory(itemObject, 1);
                    employerData.ExpendMoney(itemObject.Cost);
                    continue;
                }
                inventoryModule.AddItemToInventory(itemObject, 1);
                statusModule.ReceiveOmniCredits(-itemObject.Cost);
            }
        }

        void ProcessUnemployedPurchase()
        {
            var playerBudget = GameDirector.Instance.GetActiveGameProfile.GetStatusModule().PlayerOmniCredits;
            if (_mAccumulatedCartPrice > playerBudget)
            {
                PopUpOperator.ActivatePopUp(BitPopUpId.NOT_ENOUGH_CREDIT);
                return;
            }
            var personalChoicePopUp = (IConfirmPersonalPurchasePopUp)PopUpOperator.ActivatePopUp(BitPopUpId.USE_PERSONAL_BUDGET);
            personalChoicePopUp.SetLostValue(_mAccumulatedCartPrice);
            personalChoicePopUp.OnPurchaseConfirmed += ReceivePurchaseConfirmation;
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
                        guardController.Initialize(this, item.GetItemData);
                        _mUIItemsInShop.Add(guardController);
                        break;
                    case BitItemType.CAMERA_ITEM_TYPE:
                        var cameraObject = item.IsLocked ? Instantiate(lockedItemInStorePrefab, camerasPanel) : Instantiate(itemInStorePrefab, camerasPanel);
                        if(item.IsLocked) break;
                        var cameraObjectController = cameraObject.GetComponent<UIItemInStoreObject>();
                        cameraObjectController.Initialize(this, item.GetItemData);
                        _mUIItemsInShop.Add(cameraObjectController);
                        break;
                    case BitItemType.WEAPON_ITEM_TYPE:
                        var weaponObject = item.IsLocked ? Instantiate(lockedItemInStorePrefab, weaponsPanel) : Instantiate(itemInStorePrefab, weaponsPanel);
                        if(item.IsLocked) break;
                        var weaponObjectController = weaponObject.GetComponent<UIItemInStoreObject>();
                        weaponObjectController.Initialize(this, item.GetItemData);
                        _mUIItemsInShop.Add(weaponObjectController);
                        break;
                    case BitItemType.TRAP_ITEM_TYPE:
                        var trapObject = item.IsLocked ? Instantiate(lockedItemInStorePrefab, trapsPanel) : Instantiate(itemInStorePrefab, trapsPanel);
                        if(item.IsLocked) break;
                        var trapObjectController = trapObject.GetComponent<UIItemInStoreObject>();
                        trapObjectController.Initialize(this, item.GetItemData);
                        _mUIItemsInShop.Add(trapObjectController);
                        break;
                    case BitItemType.SPECIAL_ITEM_TYPE:
                        var otherObject = item.IsLocked ? Instantiate(lockedItemInStorePrefab, othersPanel) : Instantiate(itemInStorePrefab, othersPanel);
                        if(item.IsLocked) break;
                        var otherObjectController = otherObject.GetComponent<UIItemInStoreObject>();
                        otherObjectController.Initialize(this, item.GetItemData);
                        _mUIItemsInShop.Add(otherObjectController);
                        break;
                    default:
                        return;
                }
            }
        }

        public void AttemptAddToCart(int itemId)
        {
            if(_mSupplierShop.GetSupplierItemInStore(itemId).GetCurrentAmount <= 0) 
                return;

            var item = _mSupplierShop.GetItemObject(itemId);
            _mItemCart.Add(item);
            
            var itemInStore = _mSupplierShop.GetSupplierItemInStore(itemId);
            itemInStore.ReduceStock(1);
            _mAccumulatedCartPrice += itemInStore.GetItemData.Cost;
            totalPriceObj.text = _mAccumulatedCartPrice.ToString();
        }
        

        public void AttemptRemoveFromCart(int itemId)
        {
            if (_mSupplierShop.GetSupplierItemInStore(itemId).GetCurrentAmount >=
                _mSupplierShop.GetItemTempLiveStock(itemId))
            {
                return;
            }
            if(_mItemCart.Count == 0) return;
            if(_mItemCart.All(x => x.BitId != itemId))
            {
                return;
            }
            var item = _mItemCart.First(x => x.BitId == itemId);
            _mItemCart.Remove(item);
            var itemInStore = _mSupplierShop.GetSupplierItemInStore(itemId);
            itemInStore.AddStock(1);
            _mAccumulatedCartPrice -= itemInStore.GetItemData.Cost;
            totalPriceObj.text = _mAccumulatedCartPrice.ToString();
        }

        private void RestartCart()
        {
            var shopItems = _mSupplierShop.GetAllSupplierItems;
            foreach (var item in shopItems)
            {
                item.SetStock(_mSupplierShop.GetItemTempLiveStock(item.GetItemData.BitId));
            }
            _mAccumulatedCartPrice = 0;
            _mItemCart.Clear();
            totalPriceObj.text = _mAccumulatedCartPrice.ToString();
            _mUIItemsInShop.ForEach(x => x.UpdateAmountLeft());
        }

        public int GetCurrentItemStock(int itemId)
        {
            if (_mSupplierShop.GetSupplierItemInStore(itemId).IsLocked)
            {
                return 0;
            }
            return _mSupplierShop.GetSupplierItemInStore(itemId).GetCurrentAmount;
        }
    }
}