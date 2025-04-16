using GamePlayManagement.ItemManagement;

namespace GamePlayManagement.ItemPlacement.PlacementManagement
{
    public interface IWeaponPlacementPosition : IBasePlacementPosition
    {
        public IGuardItemObject GuardObject { get; }
    }
}