using GameDirection;
using GamePlayManagement.GameRequests;
using TMPro;
using UI.PopUpManager;
using UI.PopUpManager.InfoPanelPopUp;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TabManagement.NotebookTabs
{
    public class RequirementObjectPrefab : MonoBehaviour, IRequirementObjectPrefab
    {
        private IGameRequest _mRequirementData;
        [SerializeField] private TMP_Text _mRequester;
        [SerializeField] private TMP_Text _mTitle;
        [SerializeField] private TMP_Text _mSubTitle;
        [SerializeField] private Button _mOpenNewsButton;

        private void Awake()
        {
            _mOpenNewsButton.onClick.AddListener(OpenNewsPopUp);
        }

        private void OpenNewsPopUp()
        {
            var popUpObject = (IRequirementDetailPopUp)PopUpOperator.Instance.ActivatePopUp(BitPopUpId.REQUEST_DETAIL_POPUP);
            popUpObject.SetAndDisplayRequirementInfo(_mRequirementData);
        }

        public void PopulateRequestPrefab(IGameRequest requestObject)
        {
            _mRequirementData = requestObject;
            var speakerData =  GameDirector.Instance.GetSpeakerData(requestObject.RequesterSpeakerId);
            
            _mRequester.text = speakerData.SpeakerName;
            _mTitle.text = _mRequirementData.ReqTitle;
            _mSubTitle.text = _mRequirementData.ReqDescription;
        }
    }
}