namespace UI.PopUpManager
{
    public interface IPopUpObject
    {
        public void InitializePopUp(IPopUpOperator popUpOperator, BitPopUpId popUpId);
        void DeletePopUp();
        void SetPopUpActive(bool isActive);
    }
}