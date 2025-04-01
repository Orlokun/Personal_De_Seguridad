using UnityEngine;

namespace GamePlayManagement.ItemPlacement
{
    public interface IItemCameraRotation : IToggleComponent
    {
        public void SetNewPosition(Vector3 newPosition);
    }
}