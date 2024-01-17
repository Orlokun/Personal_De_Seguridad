using GamePlayManagement.ItemManagement;
using UnityEngine;

namespace GamePlayManagement.ItemPlacement
{
    public class WeaponPlacementPosition : IWeaponPlacementPosition
    {
        public Vector3 ItemPosition { get; }
        private IGuardItemObject _mGuardObject;
        public IGuardItemObject GuardObject => _mGuardObject;

        public WeaponPlacementPosition(IGuardItemObject guardObject)
        {
            _mGuardObject = guardObject;
            ItemPosition = guardObject.GuardData.GunParentTransform.position;
        }

    }
    public class FloorPlacementPosition : IBasePlacementPosition
    {
        public Vector3 ItemPosition { get; }
        public FloorPlacementPosition(Vector3 cameraPosition)
        {
            ItemPosition = cameraPosition;
        }
    }
}