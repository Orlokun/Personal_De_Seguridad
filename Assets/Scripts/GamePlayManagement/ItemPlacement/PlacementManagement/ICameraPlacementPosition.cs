using System;

namespace GamePlayManagement.ItemPlacement.PlacementManagement
{
    public interface ICameraPlacementPosition : IBasePlacementPosition
    {
        public string PositionName { get; }
        public Guid Id { get; }
        public bool IsOccupied { get; }
    }
}