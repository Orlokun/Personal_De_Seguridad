using System.Collections.Generic;

namespace UI.PopUpManager
{
    public interface IPopUpOperator
    {
        public bool IsPopupActive(BitPopUpId popUpId);
        public IPopUpObject ActivatePopUp(BitPopUpId newPopUp);
        public void RemovePopUp(BitPopUpId removedPopUp);
        public void RemoveAllPopUps();
        public void TogglePopUpsActive(bool isActive);

        public void RemoveAllPopUpsExceptOne(BitPopUpId exception);
        public void RemoveAllPopUpsExceptList(List<BitPopUpId> exceptions);
        public IPopUpObject GetActivePopUp(BitPopUpId popUpId);

    }
}