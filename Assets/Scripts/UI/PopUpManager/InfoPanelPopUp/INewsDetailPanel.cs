using GameDirection.NewsManagement;

namespace UI.PopUpManager.InfoPanelPopUp
{
    public interface INewsDetailPanel
    {
        void SetAndDisplayNewsData(INewsObject newsObject);
    }
}