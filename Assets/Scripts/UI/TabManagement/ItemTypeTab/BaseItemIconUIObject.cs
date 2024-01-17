using System.Globalization;
using DataUnits.ItemScriptableObjects;
using GameDirection;
using GamePlayManagement.BitDescriptions;
using GamePlayManagement.ItemPlacement;
using TMPro;
using UI.PopUpManager;
using UI.PopUpManager.InfoPanelPopUp;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.TabManagement.ItemTypeTab
{
    public class BaseItemIconUIObject : MonoBehaviour, IInitializeWithArg1<IItemObject>
    {
        [SerializeField] protected TMP_Text costTextObject;
        [SerializeField] protected TMP_Text otherCostTextObject;
        [SerializeField] protected TMP_Text itemNameTextObject;
        [SerializeField] protected Image iconImage;
        [SerializeField] private Button MInfoPanelButton;

        private BaseItemPlacement _mPlacement;

        protected IItemObject MItemObject;
        protected bool MInitialized;
    
    
        public virtual void Initialize(IItemObject injectionClass)
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
            var currentBudget = GameDirector.Instance.GetActiveGameProfile.GetActiveJobsModule().CurrentEmployerData()
                .Budget;
            Debug.Log($"[BaseItemIconUIObject.OnItemClicked] Item named {MItemObject.ItemName} was clicked. " +
                      $"Current budget = {currentBudget}. Item Cost is: {MItemObject.Cost}");
            if (currentBudget >= MItemObject.Cost)
            {
                _mPlacement.OnItemClicked(MItemObject);
            }
        }

        
        private void SetObjectValuesInUI()
        {
            costTextObject.text = MItemObject.Cost.ToString(CultureInfo.InvariantCulture);
            otherCostTextObject.text = MItemObject.ItemActions.ToString(CultureInfo.InvariantCulture);
            itemNameTextObject.text = MItemObject.ItemName;
            iconImage.sprite = MItemObject.ItemIcon;
        }
    
        public void OpenItemInfoPanel()
        {
            Debug.Log("[OpenItemInfoPanel]");
            var panelType = GetItemInfoPanel();
            var infoPanelPopUp = (IItemInfoPanel)PopUpOperator.Instance.ActivatePopUp(panelType);
            infoPanelPopUp.SetAndDisplayInfoPanelData(MItemObject);
        }

        private BitPopUpId GetItemInfoPanel()
        {
            switch (MItemObject.ItemType)
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