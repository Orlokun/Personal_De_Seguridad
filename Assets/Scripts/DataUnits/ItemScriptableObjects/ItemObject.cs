using System;
using GamePlayManagement.BitDescriptions;
using GamePlayManagement.BitDescriptions.Suppliers;
using UnityEngine;
using Utils;

namespace DataUnits.ItemScriptableObjects
{
    [Serializable]
    [CreateAssetMenu(menuName = "Items/BaseItem")]
    public class ItemObject : ScriptableObject, IItemObject, IInitialize, IBaseItemInformation
    {
        protected BitItemSupplier _itemSupplier;
        protected BitItemType _itemType;
        protected int _bitId;
        protected string _itemName;
        protected string _mItemDescription;
        protected int _mItemUnlockPoints;
        protected int _mItemCost;
        protected int _mItemAmount;
        protected string _prefabName;
        protected IItemTypeStats _mItemStats;
        
        protected Sprite _itemIcon;
        protected bool _mInitialized;
        
        public BitItemType ItemType
        {
            get { return _itemType; }
        }
        public BitItemSupplier ItemSupplier => _itemSupplier;
        public int BitId => _bitId;
        public string ItemName => _itemName;
        public string ItemDescription => _mItemDescription;
        public int UnlockPoints => _mItemUnlockPoints;
        public int Cost => _mItemCost;
        public Sprite ItemIcon =>_itemIcon;
        public int ItemAmount => _mItemAmount;
        public string PrefabName => _prefabName;
        public IItemTypeStats ItemStats => _mItemStats;
        
        public void SetItemObjectData(BitItemSupplier itemSupplier,BitItemType itemType, int bitId, string itemName, 
            int itemUnlockPoints, int itemCost, string itemDescription, string itemSpriteName, int itemStoreAmount, string prefabName)
        {
            if (IsInitialized)
            {
                return;
            }
            _itemSupplier = itemSupplier;
            _itemType = itemType;
            _bitId = bitId;
            _itemName = itemName;
            _mItemUnlockPoints = itemUnlockPoints;
            _mItemCost = itemCost;
            _mInitialized = true;
            _mItemDescription = itemDescription;
            _itemIcon = GetItemIcon(itemSpriteName);
            _mItemAmount = itemStoreAmount;
            _prefabName = prefabName;
        }

        public void SetItemSpecialStats(IItemTypeStats itemStats)
        {
            _mItemStats = itemStats;
        }

        private Sprite GetItemIcon(string spriteName)
        {
            return IconsSpriteData.GetSpriteForItemIcon(spriteName);
        }

        public bool IsInitialized => _mInitialized;
        public void Initialize()
        {
        }
    }
}