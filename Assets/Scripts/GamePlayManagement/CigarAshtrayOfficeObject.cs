using UI.PopUpManager;
using UnityEngine;

namespace GamePlayManagement
{
    public class CigarAshtrayOfficeObject : MonoBehaviour, IInteractiveClickableObject
    {
        public void SendClickObject()
        {
            if (PopUpOperator.Instance.IsPopupActive(BitPopUpId.CIGAR_CONFIRMATION_POPUP))
            {
                return;
            }
            PopUpOperator.Instance.ActivatePopUp(BitPopUpId.CIGAR_CONFIRMATION_POPUP);
        }

        private bool _mHasSnippet = false;
        public string GetSnippetText { get; }
        public bool HasSnippet => _mHasSnippet;
        public void DisplaySnippet()
        {
            throw new System.NotImplementedException();
        }
    }
}