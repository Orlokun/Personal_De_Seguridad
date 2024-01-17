using System;
using System.Linq;
using DataUnits.ItemScriptableObjects;
using GamePlayManagement.BitDescriptions;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlayManagement.ItemPlacement
{
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
            MInstantiatedObject = GetItemPrefab(itemData);
            if (MInstantiatedObject.activeInHierarchy != true)
            {
                MInstantiatedObject.SetActive(true);
            }
        }
        
        protected virtual GameObject GetItemPrefab(IItemObject itemData)
        {
            var itemPrefabPath = GetBaseItemPath(itemData.ItemType, itemData.PrefabName);
            var prefabObject = (GameObject) Instantiate(Resources.Load(itemPrefabPath));
            prefabObject.SetActive(false);
            return prefabObject;
        }

        protected const string GuardsPath = "InventoryItemsPrefabs/Guards/";
        protected const string CamsPath = "InventoryItemsPrefabs/Camera/";
        protected const string WeaponsPath = "InventoryItemsPrefabs/Wapons/";
        protected const string TrapsPath = "InventoryItemsPrefabs/Traps/";
        protected const string OtherItemsPath = "InventoryItemsPrefabs/Other/";
        protected string GetBaseItemPath(BitItemType itemType, string prefabName)
        {
            string basePath = "";
            switch (itemType)
            {
                case BitItemType.GUARD_ITEM_TYPE:
                    basePath = GuardsPath;
                    break;
                case BitItemType.CAMERA_ITEM_TYPE:
                    basePath = CamsPath;
                    break;
                case BitItemType.WEAPON_ITEM_TYPE:
                    basePath = WeaponsPath;
                    break;
                case BitItemType.TRAP_ITEM_TYPE:
                    basePath = TrapsPath;
                    break;
                case BitItemType.OTHERS_ITEM_TYPE:
                    basePath = OtherItemsPath;
                    break;
            }
            if (basePath.Length == 0)
            {
                Debug.LogError("[BaseItemPlacement.GetBaseItemPath] Path must exist");
                return "";
            }
            var path = string.Concat(basePath, prefabName);
            return path;
        }
    }
}