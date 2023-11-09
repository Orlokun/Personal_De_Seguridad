using GamePlayManagement.BitDescriptions.Suppliers;
using UI;
using UnityEngine;

namespace DataUnits.ItemScriptableObjects
{
    public interface IItemObject
    {
        public BitItemType ItemType { get; }
        public BitItemSupplier ItemSupplier { get;}
        public int BitId { get;}
        public string ItemName { get; }
        public int UnlockPoints { get; }
        public float Price { get; }
        public Sprite ItemIcon{ get; }
        public void SetItemObjectData(BitItemSupplier itemSupplier, BitItemType itemType, int bitId, string itemName,
            int itemUnlockPoints, int itemCost);

    }
}