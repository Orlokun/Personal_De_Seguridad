using System;
using UI;
using UnityEngine;

namespace GameManagement.ProfileDataModules.Items
{
    [CreateAssetMenu(menuName = "Items/BaseItemObject")]
    public class BaseUITabItemObject : ScriptableObject, IUITabItemObject
    {
        #region Members
        [SerializeField] protected string itemName;
        [SerializeField] protected Sprite itemIcon;
        [SerializeField] protected Sprite itemLockedIcon;
        
        [SerializeField] protected BitItemType itemType;
        [SerializeField] protected GameObject prefabObject;
        #endregion
        
        #region Public Interface
        
        public GameObject GetPrefabObject => prefabObject;
        public BitItemType GetItemType => itemType;
        
        #endregion
        
        private Guid ItemId;

        protected void Awake()
        {
            ItemId = Guid.NewGuid();
        }
    }
    
    [CreateAssetMenu(menuName = "Items/BaseItemObject")]
    public class CameraUITabItemObject : BaseUITabItemObject
    {

    }
}