using DataUnits.ItemScriptableObjects;
using UnityEngine.UI;

namespace GamePlayManagement.ItemPlacement.PlacementManagement
{
    public interface IBaseItemPlacementManager
    {
        public void OnItemClicked(IItemObject itemData);
        public Button InstantiatingButton { get; }
    }
}