using GamePlayManagement;
using GamePlayManagement.BitDescriptions;
using GamePlayManagement.Inventory;
using GamePlayManagement.ItemPlacement.PlacementManagement;
using TMPro;
using UI.PopUpManager;
using UI.PopUpManager.InfoPanelPopUp;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.TabManagement.ItemTypeTab
{
    public class BaseItemIconUIObject : MonoBehaviour, IInitializeWithArg1<IItemInInventory>
    {
        [SerializeField] protected TMP_Text countAvailable;
        [SerializeField] protected TMP_Text itemNameTextObject;
        [SerializeField] protected Image iconImage;
        [SerializeField] private Button MInfoPanelButton;

        private BaseItemPlacement _mPlacement;

        protected IItemInInventory MItemObject;
        protected bool MInitialized;
        public bool IsInitialized => MInitialized;

        public virtual void Initialize(IItemInInventory injectionClass)
        {
            if (MInitialized)
            {
                return;
            }
            MItemObject = injectionClass;
            SetObjectValuesInUI();
            MInfoPanelButton.onClick.AddListener(OpenItemInfoPanel);
            GetItemPlacementComponent();
            MInitialized = true;
        }

        private void GetItemPlacementComponent()
        {
            var hasPlacementManager = TryGetComponent<BaseItemPlacement>(out _mPlacement);
            if (!hasPlacementManager)
            {
                Debug.LogWarning("Item UI Object must have a placement component available");
            }
            var instButton = _mPlacement.InstantiatingButton;
            instButton.onClick.AddListener(OnItemClicked);
        }

        private void OnItemClicked()
        {

            Debug.Log($"[BaseItemIconUIObject.OnItemClicked] Item named {MItemObject.ItemName} was clicked");
            _mPlacement.OnItemClicked(MItemObject.ItemData);
        }

        
        private void SetObjectValuesInUI()
        {
            countAvailable.text = MItemObject.AvailableCount.ToString();
            itemNameTextObject.text = MItemObject.ItemName;
            iconImage.sprite = MItemObject.ItemData.ItemIcon;
        }
    
        public void OpenItemInfoPanel()
        {
            Debug.Log("[OpenItemInfoPanel]");
            var panelType = GetItemInfoPanel();
            var infoPanelPopUp = (IItemInfoPanel)PopUpOperator.Instance.ActivatePopUp(panelType);
            infoPanelPopUp.SetAndDisplayInfoPanelData(MItemObject.ItemData);
        }

        private BitPopUpId GetItemInfoPanel()
        {
            switch (MItemObject.ItemData.ItemType)
            {
                case BitItemType.GUARD_ITEM_TYPE:
                    return BitPopUpId.GUARD_ITEM_INFO_PANEL;
                case BitItemType.CAMERA_ITEM_TYPE:
                    return BitPopUpId.CAMERA_ITEM_INFO_PANEL;
                case BitItemType.WEAPON_ITEM_TYPE:
                    return BitPopUpId.WEAPON_ITEM_INFO_PANEL;
                case BitItemType.TRAP_ITEM_TYPE:
                    return BitPopUpId.TRAP_ITEM_INFO_PANEL;
                case BitItemType.OTHERS_ITEM_TYPE:
                    return BitPopUpId.OTHER_ITEM_INFO_PANEL;
            }
            return 0;
        }
    }
}