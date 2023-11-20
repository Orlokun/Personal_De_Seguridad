using UI.PopUpManager;

namespace GamePlayManagement
{
    public class CigarAshtrayOfficeObject : OfficeInteractiveObject
    {
        public override void SendClickObject()
        {
            if (PopUpOperator.Instance.IsPopupActive(BitPopUpId.CIGAR_CONFIRMATION_POPUP))
            {
                return;
            }
            PopUpOperator.Instance.ActivatePopUp(BitPopUpId.CIGAR_CONFIRMATION_POPUP);
        }
    }
}