using DataUnits;
using GamePlayManagement.BitDescriptions.Suppliers;
using TMPro;
using UI.PopUpManager;
using UI.PopUpManager.NotebookScreen;
using UnityEngine;

namespace UI.TabManagement.NBVerticalTabs
{
    internal class NotebookJobPrefab : MonoBehaviour
    {
        [SerializeField] private TMP_Text mStoreName;
        [SerializeField] private TMP_Text mStorePhone;
        private BitGameJobSuppliers _supplierId;

        private IPopUpOperator _popUpOperator;
        
        private void Awake()
        {
            _popUpOperator = PopUpOperator.Instance;
        }
        public void SetJobPrefabValues(IJobSupplierObject supplierData)
        {
            mStoreName.text = supplierData.SupplierName;
            mStorePhone.text = supplierData.SupplierNumber;
            _supplierId = supplierData.JobSupplier;
        }
        
        public void OpenOptionsPopUp()
        {
            var popUp = _popUpOperator.ActivatePopUp(BitPopUpId.NotebookObjectAction);
            var casPopUp = (IItemSupplierNotebookButtonAction) popUp;
            casPopUp.SetSupplier((int)_supplierId, CallableObjectType.Job);
        }
    }
}