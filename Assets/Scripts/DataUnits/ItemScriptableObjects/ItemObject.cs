using System;
using GamePlayManagement.BitDescriptions.Suppliers;
using UI;
using UnityEngine;
using Utils;

namespace DataUnits.ItemScriptableObjects
{
    [Serializable]
    [CreateAssetMenu(menuName = "Items/BaseItem")]
    public class ItemObject : ScriptableObject, IItemObject, IInitialize
    {
        protected BitItemSupplier _itemSupplier;
        protected BitItemType _itemType;
        protected int _bitId;
        protected string _itemName;
        protected int _mItemUnlockPoints;
        protected int _mItemCost;
        
        protected Sprite _itemIcon;
        protected bool _mInitialized;
        
        protected int quantityAvailable;
        [SerializeField] protected float price;

        public BitItemType ItemType
        {
            get { return _itemType; }
        }
        public BitItemSupplier ItemSupplier => _itemSupplier;
        public int BitId => _bitId;
        public string ItemName => _itemName;
        public int UnlockPoints => _mItemUnlockPoints;
        public float Price => price;
        public Sprite ItemIcon =>_itemIcon;
        
        public void SetItemObjectData(BitItemSupplier itemSupplier,BitItemType itemType, int bitId, string itemName, int itemUnlockPoints, int itemCost)
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
        }

        public bool IsInitialized => _mInitialized;
        public void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}