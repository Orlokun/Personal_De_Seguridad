using GamePlayManagement.BitDescriptions;
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
        public string ItemDescription { get; }
        public int UnlockPoints { get; }
        public float Cost { get; }
        public int ItemActions { get; }
        public Sprite ItemIcon{ get; }
        public IItemTypeStats ItemStats {get;}
        public void SetItemObjectData(BitItemSupplier itemSupplier, BitItemType itemType, int bitId, string itemName,
            int itemUnlockPoints, int itemCost, string itemDescription, string spriteIconName, int itemActions);
        public void SetItemSpecialStats(IItemTypeStats itemStats);
    }
}