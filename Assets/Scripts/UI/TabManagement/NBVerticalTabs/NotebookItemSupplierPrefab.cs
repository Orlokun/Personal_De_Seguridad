using GamePlayManagement.BitDescriptions.Suppliers;
using TMPro;
using UI.PopUpManager;
using UI.PopUpManager.NotebookScreen;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TabManagement.NBVerticalTabs
{
    internal class NotebookItemSupplierPrefab : MonoBehaviour
    {
        private BitItemSupplier _supplierId;
        [SerializeField] private TMP_Text mStoreName;
        [SerializeField] private TMP_Text mStorePhone;
        [SerializeField] private Image mStoreLogo;

        private IPopUpOperator _popUpOperator;
        
        private void Awake()
        {
            _popUpOperator = PopUpOperator.Instance;
        }

        public void SetSupplierPrefabValues(string storeName, string storePhone, Sprite storeLogo, BitItemSupplier supplierId)
        {
            mStoreName.text = storeName;
            mStorePhone.text = storePhone;
            mStoreLogo.sprite = storeLogo;
            _supplierId = supplierId;
        }

        public void OpenOptionsPopUp()
        {
            var popUp = _popUpOperator.ActivatePopUp(BitPopUpId.NotebookObjectAction);
            var casPopUp = (IItemSupplierNotebookButtonAction) popUp;
            casPopUp.SetSupplier((int)_supplierId, CallableObjectType.Item);
        }
    }
}