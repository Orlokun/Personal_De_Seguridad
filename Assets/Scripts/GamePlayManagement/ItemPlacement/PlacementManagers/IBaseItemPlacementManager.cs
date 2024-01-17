using DataUnits.ItemScriptableObjects;
using UnityEngine.UI;

namespace GamePlayManagement.ItemPlacement.PlacementManagers
{
    public interface IBaseItemPlacementManager
    {
        public void OnItemClicked(IItemObject itemData);
        public Button InstantiatingButton { get; }
    }
}