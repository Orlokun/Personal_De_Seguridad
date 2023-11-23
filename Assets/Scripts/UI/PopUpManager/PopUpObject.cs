using UnityEngine;

namespace UI.PopUpManager
{
    public class PopUpObject : MonoBehaviour, IPopUpObject
    {
        protected IPopUpOperator PopUpOperator;
        protected BitPopUpId PopUpId;

        public void InitializePopUp(IPopUpOperator popUpOperator, BitPopUpId popUpId)
        {
            PopUpOperator = popUpOperator;
            PopUpId = popUpId;
        }

        public void DeletePopUp()
        {
            Destroy(gameObject);
        }

        public void SetPopUpActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}