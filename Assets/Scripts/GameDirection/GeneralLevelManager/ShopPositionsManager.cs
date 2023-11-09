using System;
using GamePlayManagement.LevelManagement.LevelObjectsManagement;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace GameDirection.GeneralLevelManager
{
    public interface IShopPositionsManager
    {
        public void Initialize();
        public IShelfInMarket[] GetUnoccupiedShelf(int numberOfPositions);
        public bool OccupyPoi(Guid occupier, IShopPoiData occupiedPoi);
        public Vector3 EntrancePosition();
        public Vector3 PayingPosition();
        public Vector3 InstantiatePosition();
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
        
        private IShelfInMarket[] _mShelfObjects;

        [SerializeField] private Transform payingPosition;
        [SerializeField] private Transform entranceTransform;
        [SerializeField] private Transform customerInstantiationTransform;

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
            _mShelfObjects = (IShelfInMarket[])FindObjectsOfType<ShelfInMarket>();
            _positionsInLevel = _mShelfObjects.Length;
            MIsInitialized = true;
        }
    
        public IShelfInMarket[] GetUnoccupiedShelf(int numberOfPositions)
        {
            if (!MIsInitialized)
            {
                Debug.LogError("[GetUnoccupiedPositions] Not initialized");
                return null;
            }

            if (numberOfPositions <= 0)
            {
                Debug.LogWarning("[GetUnoccupiedPositions] Number of positions to get must be greater than 0");
                return null;
            }
        
            var emptyShelfs = new IShelfInMarket[numberOfPositions];
            for (var i = 0; i < emptyShelfs.Length; i++)
            {
                IShelfInMarket emptyPositionFound;
                do
                {
                    var positionIndex = Random.Range(0, _positionsInLevel);
                    emptyPositionFound = _mShelfObjects[positionIndex];
                } while (emptyPositionFound.GetCustomerPoI.IsOccupied);
                emptyShelfs[i] = emptyPositionFound;
            }
            return emptyShelfs;
        }

        public bool OccupyPoi(Guid occupier, IShopPoiData occupiedPoi)
        {
            if (occupiedPoi.IsOccupied)
            {
                return false;
            }
            occupiedPoi.OccupyPoi(occupier);
            return true;
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