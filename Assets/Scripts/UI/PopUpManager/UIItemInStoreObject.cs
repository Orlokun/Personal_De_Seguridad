using DataUnits.ItemScriptableObjects;
using TMPro;
using UI.PopUpManager.InfoPanelPopUp;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.PopUpManager
{
    public class UIItemInStoreObject : MonoBehaviour, IInitializeWithArg2<ISupplierShopPanel,IItemObject>, IUIItemInStoreObject
    {
        private IItemObject _mItemObject;
        private ISupplierShopPanel _mShopPanel;

        [SerializeField] private Image mItemIcon;
    
        [SerializeField] private Button mMainButton;
        [SerializeField] private Button mAddToCart;
        [SerializeField] private Button mRemoveFromCart;
    
        [SerializeField] private TMP_Text mItemName;
        [SerializeField] private TMP_Text mItemMaxAmount;
        [SerializeField] private TMP_Text mItemAmount;
    
        public bool IsInitialized => _mInitialized;
        private bool _mInitialized;


        private void Awake()
        {
            mMainButton.onClick.AddListener(OnMainButtonClicked);
            mAddToCart.onClick.AddListener(OnAddButtonClicked);
            mRemoveFromCart.onClick.AddListener(OnRemoveButtonClicked);
        }

        private void OnRemoveButtonClicked()
        {
            _mShopPanel.AttemptRemoveFromCart(_mItemObject.BitId);
            UpdateAmountLeft();
        }

        private void OnAddButtonClicked()
        {
            _mShopPanel.AttemptAddToCart(_mItemObject.BitId);
            UpdateAmountLeft();
        }

        public void UpdateAmountLeft()
        {
            mItemAmount.text = _mShopPanel.GetCurrentItemStock(_mItemObject.BitId).ToString();
        }

        public void Initialize(ISupplierShopPanel injectionClass1,IItemObject injectionClass2)
        {
            _mShopPanel = injectionClass1;
            _mItemObject = injectionClass2;
            mItemName.text = _mItemObject.ItemName;
            mItemAmount.text = _mShopPanel.GetCurrentItemStock(_mItemObject.BitId).ToString();
            mItemMaxAmount.text = _mItemObject.ItemAmount.ToString();
            mItemIcon.sprite = _mItemObject.ItemIcon;
        }
        private void OnMainButtonClicked()
        {
            //Open Item Details Info Panel
        }

    }

    public interface IUIItemInStoreObject
    {
        void UpdateAmountLeft();
    }
}
