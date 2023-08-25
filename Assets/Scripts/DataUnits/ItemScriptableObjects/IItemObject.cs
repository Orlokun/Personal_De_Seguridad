using GamePlayManagement.BitDescriptions.Suppliers;
using UI;
using UnityEngine;

namespace DataUnits.ItemScriptableObjects
{
    public interface IItemObject
    {
        public BitItemType ItemType { get; }
        public BitItemSupplier ItemSupplier { get; }
        public int BitId { get; }
        public string ItemName { get; }
        public int QuantityAvailable { get; }
        public float Price { get; }
        public Sprite ItemIcon{ get; }

    }
}