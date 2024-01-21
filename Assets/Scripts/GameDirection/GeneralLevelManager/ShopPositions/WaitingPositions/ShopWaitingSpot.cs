using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace GameDirection.GeneralLevelManager.ShopPositions.WaitingPositions
{
    public class ShopWaitingSpot : MonoBehaviour, IShopWaitingSpots, IInitialize
    {
        private bool _mIsInitialized;
        public Guid WaitingSpotId => _mWaitingPointId;
        public bool AnySpotAvailable => _characterSpots.Any(x => x.IsOccupied != true);
        public Guid _mWaitingPointId;

        [SerializeField] private List<Transform> characterSpotTransforms;
        private List<ISingleWaitingSpot> _characterSpots;
        public List<ISingleWaitingSpot> WaitingSpots => _characterSpots;
        public Tuple <ISingleWaitingSpot, bool> OccupyWaitingSpot(Guid occupier)
        {
            try
            {
                if (WaitingSpots.All(x => x.IsOccupied))
                {
                    return new Tuple<ISingleWaitingSpot, bool>(null, false);
                }
                var emptySpot = WaitingSpots.FirstOrDefault(x => x.IsOccupied == false);
                if (emptySpot == null)
                {
                    return new Tuple<ISingleWaitingSpot, bool>(null, false);
                }
                var occupiedSpot = emptySpot.OccupyWaitingSpot(occupier);
                return occupiedSpot;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void LeaveWaitingSpot(Guid occupier)
        {
            throw new NotImplementedException();
        }

        public bool IsInitialized => _mIsInitialized;
        public void Initialize()
        {
            if (_mIsInitialized)
            {
                return;
            }
            _mWaitingPointId = Guid.NewGuid();
            ConfirmWaitingPoints();
            _mIsInitialized = true;
        }
        private void ConfirmWaitingPoints()
        {
            if (characterSpotTransforms.Count == 0)
            {
                Debug.LogError($"[ShopWaitingSpots.ConfirmWaitingPoints] Waiting Spot {gameObject.name} must have single spots for characters available");
                return;
            }
            _characterSpots = new List<ISingleWaitingSpot>();
            foreach (var waitingSpotObject in characterSpotTransforms)
            {
                var waitingSpaceData = waitingSpotObject.GetComponent<ISingleWaitingSpot>();
                waitingSpaceData.Initialize(this);
                _characterSpots.Add(waitingSpaceData);
            }
        }
    }
}