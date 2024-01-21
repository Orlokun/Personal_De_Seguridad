using System;
using UnityEngine;

namespace GameDirection.GeneralLevelManager.ShopPositions.WaitingPositions
{
    public class SingleWaitingSpot : MonoBehaviour, ISingleWaitingSpot
    {
        public bool IsInitialized => _mIsInitialized;
        private bool _mIsInitialized;
        
        public Guid SpotId => _mSpotId;
        private Guid _mSpotId;
        
        
        public Vector3 Position => transform.position;

        public bool IsOccupied => _mIsOccupied;
        private bool _mIsOccupied;
        
        public Guid CharacterInPlace => _mCurrentCharacterInPlace;
        private Guid _mCurrentCharacterInPlace;

        private IShopWaitingSpots _mWaitingParentSpot;

        public void Initialize(IShopWaitingSpots injectionClass)
        {
            if (_mIsInitialized)
            {
                return;
            }
            _mSpotId = Guid.NewGuid();
            _mWaitingParentSpot = injectionClass;
            _mCurrentCharacterInPlace = Guid.Empty;
            _mIsOccupied = false;
            _mIsInitialized = true;
        }

        #region ManageWaitingspotsApi
        public Tuple<ISingleWaitingSpot, bool> OccupyWaitingSpot(Guid occupierId)
        {
            if (occupierId == Guid.Empty || occupierId == _mCurrentCharacterInPlace || IsOccupied)
            {
                return new Tuple<ISingleWaitingSpot, bool>(null, false);
            }
            _mCurrentCharacterInPlace = occupierId;
            _mIsOccupied = true;
            return new Tuple<ISingleWaitingSpot, bool>(this, true);
        }

        public void ReleaseWaitingSpot(Guid occupierId)
        {
            if (occupierId != _mCurrentCharacterInPlace)
            {
                Debug.LogWarning("Only occupier can release waiting spot");
                return;
            }
            _mCurrentCharacterInPlace = Guid.Empty;
            _mIsOccupied = false;
        }

        public Guid GetCurrentOccupier => _mCurrentCharacterInPlace;

        #endregion

    }
}