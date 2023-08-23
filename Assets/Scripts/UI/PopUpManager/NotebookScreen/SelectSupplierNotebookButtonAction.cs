using CameraManagement;
using DataUnits;
using DataUnits.GameCatalogues;
using GamePlayManagement.BitDescriptions.Suppliers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.PopUpManager.NotebookScreen
{
    public class SelectSupplierNotebookButtonAction : PopUpObject, IPointerClickHandler, IItemSupplierNotebookButtonAction
    {
        private int _supplierId;
        private CallableObjectType _mCallableObjectType ;
        [SerializeField] private RectTransform BasePopUp; 

        public void SetSupplier(int selectedSupplier, CallableObjectType callableObjectType)
        {
            _mCallableObjectType = callableObjectType;
            _supplierId = selectedSupplier;
        }
        
        public void CallSupplierOption()
        {
            UIController.Instance.UpdateOfficeUIElement((int)OfficeCameraStates.Phone);
            GameCameraManager.Instance.ActivateNewCamera(GameCameraState.Office, (int)OfficeCameraStates.Phone);
            PopUpOperator.RemoveAllPopUps();
            var supplierData = _mCallableObjectType == CallableObjectType.Item
                ? (ISupplierBaseObject)BaseItemSuppliersCatalogue.Instance.GetItemSupplierData((BitItemSupplier) _supplierId)
                : (ISupplierBaseObject)BaseJobsCatalogue.Instance.GetJobSupplierObject((BitGameJobSuppliers) _supplierId);
            PhoneCallOperator.Instance.StartCallImmediately(supplierData);
            Debug.Log($"Not Implemented. Supplier is: {_supplierId}");
        }

        public void OpenSupplierOption()
        {
            Debug.Log($"Not Implemented. Supplier is: {_supplierId}");
        }
        
        protected override void InitializeValues()
        {
            
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            PopUpOperator.RemovePopUp(PopUpId);
        }
    }
}