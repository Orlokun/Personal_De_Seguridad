using UnityEngine;

namespace UI.Items
{
    public interface IUITabItemObject
    {
        public GameObject GetPrefabObject { get; }
        public BitItemType GetItemType { get; }
    }
}