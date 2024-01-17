using DataUnits.ItemScriptableObjects;
using GamePlayManagement.ItemPlacement.PlacementManagers;

namespace GamePlayManagement.ItemPlacement
{
    public class WeaponItemPlacementButton : BaseItemPlacement
    {
        public override void OnItemClicked(IItemObject itemData)
        {
            base.OnItemClicked(itemData);
            WeaponPlacementManager.Instance.AttachNewObject(itemData, MInstantiatedObject);    
            WeaponPlacementManager.Instance.ToggleRoofObject(false);    
        }
    }
}