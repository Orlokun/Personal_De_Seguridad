using UnityEngine;

namespace InputManagement.MouseInput
{
    public interface IInteractiveClickableObject : ISnippetObject
    {
        public void ReceiveActionClickedEvent(RaycastHit hitInfo);
        public void ReceiveDeselectObjectEvent();

        public void ReceiveFirstClickEvent();
    }
}