using System;
using UnityEngine;

namespace GamePlayManagement.ItemPlacement
{
    public class CameraPlacementPosition : ICameraPlacementPosition
    {
        private Guid _cameraPositionId;
        private Vector3 _cameraPosition;
        private bool _isOccupied;
        private string _positionName;
        public CameraPlacementPosition(Guid cameraPositionId, Vector3 cameraPosition, string positionName)
        {
            _cameraPositionId = cameraPositionId;
            _cameraPosition = cameraPosition;
            _isOccupied = false;
            _positionName = positionName;
        }

        public string PositionName => _positionName;
        public Guid Id => _cameraPositionId;
        public Vector3 ItemPosition => _cameraPosition;
        public bool IsOccupied => _isOccupied;
    }
}