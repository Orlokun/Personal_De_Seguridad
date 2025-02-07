using DataUnits.GameRequests;
using GameDirection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PopUpManager.InfoPanelPopUp
{
    public interface IRequirementDetailPopUp
    {
        void SetAndDisplayRequirementInfo(IGameRequest newsObject);
    }
    public class RequirementDetailPopUp : PopUpObject, IRequirementDetailPopUp
    {
        private IGameRequest _mRequestData;
        [SerializeField] protected Image reqMainImage;
        [SerializeField] protected TMP_Text reqTitle;
        [SerializeField] protected TMP_Text reqOwner;
        [SerializeField] protected TMP_Text reqDescription;
        [SerializeField] protected Button closeButton;
        
        protected void Awake()
        {
            closeButton.onClick.AddListener(ClosePanel);
        }
        private void ClosePanel()
        {
            PopUpOperator.RemovePopUp(PopUpId);
        }

        public void SetAndDisplayRequirementInfo(IGameRequest request)
        {
            _mRequestData = request;
            
            var speakerData =  GameDirector.Instance.GetSpeakerData(request.RequesterSpeakerId);
            reqOwner.text = speakerData.SpeakerName;
            reqTitle.text = _mRequestData.ReqTitle;
            reqDescription.text = _mRequestData.ReqDescription;
        }
    }
}