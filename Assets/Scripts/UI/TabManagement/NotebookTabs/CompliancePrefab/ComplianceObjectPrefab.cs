using GamePlayManagement.ComplianceSystem;
using TMPro;
using UI.PopUpManager;
using UI.PopUpManager.InfoPanelPopUp;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TabManagement.NotebookTabs.CompliancePrefab
{
    public class ComplianceObjectPrefab : MonoBehaviour, IComplianceObjectPrefab
    {
        private IComplianceObject _mComplianceData;
        [SerializeField] private TMP_Text _mTitle;
        [SerializeField] private TMP_Text _mSubTitle;
        [SerializeField] private Button _mOpenComplianceButton;

        private void Awake()
        {
            _mOpenComplianceButton.onClick.AddListener(OpenCompliancePopUp);
        }

        private void OpenCompliancePopUp()
        {
            var popUpObject = (IComplianceObjectDetailPopUp)PopUpOperator.Instance.ActivatePopUp(BitPopUpId.COMPLIANCE_DETAIL_POPUP);
            popUpObject.SetAndDisplayComplianceObjectInfo(_mComplianceData);
        }

        public void PopulateCompliancePrefab(IComplianceObject complianceObject)
        {
            _mComplianceData = complianceObject;
        }
    }
}