using DataUnits.ItemScriptableObjects;
using UI.PopUpManager;
using UnityEngine;
using UnityEngine.UI;

namespace ItemPlacement
{
    /// <summary>
    /// UI script to Instantiate the game object of the item that will be used by the player
    /// </summary>
    public class BaseSelectItemForPlacement : MonoBehaviour
    {
        protected GameObject MInstantiatedObject;
        [SerializeField] private Button MInstantiatingButton;

        protected void Awake()
        {
            if (MInstantiatingButton == null)
            {
                Debug.LogError("[SelectItemForPlacement] A button for Instantiating selected item must be set");
            }
            MInstantiatingButton.onClick.AddListener(OnItemClicked);
        }
        public virtual void OpenItemInfoPanel()
        {
            Debug.Log("[base.OpenItemInfoPanel]");
        }
        
        
        public virtual void OnItemClicked()
        {
            CheckBudgetRestrictions();
            Debug.Log("[base.OnPointerClick]");
            if (MInstantiatedObject.activeInHierarchy != true)
            {
                MInstantiatedObject.SetActive(true);
            }
        }

        protected virtual void CheckBudgetRestrictions()
        {
            
        }
    }
}