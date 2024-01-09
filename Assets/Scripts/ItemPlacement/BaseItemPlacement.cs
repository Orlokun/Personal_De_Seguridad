using DataUnits.ItemScriptableObjects;
using GameDirection;
using UnityEngine;
using UnityEngine.UI;

namespace ItemPlacement
{
    public interface IBaseItemPlacementManager
    {
        public void OnItemClicked(IItemObject itemData);
        public Button InstantiatingButton { get; }
    }
    
    /// <summary>
    /// UI script to Instantiate the game object of the item that will be used by the player
    /// </summary>
    public class BaseItemPlacement : MonoBehaviour, IBaseItemPlacementManager
    {
        protected GameObject MInstantiatedObject;
        protected IItemObject MItemData;
        [SerializeField] private Button MInstantiatingButton;
        public Button InstantiatingButton => MInstantiatingButton;
        protected void Awake()
        {
            if (MInstantiatingButton == null)
            {
                Debug.LogError("[SelectItemForPlacement] A button for Instantiating selected item must be set");
            }
        }
        public virtual void OpenItemInfoPanel()
        {
            Debug.Log("[base.OpenItemInfoPanel]");
        }
        
        public virtual void OnItemClicked(IItemObject itemData)
        {
            Debug.Log("[base.OnPointerClick]");
            if (MInstantiatedObject.activeInHierarchy != true)
            {
                MInstantiatedObject.SetActive(true);
            }
        }

    }
}