using System;
using System.Collections.Generic;
using System.Linq;
using GamePlayManagement.LevelManagement.LevelObjectsManagement;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace GameDirection.GeneralLevelManager
{
    public interface IShopPositionsManager
    {
        public void Initialize();
        public List<Guid> GetShelvesOfInterestIds(int numberOfPois);
        public List<IShelfInMarket> GetShelvesOfInterestData(List<Guid> poisId);
        public void OccupyPoi(Guid occupier, Guid occupiedPoi);
        public void ReleasePoi(Guid occupier, Guid occupiedPoi);
        public Vector3 EntrancePosition();
        public Vector3 PayingPosition();
        public Vector3 InstantiatePosition();
        public Dictionary<Guid,IShelfInMarket> ShelfObjects { get; }
        public IShelfInMarket GetShelfObject(Guid shelfId);
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
        private int _positionsInLevel;

        private Dictionary<Guid, IShelfInMarket> mShelfObjects = new Dictionary<Guid, IShelfInMarket>();

        [SerializeField] private Transform payingPosition;
        [SerializeField] private Transform entranceTransform;
        [SerializeField] private Transform customerInstantiationTransform;
        public Dictionary<Guid, IShelfInMarket> ShelfObjects => mShelfObjects;

        public List<IShelfInMarket> GetShelvesOfInterestData(List<Guid> poisId)
        {
            var shelves = new List<IShelfInMarket>();
            foreach (var poiId in poisId)
            {
                if (!mShelfObjects.ContainsKey(poiId))
                {
                    continue;
                }
                shelves.Add(mShelfObjects[poiId]);
            }
            return shelves;
        }

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
            PopulateShelves();
            _positionsInLevel = mShelfObjects.Count;
            MIsInitialized = true;
        }

        private void PopulateShelves()
        {
            var shelfObjects = FindObjectsOfType<ShelfInMarket>();
            foreach (var shelfObject in shelfObjects)
            {
                var id = shelfObject.ShelfId;
                mShelfObjects.Add(id, shelfObject);
            }
        }

        public IShelfInMarket GetShelfObject(Guid shelfId)
        {
            if (!mShelfObjects.ContainsKey(shelfId))
            {
                return null;
            }
            return mShelfObjects[shelfId];
        }
    
        public List<Guid> GetShelvesOfInterestIds(int numberOfPois)
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
            var pickedShelves = new List<Guid>();
            for (var i = 0; i < numberOfPois; i++)
            {
                Guid randomPosition;
                while (pickedShelves.Contains(randomPosition) || randomPosition == Guid.Empty)
                {
                    var positionIndex = Random.Range(0, _positionsInLevel-1);
                    randomPosition = mShelfObjects.Keys.ElementAtOrDefault(positionIndex);
                }
                pickedShelves.Add(randomPosition);
            }
            return pickedShelves;
        }

        public void OccupyPoi(Guid occupier, Guid occupiedPoi)
        {
            if (!mShelfObjects.ContainsKey(occupiedPoi))
            {
                return;
            }   
            mShelfObjects[occupiedPoi].GetCustomerPoI.OccupyPoi(occupier);
            PrintPoiStatus();
        }

        public void ReleasePoi(Guid occupier, Guid occupiedPoi)
        {
            if (!mShelfObjects.ContainsKey(occupiedPoi))
            {
                return;
            }
            var poi = mShelfObjects[occupiedPoi].GetCustomerPoI;
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
            foreach (var shelfInMarket in mShelfObjects)
            {
                var isOccupied = shelfInMarket.Value.GetCustomerPoI.IsOccupied;
                if (isOccupied)
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

        public Vector3 EntrancePosition()
        {
            return entranceTransform.position;
        }
        public Vector3 InstantiatePosition()
        {
            return customerInstantiationTransform.position;
        }
        
        public Vector3 PayingPosition()
        {
            return payingPosition.position;
        }
    }
}