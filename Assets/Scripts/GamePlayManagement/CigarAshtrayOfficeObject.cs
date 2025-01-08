using InputManagement;
using InputManagement.MouseInput;
using UI.PopUpManager;
using UnityEngine;

namespace GamePlayManagement
{
    public class CigarAshtrayOfficeObject : MonoBehaviour, IInteractiveClickableObject
    {
        #region Interactive Object Interface

        

        #endregion
        public void ReceiveActionClickedEvent()
        {
            throw new System.NotImplementedException();
        }

        public void ReceiveActionClickedEvent(RaycastHit hitInfo)
        {
            //Nothing to do, just deselect
        }

        public void ReceiveDeselectObjectEvent()
        {
            throw new System.NotImplementedException();
        }

        public void ReceiveClickEvent()
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