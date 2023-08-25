using GamePlayManagement.BitDescriptions.Suppliers;
using UI;
using UnityEngine;

namespace DataUnits.ItemScriptableObjects
{
    [CreateAssetMenu(menuName = "Items/BaseItem")]
    public class ItemObject : ScriptableObject, IItemObject
    {
        [SerializeField] protected BitItemSupplier _itemSupplier;
        [SerializeField] protected BitItemType _itemType;
        [SerializeField] protected Sprite _itemIcon;
        [SerializeField] protected string itemName;

        protected int _bitId;
        protected int quantityAvailable;
        [SerializeField] protected float price;

        public BitItemType ItemType => _itemType;
        public BitItemSupplier ItemSupplier => _itemSupplier;
        public int BitId => _bitId;
        public string ItemName => itemName;
        public int QuantityAvailable => quantityAvailable;
        public float Price => price;
        public Sprite ItemIcon =>_itemIcon;

    }
}