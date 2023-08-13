using GameDirection;
using GameManagement.ProfileDataModules.ItemStores;
using UI.TabManagement.AbstractClasses;
using UnityEngine;
namespace UI.TabManagement
{
    public class ItemsTabGroup : TabGroup
    {
        [SerializeField] private BitItemType startType;
        [SerializeField] private Transform gridParentObject;
        [SerializeField] private Transform storeTitle;

        private IItemSourceModule _activeItemsModule;
        
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
            //UpdateSelectedItemsModule(startType);
        }

        public override bool ActivateTabInUI()
        {
            MTabActiveState = true;
            MuiController.ActivateObject(CanvasBitId.GamePlayCanvas,GameplayPanelsBitStates.ITEM_DETAILED_SIDEBAR);
            return MTabActiveState;
        }

        public override bool DeactivateGroupInUI()
        {
            MTabActiveState = false;
            MuiController.DeactivateObject(CanvasBitId.GamePlayCanvas,GameplayPanelsBitStates.ITEM_DETAILED_SIDEBAR);
            return MTabActiveState;
        }

        public override void UpdateTabGroupContent(int selectedTabIndex)
        {
            base.UpdateTabGroupContent(selectedTabIndex);
            if (gridParentObject.childCount > 0)
            {
                ClearGrid();
            }
            
            UpdateSelectedItemsModule((BitItemType) selectedTabIndex);
            var activeItems = _activeItemsModule.ActiveItemsInSource;
        }

        private void UpdateSelectedItemsModule(BitItemType index)
        {
            _activeItemsModule = GameDirector.Instance.GetActiveGameProfile.GetItemSourceWithIndex(index);
        }
        
        private void ClearGrid()
        {
            foreach (Transform itemObject in gridParentObject)
            {
                Destroy(itemObject.gameObject);
            }
        }
    }
}