using System.Collections.Generic;
using UI.PopUpManager.NotebookScreen;
using UnityEngine;

namespace UI.PopUpManager
{
    public enum BitPopUpId
    {
        NotebookObjectAction = 1
    }

    public class PopUpOperator : MonoBehaviour,IPopUpOperator
    {
        private static IPopUpOperator _mInstance;
        public static IPopUpOperator Instance => _mInstance;

        private int _mActiveBitPopUps;
        private Dictionary<BitPopUpId, IPopUpObject> _activePopUps;
        private const string PopupPath = "UI/PopUps/";
        private void Awake()
        {
            DontDestroyOnLoad(this);
            _activePopUps = new Dictionary<BitPopUpId, IPopUpObject>();
            if (_mInstance != null)
            {
                Destroy(this);
                return;
            }
            _mInstance = this;
        }
        public bool IsPopupActive(BitPopUpId popUpId)
        {
            return ((int) popUpId & _mActiveBitPopUps) != 0;
        }
        public void TogglePopUpsActive(bool isActive)
        {
            
        }
        public IPopUpObject ActivatePopUp(BitPopUpId newPopUp)
        {
            if (((int) newPopUp & _mActiveBitPopUps) != 0)
            {
                return null;
            }
            _mActiveBitPopUps |= (int)newPopUp;
            var popUpInterface = InstantiatePopUp(newPopUp);
            popUpInterface.InitializePopUp(this, newPopUp);
            _activePopUps.Add(newPopUp, popUpInterface);
            return popUpInterface;
        }
        
        public void RemovePopUp(BitPopUpId removedPopUp)
        {
            if (((int) removedPopUp & _mActiveBitPopUps) == 0)
            {
                return;
            }
            _mActiveBitPopUps &= ~(int)removedPopUp;
            _activePopUps[removedPopUp].DeletePopUp();
            _activePopUps.Remove(removedPopUp);
        }

        
        public void RemoveAllPopUps()
        {
            foreach (var activePopUp in _activePopUps)
            {
                activePopUp.Value.DeletePopUp();
            }
            _activePopUps.Clear();
            _mActiveBitPopUps = 0;
        }

        private IPopUpObject InstantiatePopUp(BitPopUpId newPopUp)
        {
            switch (newPopUp)
            {
                case BitPopUpId.NotebookObjectAction:
                    //const string pathString = PopupPath + "UI/PopUps/UI_SupplierUIActions";
                    var popUpPrefab = (GameObject)Instantiate(Resources.Load("UI/PopUps/UI_SupplierUIActions"), transform);
                    return popUpPrefab.GetComponent<SelectSupplierNotebookButtonAction>();
                default:
                    return null;
            }
        }
    }
}