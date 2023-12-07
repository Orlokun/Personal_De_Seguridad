using DataUnits.GameCatalogues;
using DataUnits.JobSources;
using GamePlayManagement.BitDescriptions.Suppliers;
using UI.PopUpManager;
using UI.PopUpManager.InfoPanelPopUp;
using UnityEngine;

namespace UI.TabManagement.NBVerticalTabs
{
    internal class NotebookJobSupplierObject : NotebookSupplierObject
    {
        private IJobSupplierObject _jobSupplier;
        private JobSupplierBitId _supplierId;

        public override void SetNotebookObjectValues(ISupplierBaseObject supplierData)
        {
            base.SetNotebookObjectValues(supplierData);
            _jobSupplier = (IJobSupplierObject)supplierData;
            _supplierId = _jobSupplier.JobSupplierBitId;
        }

        public override void OpenInfoPopUp()
        {
            var popUp = PopUpOperator.ActivatePopUp(BitPopUpId.JOB_SUPPLIER_INFO_POPUP);
            var infoPopUp = (ISupplierInfoPanel) popUp;
            infoPopUp.SetAndDisplayInfoPanelData(_jobSupplier);
        }

        protected override void CallSupplier()
        {
            base.CallSupplier();
            var supplierData = (ISupplierBaseObject) BaseJobsCatalogue.Instance.GetJobSupplierObject(_supplierId);
            PhoneCallOperator.Instance.GoToCall(supplierData);
            Debug.Log($"Not Implemented. Supplier is: {_supplierId}");
        }
    }
}