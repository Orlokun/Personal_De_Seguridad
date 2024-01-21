using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameDirection.GeneralLevelManager.ShopPositions.CustomerPois;
using GameDirection.GeneralLevelManager.ShopPositions.WaitingPositions;
using GamePlayManagement.LevelManagement.LevelObjectsManagement;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace GameDirection.GeneralLevelManager.ShopPositions
{
    public interface IShopClientPoisManager
    {
        public List<Guid> GetFirstPoiOfInterestIds(int numberOfPois);
        public List<IShopPoiData> GetPoisOfInterestData(List<Guid> poisId);
        public void OccupyPoi(Guid occupier, Guid occupiedPoi);
        public void ReleasePoi(Guid occupier, Guid occupiedPoi);
        public IShopPoiData GetPoiData(Guid poiId);
    }

    public interface IShopPositionsManager : IShopClientPoisManager, IShopGenericWaitingSpots
    {
        public void Initialize();
        public List<IStoreEntrancePosition> StartPositions { get; }
        public Vector3 PayingPosition();
    }

    /// <summary>
    /// This shop positions manager handles the places in which a Customer will stop to grab/steal products
    /// Requiremente: One object must have all the childrens so they can be tracked
    /// Currently the system is based on a Game Objects that takes the Transform of all of its children and saves them into
    /// the m_positionObjects array. From this array the Customer takes the positions it will visit randomly before going
    /// to pay (If it pays at all)
    /// </summary>
    public class ShopPositionsManager : InitializeManager, IShopPositionsManager
    {
        private Dictionary<Guid, IShopPoiData> mPoiDatas = new Dictionary<Guid, IShopPoiData>();
        private Dictionary<Guid, IShopWaitingSpots> _mWaitingSpots = new Dictionary<Guid, IShopWaitingSpots>();

        [SerializeField] private Transform payingPosition;
        
        
        [SerializeField] private List<Transform> entranceTransforms;
        [SerializeField] private List<Transform> customerInstantiationTransforms;

        private List<IStoreEntrancePosition> _mStartPositions = new List<IStoreEntrancePosition>();
        public List<IStoreEntrancePosition> StartPositions => _mStartPositions;
        
        private int _positionsInLevel;

        #region Init
        private void Awake()
        {
            this.Initialize();
        }
        public override void Initialize()
        {
            if (MIsInitialized)
            {
                return;
            }
            BaseInitialization();
        }
        protected void BaseInitialization()
        {
            // ReSharper disable once CoVariantArrayConversion
            PopulateEntrancePositions();
            PopulatePois();
            PopulateWaitingSpots();
            PopulateGuardRoutes();
            _positionsInLevel = mPoiDatas.Count;
            MIsInitialized = true;
        }

        private void PopulateEntrancePositions()
        {
            if (entranceTransforms.Count != customerInstantiationTransforms.Count)
            {
                Debug.LogWarning("[ShopPositionManager.PopulateEntrancePosition] Entrance and Instantiation positions must be the same");
            }

            for (var i = 0; i < customerInstantiationTransforms.Count; i++)
            {
                var instantiationTransform = customerInstantiationTransforms[i];
                var startPosition = Factory.CreateStartPosition(instantiationTransform, entranceTransforms[i]);
                StartPositions.Add(startPosition);
            }
        }
        private void PopulatePois()
        {
            var shelfObjects = FindObjectsOfType<ShelfInMarket>();
            foreach (var shelfObject in shelfObjects)
            {
                shelfObject.Initialize();
                foreach (var poi in shelfObject.GetAllPois)
                {
                    mPoiDatas.Add(poi.PoiId, poi);
                }
            }
        }

        private void PopulateWaitingSpots()
        {
            var waitingSpots = FindObjectsOfType<ShopWaitingSpot>();
            foreach (var waitingSpot in waitingSpots)
            {
                waitingSpot.Initialize();
                _mWaitingSpots.Add(waitingSpot.WaitingSpotId, waitingSpot);
            }
        }

        private void PopulateGuardRoutes()
        {
            Debug.LogWarning("[PopulateGuardRoutes] Not Implemented Yet");
        }
        #endregion

        #region PoiManagement
        public List<IShopPoiData> GetPoisOfInterestData(List<Guid> poisId)
        {
            var poisData = new List<IShopPoiData>();
            foreach (var poiId in poisId)
            {
                if (!mPoiDatas.ContainsKey(poiId))
                {
                    continue;
                }
                poisData.Add(mPoiDatas[poiId]);
            }
            return poisData;
        }

        public IShopPoiData GetPoiData(Guid poiId)
        {
            if (!mPoiDatas.ContainsKey(poiId))
            {
                Debug.LogWarning("[GetPoiData] Poi key must be available in dict");
                return null;
            }
            return mPoiDatas[poiId];
        }
        public List<Guid> GetFirstPoiOfInterestIds(int numberOfPois)
        {
            if (!MIsInitialized)
            {
                Debug.LogError("[GetUnoccupiedPositions] Not initialized");
                return null;
            }

            if (numberOfPois <= 0)
            {
                Debug.LogWarning("[GetUnoccupiedPositions] Number of positions to get must be greater than 0");
                return null;
            }

            Random.InitState(DateTime.Now.Millisecond);
            var pickedPois = new List<Guid>();
            for (var i = 0; i < numberOfPois; i++)
            {
                var randomPosition = Guid.Empty;
                while (pickedPois.Contains(randomPosition) || randomPosition == Guid.Empty)
                {
                    var positionIndex = Random.Range(0, _positionsInLevel-1);
                    randomPosition = mPoiDatas.Keys.ElementAtOrDefault(positionIndex);
                }
                pickedPois.Add(randomPosition);
            }
            return pickedPois;
        }
        public void OccupyPoi(Guid occupier, Guid occupiedPoi)
        {
            if (!mPoiDatas.ContainsKey(occupiedPoi))
            {
                return;
            }   
            mPoiDatas[occupiedPoi].OccupyPoi(occupier);
            PrintPoiStatus();
        }
        public void ReleasePoi(Guid occupier, Guid occupiedPoi)
        {
            if (!mPoiDatas.ContainsKey(occupiedPoi))
            {
                return;
            }
            var poi = mPoiDatas[occupiedPoi];
            if (!poi.IsOccupied || poi.OccupierId != occupier)
            {
                return;
            }
            poi.LeavePoi(occupier);
            PrintPoiStatus();
        }
        private void PrintPoiStatus()
        {
            var occupiedShelves = 0;
            var unOccupiedShelves = 0;

            foreach (var poi in mPoiDatas)
            {
                if (poi.Value.IsOccupied)
                {
                    occupiedShelves++;
                }
                else
                {
                    unOccupiedShelves++;
                }
            }
            Debug.Log($"Occupied Shelves: {occupiedShelves}. Unoccupied Shelves: {unOccupiedShelves}");
        }
        #endregion

        #region CustomerInstantiation
        public IStoreEntrancePosition RandomStartPosition()
        {
            Random.InitState(DateTime.Now.Second);
            var randomEntrance = Random.Range(0, _mStartPositions.Count -1);
            return _mStartPositions[randomEntrance];        
        }

        public Vector3 PayingPosition()
        {
            return payingPosition.position;
        }
        #endregion

        #region WaitingPositions
        public IShopWaitingSpots GetWaitingSpot(Guid waitingPointId)
        {
            if (!_mWaitingSpots.ContainsKey(waitingPointId))
            {
                Debug.LogWarning("[GetWaitingSpot] Asked Waiting spot must be available");
                return null;
            }
            return _mWaitingSpots[waitingPointId];
        }

        private int _processSemaphore = 0;
        private int iterationsThreshold = 10;
        private int currentIteration = 0;
        public async Task<Tuple<ISingleWaitingSpot, bool>> OccupyEmptyWaitingSpot(Guid occupierCharacter)
        {
            if (_processSemaphore > 0)
            {
                do
                {
                    await Task.Delay(10);
                    currentIteration++;
                    if (currentIteration >= iterationsThreshold)
                    {
                        currentIteration = 0;
                        return new Tuple<ISingleWaitingSpot, bool>(null, false);
                    }
                } while (_processSemaphore != 0);
            }
            _processSemaphore++;
            var anyPointAvailable = _mWaitingSpots.Any(x => x.Value.AnySpotAvailable);
            if (anyPointAvailable)
            {
                var resultTuple = FindAndOccupyEmptyObject(occupierCharacter);
                _processSemaphore--;
                return resultTuple;
            }
            _processSemaphore--;
            return new Tuple<ISingleWaitingSpot, bool>(null, false);
        }

        private Tuple<ISingleWaitingSpot, bool> FindAndOccupyEmptyObject(Guid occupierCharacter)
        {
            var spotEmptySpace = _mWaitingSpots.FirstOrDefault(x => x.Value.AnySpotAvailable);
            var singleSpot = spotEmptySpace.Value.OccupyWaitingSpot(occupierCharacter);
            return singleSpot;
        }
        public bool AnyUnoccupiedSpot => _mWaitingSpots.Any(x => x.Value.AnySpotAvailable);
        #endregion

    }
}