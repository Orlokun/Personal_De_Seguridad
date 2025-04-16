using UnityEngine;

namespace GamePlayManagement.ItemPlacement.PlacementManagement
{
    public interface IItemCameraRotation : IToggleComponent
    {
        public void SetNewPosition(Vector3 newPosition);
    }
}