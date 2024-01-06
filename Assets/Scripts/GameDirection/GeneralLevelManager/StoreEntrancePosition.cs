using UnityEngine;
namespace GameDirection.GeneralLevelManager
{
    public class StoreEntrancePosition : IStoreEntrancePosition
    {
        public Vector3 StartPosition => _mInstantiationPoistion.position;
        public Vector3 EntrancePosition => _mEntrancePosition.position;
        
        private Transform _mInstantiationPoistion;
        private Transform _mEntrancePosition;

        public StoreEntrancePosition(Transform instantiationPosition, Transform entrancePosition)
        {
            _mInstantiationPoistion = instantiationPosition;
            _mEntrancePosition = entrancePosition;
        }
    }
}