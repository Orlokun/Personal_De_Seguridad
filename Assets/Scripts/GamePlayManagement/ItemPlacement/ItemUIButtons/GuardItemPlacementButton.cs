using DataUnits.ItemScriptableObjects;
using GamePlayManagement.ItemPlacement.PlacementManagement;

namespace GamePlayManagement.ItemPlacement.ItemUIButtons
{
    public class GuardItemPlacementButton : BaseItemPlacement
    {
        public override void OnItemClicked(IItemObject itemData)
        {
            base.OnItemClicked(itemData);
            FloorPlacementManager.Instance.AttachNewObject(itemData, MInstantiatedObject);    
            FloorPlacementManager.Instance.ToggleRoofObject(false);    
        }
    }
}