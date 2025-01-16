using GameDirection.NewsManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PopUpManager.InfoPanelPopUp
{
    public class NewsDetailPopUp : PopUpObject, INewsDetailPopUp
    {
        private INewsObject _mNewsObject;
        [SerializeField] protected Image newsMainImage;
        [SerializeField] protected TMP_Text newsTitle;
        [SerializeField] protected TMP_Text newsSubtitle;
        [SerializeField] protected TMP_Text newsContent;
        [SerializeField] protected Button closeButton;
        
        protected void Awake()
        {
            closeButton.onClick.AddListener(ClosePanel);
        }
        private void ClosePanel()
        {
            PopUpOperator.RemovePopUp(PopUpId);
        }

        public void SetAndDisplayNewsInfo(INewsObject newsObject)
        {
            _mNewsObject = newsObject;
            newsTitle.text = newsObject.NewsTitle;
            newsSubtitle.text = newsObject.NewsSubHead;
            newsContent.text = newsObject.NewsContent;
        }
    }

    public interface INewsDetailPopUp
    {
        void SetAndDisplayNewsInfo(INewsObject newsObject);
    }
}