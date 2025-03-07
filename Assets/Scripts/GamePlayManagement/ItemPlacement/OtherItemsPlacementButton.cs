using DataUnits.ItemScriptableObjects;
using GamePlayManagement.ItemPlacement.PlacementManagement;

namespace GamePlayManagement.ItemPlacement
{
    public class OtherItemsPlacementButton : BaseItemPlacement
    {
        //TODO: Each item type must have its own position finder algorithm. 
        //For now, floor placement logic will be shared between guards, traps. Other will use the weapon logic for now.  
        public override void OnItemClicked(IItemObject itemData)
        {
            base.OnItemClicked(itemData);
            WeaponPlacementManager.Instance.AttachNewObject(itemData, MInstantiatedObject);    
            WeaponPlacementManager.Instance.ToggleRoofObject(false);    
        }
    }
}