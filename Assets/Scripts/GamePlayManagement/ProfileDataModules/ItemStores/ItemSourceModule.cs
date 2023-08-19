using GamePlayManagement.ProfileDataModules.ItemStores;
using UnityEngine;

namespace GameManagement.ProfileDataModules.ItemStores
{
    public abstract class ItemSourceModule : IItemSourceModule
    {
        public ItemSourceModule()
        {
            
        }
        protected int MItemsActive = 0;
        protected const int MHighestBitItem = 16384;
        
        //Interfaces values implementations
        public int ElementsActive => MItemsActive;
        public bool IsModuleActive => MItemsActive > 0;
        public int ActiveItemsInSource => MItemsActive;
        public bool IsItemActive(int item)
        {
            return (MItemsActive & item) != 0;
        }
        public void ActivateNewItem(int newItem)
        {
            if ((MItemsActive & newItem) != 0)
            {
                Debug.Log("[ActivateNewItem] Item must not present in order to be added");
                return;
            }
            MItemsActive |= newItem;
        }
        public void RemoveItem(int newItem)
        {
            if ((MItemsActive & newItem) == 0)
            {
                Debug.Log("[ActivateNewItem] Item must be present in order to be removed");
                return;
            }
            MItemsActive &= ~newItem;
        }
    }
}