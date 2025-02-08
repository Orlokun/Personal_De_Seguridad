using CameraManagement;
using DataUnits.JobSources;
using TMPro;
using UI.PopUpManager;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TabManagement.NotebookTabs
{
    public class NotebookSupplierObject : MonoBehaviour
    {
        [SerializeField] private TMP_Text mStoreName;
        [SerializeField] private TMP_Text mStorePhone;

        [SerializeField] private Button callButton;
        [SerializeField] private Button infoButton;
        
        protected IPopUpOperator PopUpOperator;
        
        private void Awake()
        {
            PopUpOperator = PopUpManager.PopUpOperator.Instance;
            infoButton.onClick.AddListener(OpenInfoPopUp);
            callButton.onClick.AddListener(CallSupplier);
        }
        public virtual void SetNotebookObjectValues(ISupplierBaseObject supplierData)
        {
            mStoreName.text = supplierData.StoreName;
            mStorePhone.text = supplierData.StorePhoneNumber.ToString();
        }

        protected virtual void CallSupplier()
        {
            Debug.Log("Start Calling supplier");
            UIController.Instance.UpdateOfficeUIElement((int)OfficeCameraStates.Phone);
            GameCameraOperator.Instance.ActivateNewCamera(GameCameraState.Office, (int)OfficeCameraStates.Phone);
            PopUpManager.PopUpOperator.Instance.RemoveAllPopUps();
        }
        public virtual void OpenInfoPopUp()
        {
            
        }
    }

}