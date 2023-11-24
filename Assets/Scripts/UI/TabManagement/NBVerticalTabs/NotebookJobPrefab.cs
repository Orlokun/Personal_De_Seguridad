using DataUnits;
using DataUnits.JobSources;
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
            mStoreName.text = supplierData.StoreName;
            mStorePhone.text = supplierData.StorePhoneNumber.ToString();
            _supplierId = supplierData.BitId;
        }
        
        public void OpenInfoPopUp()
        {
            var popUp = _popUpOperator.ActivatePopUp(BitPopUpId.JOB_SUPPLIER_INFO_POPUP);
            var casPopUp = (IItemSupplierNotebookButtonAction) popUp;
            casPopUp.SetSupplier((int)_supplierId, CallableObjectType.Job);
        }
    }
}