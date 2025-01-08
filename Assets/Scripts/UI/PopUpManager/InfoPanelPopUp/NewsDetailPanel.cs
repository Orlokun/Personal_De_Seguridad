using GameDirection.NewsManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PopUpManager.InfoPanelPopUp
{
    public class NewsDetailPanel : PopUpObject, INewsDetailPanel
    {
        [SerializeField] private TMP_Text newsTitle;
        [SerializeField] private TMP_Text newsContent;
        [SerializeField] private Button closeButton;
        
        private INewsObject _mNewsObject;

        private void Awake()
        {
            closeButton.onClick.AddListener(ClosePanel);
        }

        public void SetAndDisplayNewsData(INewsObject newsObject)
        {
            _mNewsObject = newsObject;
            newsTitle.text = _mNewsObject.NewsTitle;
            newsContent.text = _mNewsObject.NewsContent;
        }

        private void ClosePanel()
        {
            PopUpOperator.RemovePopUp(PopUpId);
        }
    }
}