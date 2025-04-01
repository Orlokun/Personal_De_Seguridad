namespace UI.PopUpManager
{
    public interface IBannerObjectController : IPopUpObject
    {
        public void ToggleBannerForSeconds(string newText, float seconds);
    }
}