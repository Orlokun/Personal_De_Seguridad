using DataUnits.JobSources;
using GameDirection.NewsManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PopUpManager.InfoPanelPopUp
{
    public class BaseSupplierInfoPanel : PopUpObject, ISupplierInfoPanel
    {
        [SerializeField] protected Image supplierImage;
        [SerializeField] protected TMP_Text supplierStoreName;
        [SerializeField] protected TMP_Text supplierPhoneNumber;
        [SerializeField] protected TMP_Text supplierName;
        [SerializeField] protected TMP_Text supplierAge;
        [SerializeField] protected TMP_Text supplierDescription;
        [SerializeField] protected Button closeButton;
        
        protected ISupplierBaseObject _mSupplierObject;

        protected void Awake()
        {
            closeButton.onClick.AddListener(ClosePanel);
        }

        public virtual void SetAndDisplayInfoPanelData(ISupplierBaseObject supplierObject)
        {
            _mSupplierObject = supplierObject;
            supplierStoreName.text = _mSupplierObject.StoreName;
            supplierPhoneNumber.text = _mSupplierObject.StorePhoneNumber.ToString();
        }
        private void ClosePanel()
        {
            PopUpOperator.RemovePopUp(PopUpId);
        }
    }
}