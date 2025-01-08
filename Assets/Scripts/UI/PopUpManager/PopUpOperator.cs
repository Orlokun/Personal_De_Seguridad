using System.Collections.Generic;
using GameDirection.NewsManagement;
using UI.PopUpManager.InfoPanelPopUp;
using UI.PopUpManager.OfficeRelatedPopUps;
using UI.TabManagement.NBVerticalTabs;
using UnityEngine;

namespace UI.PopUpManager
{
    public enum BitPopUpId
    {
        JOB_SUPPLIER_INFO_POPUP = 1,
        LARGE_HORIZONTAL_BANNER = 2,
        CIGAR_CONFIRMATION_POPUP = 4,
        GUARD_ITEM_INFO_PANEL = 8,
        CAMERA_ITEM_INFO_PANEL = 16,
        WEAPON_ITEM_INFO_PANEL = 32,
        TRAP_ITEM_INFO_PANEL = 64,
        OTHER_ITEM_INFO_PANEL = 128,
        ITEM_SUPPLIER_INFO_PANEL = 256,
        NEWS_DETAIL_POPUP = 512
    }

    public class PopUpOperator : MonoBehaviour,IPopUpOperator
    {
        private static IPopUpOperator _mInstance;
        public static IPopUpOperator Instance => _mInstance;

        private int _mActiveBitPopUps;
        private Dictionary<BitPopUpId, IPopUpObject> _activePopUps;
        private const string PopupPath = "UI/PopUps/";
        private void Awake()
        {
            DontDestroyOnLoad(this);
            _activePopUps = new Dictionary<BitPopUpId, IPopUpObject>();
            if (_mInstance != null)
            {
                Destroy(this);
                return;
            }
            _mInstance = this;
        }
        public bool IsPopupActive(BitPopUpId popUpId)
        {
            return ((int) popUpId & _mActiveBitPopUps) != 0;
        }

        public IPopUpObject GetActivePopUp(BitPopUpId popUpId)
        {
            if (!_activePopUps.ContainsKey(popUpId))
            {
                Debug.LogError("[GetActivePopUp]Pop Up Must Be Active Before Getting!");
            }
            return _activePopUps[popUpId];
        }

        public void TogglePopUpsActive(bool isActive)
        {
            
        }
        public IPopUpObject ActivatePopUp(BitPopUpId newPopUp)
        {
            if (IsPopupActive(newPopUp))
            {
                return GetActivePopUp(newPopUp);
            }
            _mActiveBitPopUps |= (int)newPopUp;
            var popUpInterface = InstantiatePopUp(newPopUp);
            popUpInterface.InitializePopUp(this, newPopUp);
            _activePopUps.Add(newPopUp, popUpInterface);
            return popUpInterface;
        }
        public void RemovePopUp(BitPopUpId removedPopUp)
        {
            if (((int) removedPopUp & _mActiveBitPopUps) == 0)
            {
                return;
            }
            _mActiveBitPopUps &= ~(int)removedPopUp;
            _activePopUps[removedPopUp].DeletePopUp();
            _activePopUps.Remove(removedPopUp);
        }
        public void RemoveAllPopUps()
        {
            foreach (var activePopUp in _activePopUps)
            {
                activePopUp.Value.DeletePopUp();
            }
            _activePopUps.Clear();
            _mActiveBitPopUps = 0;
        }
        public void RemoveAllPopUpsExceptOne(BitPopUpId exceptPopUp)
        {
            var deletedObjects = new List<BitPopUpId>();
            foreach (var activePopUp in _activePopUps)
            {
                if (activePopUp.Key != exceptPopUp)
                {
                    _mActiveBitPopUps &= ~(int) activePopUp.Key; 
                    activePopUp.Value.DeletePopUp();
                    deletedObjects.Add(activePopUp.Key);
                }
            }

            foreach (var deletedObject in deletedObjects)
            {
                if (_activePopUps.ContainsKey(deletedObject))
                {
                    _activePopUps.Remove(deletedObject);
                }
            }
            if((_mActiveBitPopUps & (int) exceptPopUp) != 0)
            {
                _mActiveBitPopUps = (int)exceptPopUp;
            }
            /*
            else
            {
                _mActiveBitPopUps = 0;
            }*/
        }

        private IPopUpObject InstantiatePopUp(BitPopUpId newPopUp)
        {
            switch (newPopUp)
            {
                case BitPopUpId.JOB_SUPPLIER_INFO_POPUP:
                    //const string pathString = PopupPath + "UI/PopUps/UI_SupplierUIActions";
                    var notebookOptions = (GameObject)Instantiate(Resources.Load("UI/PopUps/UI_ItemInfoPanel_JobSupplier"), transform);
                    return notebookOptions.GetComponent<JobSupplierInfoPanel>();
                case BitPopUpId.LARGE_HORIZONTAL_BANNER:
                    var bannerPrefab = (GameObject) Instantiate(Resources.Load("UI/PopUps/UI_LargeBannerObject"), transform);
                    return bannerPrefab.GetComponent<BannerObjectController>();
                case BitPopUpId.CIGAR_CONFIRMATION_POPUP:
                    var cigarConfirmationPopup = (GameObject) Instantiate(Resources.Load("UI/PopUps/UI_Office_CigarActionConfirmation"), transform);
                    return cigarConfirmationPopup.GetComponent<CigarConfirmationPopUp>();
                case BitPopUpId.GUARD_ITEM_INFO_PANEL:
                    var guardInfoPanelPopup = (GameObject) Instantiate(Resources.Load("UI/PopUps/UI_ItemInfoPanel_Guard"), transform);
                    return guardInfoPanelPopup.GetComponent<GuardItemInfoPanel>();
                case BitPopUpId.CAMERA_ITEM_INFO_PANEL:
                    var cameraInfoPanel = (GameObject) Instantiate(Resources.Load("UI/PopUps/UI_ItemInfoPanel_Camera"), transform);
                    return cameraInfoPanel.GetComponent<IItemInfoPanel>();
                case BitPopUpId.WEAPON_ITEM_INFO_PANEL:
                    var weaponInfoPanelPopup = (GameObject) Instantiate(Resources.Load("UI/PopUps/UI_ItemInfoPanel_Weapon"), transform);
                    return weaponInfoPanelPopup.GetComponent<IItemInfoPanel>();
                case BitPopUpId.TRAP_ITEM_INFO_PANEL:
                    var trapInfoPanelPopup = (GameObject) Instantiate(Resources.Load("UI/PopUps/UI_ItemInfoPanel_Trap"), transform);
                    return trapInfoPanelPopup.GetComponent<IItemInfoPanel>();
                case BitPopUpId.OTHER_ITEM_INFO_PANEL:
                    var otherItemInfoPanelPopup = (GameObject) Instantiate(Resources.Load("UI/PopUps/UI_ItemInfoPanel_Other"), transform);
                    return otherItemInfoPanelPopup.GetComponent<IItemInfoPanel>();
                case BitPopUpId.ITEM_SUPPLIER_INFO_PANEL:
                    var itemSupplierInfoPanelPopup = (GameObject) Instantiate(Resources.Load("UI/PopUps/UI_ItemInfoPanel_ItemSupplier"), transform);
                    return itemSupplierInfoPanelPopup.GetComponent<ItemSupplierInfoPanel>();
                case BitPopUpId.NEWS_DETAIL_POPUP:
                    var newsDetailPopup = (GameObject) Instantiate(Resources.Load("UI/PopUps/UI_NewsDetailPopUp"), transform);
                    return newsDetailPopup.GetComponent<NewsDetailPanel>();
                default:
                    return null;
            }
        }
    }
}