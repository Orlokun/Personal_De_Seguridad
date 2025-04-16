using System.Collections.Generic;

namespace DataUnits.ItemScriptableObjects
{
    public interface IItemTypeStats
    {
        public List<int> GetStats();
        public List<ItemBaseRace> ItemTypes { get; }
        public ItemOrigin ItemOrigin { get; }
        public ItemBaseQuality ItemBaseQuality { get; }
    }
}