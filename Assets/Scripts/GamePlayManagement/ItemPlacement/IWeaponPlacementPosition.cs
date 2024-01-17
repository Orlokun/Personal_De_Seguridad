using GamePlayManagement.ItemManagement;

namespace GamePlayManagement.ItemPlacement
{
    public interface IWeaponPlacementPosition : IBasePlacementPosition
    {
        public IGuardItemObject GuardObject { get; }
    }
}