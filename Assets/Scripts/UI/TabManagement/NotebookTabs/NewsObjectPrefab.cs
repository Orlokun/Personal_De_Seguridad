using GameDirection.NewsManagement;
using TMPro;
using UI.PopUpManager;
using UI.PopUpManager.InfoPanelPopUp;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TabManagement.NotebookTabs
{
    public interface INewsObjectPrefab
    {
        void PopulateNewsPrefab(INewsObject newsObject);
    }
    public class NewsObjectPrefab : MonoBehaviour, INewsObjectPrefab
    {
        private INewsObject _mNewsData;
        [SerializeField] private TMP_Text _mTitle;
        [SerializeField] private TMP_Text _mSubTitle;
        [SerializeField] private Button _mOpenNewsButton;

        private void Awake()
        {
            _mOpenNewsButton.onClick.AddListener(OpenNewsPopUp);
        }

        private void OpenNewsPopUp()
        {
            var popUpObject = (INewsDetailPopUp)PopUpOperator.Instance.ActivatePopUp(BitPopUpId.NEWS_DETAIL_POPUP);
            popUpObject.SetAndDisplayNewsInfo(_mNewsData);
        }

        public void PopulateNewsPrefab(INewsObject newsObject)
        {
            _mNewsData = newsObject;
            _mTitle.text = _mNewsData.NewsTitle;
            _mSubTitle.text = _mNewsData.NewsSubHead;
        }
    }
}