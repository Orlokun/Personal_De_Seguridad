using DataUnits.ItemScriptableObjects;
using UnityEngine;

namespace GamePlayManagement.ItemPlacement
{
    public interface IBasePlacementManager
    {
        public void AttachNewObject(IItemObject itemData, GameObject newObject);
        public void ToggleRoofObject(bool isActive);
        public bool IsPlacingObject { get; }
    }
}