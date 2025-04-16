using UnityEngine;

namespace GamePlayManagement.ItemPlacement.PlacementManagement
{
    public class FloorPlacementPosition : IBasePlacementPosition
    {
        public Vector3 ItemPosition { get; }
        public FloorPlacementPosition(Vector3 cameraPosition)
        {
            ItemPosition = cameraPosition;
        }
    }
}