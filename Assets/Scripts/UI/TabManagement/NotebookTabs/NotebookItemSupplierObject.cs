using DataUnits.GameCatalogues;
using DataUnits.ItemSources;
using DataUnits.JobSources;
using DialogueSystem.Phone;
using GamePlayManagement.BitDescriptions.Suppliers;
using UI.PopUpManager;
using UI.PopUpManager.InfoPanelPopUp;
using UnityEngine;

namespace UI.TabManagement.NotebookTabs
{
    public class NotebookItemSupplierObject : NotebookSupplierObject
    {
        private IItemSupplierDataObject _itemSupplier;
        private BitItemSupplier _supplierId;

        public override void SetNotebookObjectValues(ISupplierBaseObject supplierData)
        {
            base.SetNotebookObjectValues(supplierData);
            _itemSupplier = (IItemSupplierDataObject)supplierData;
            _supplierId = _itemSupplier.ItemSupplierId;
        }
        public override void OpenInfoPopUp()
        {
            var popUp = PopUpOperator.ActivatePopUp(BitPopUpId.ITEM_SUPPLIER_INFO_PANEL);
            var infoPopUp = (ISupplierInfoPanel) popUp;
            infoPopUp.SetAndDisplayInfoPanelData(_itemSupplier);
        }

        protected override void CallSupplier()
        {
            base.CallSupplier();
            var supplierData = (ISupplierBaseObject) BaseItemSuppliersCatalogue.Instance.GetItemSupplierData(_supplierId);
            PhoneCallOperator.Instance.GoToCall(supplierData);
            Debug.Log($"Not Implemented. Supplier is: {_supplierId}");
        }
    }
}