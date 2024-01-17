using GamePlayManagement.BitDescriptions;
using GamePlayManagement.BitDescriptions.Suppliers;
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
        public int Cost { get; }
        public int ItemActions { get; }
        public Sprite ItemIcon{ get; }
        public string PrefabName { get; }
        public IItemTypeStats ItemStats {get;}
        public void SetItemObjectData(BitItemSupplier itemSupplier, BitItemType itemType, int bitId, string itemName,
            int itemUnlockPoints, int itemCost, string itemDescription, string spriteIconName, int itemActions, string prefabName);
        public void SetItemSpecialStats(IItemTypeStats itemStats);
    }
}