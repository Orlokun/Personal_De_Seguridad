using DataUnits.ItemScriptableObjects;
using GamePlayManagement.ItemPlacement.PlacementManagement;

namespace GamePlayManagement.ItemPlacement.ItemUIButtons
{
    public class CameraItemPlacementButton : BaseItemPlacement
    {
        private bool _mInitiliazed;
        //Prefabs that need to be instantiated
        public bool IsInitialized => _mInitiliazed;
        
        public override void OnItemClicked(IItemObject itemData)
        {
            base.OnItemClicked(itemData);
            CameraPlacementManager.MInstance.AttachNewObject(itemData, MInstantiatedObject);    
            CameraPlacementManager.MInstance.ToggleRoofObject(true);    
        }
    }
}