using DataUnits.ItemScriptableObjects;
using GamePlayManagement.ItemPlacement.PlacementManagement;
using UnityEngine;

namespace GamePlayManagement.ItemPlacement
{
    public interface IBasePlacementManager
    {
        public void AttachNewObject(IItemObject itemData, GameObject newObject);
        public void ToggleRoofObject(bool isActive);
        public bool IsPlacingObject { get; }
        public event BasePlacementManager.PlacedItem OnItemPlaced;
    }
}