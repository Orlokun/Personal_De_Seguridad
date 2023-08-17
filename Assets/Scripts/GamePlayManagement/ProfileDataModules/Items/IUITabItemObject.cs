using UI;
using UnityEngine;

namespace GameManagement.ProfileDataModules.Items
{
    public interface IUITabItemObject
    {
        public GameObject GetPrefabObject { get; }
        public BitItemType GetItemType { get; }
    }
}