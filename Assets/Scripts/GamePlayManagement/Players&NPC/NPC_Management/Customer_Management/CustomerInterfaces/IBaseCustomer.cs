using System;
using GameDirection.GeneralLevelManager;
using GameDirection.GeneralLevelManager.ShopPositions;
using GamePlayManagement.LevelManagement.LevelObjectsManagement;
using UnityEngine;

namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.CustomerInterfaces
{
    public interface IBaseCustomer : IProductInterestManagement
    {
        public ICustomerTypeData CustomerTypeData { get; }
        public Guid CustomerId { get; }
        public ICustomerPurchaseStealData GetCustomerStoreVisitData { get; }
        public bool IsCustomerStealing { get; }
        public void SetCustomerId(Guid newId);
        public void SetInitialMovementData(IStoreEntrancePosition entranceData);
        public void SetCustomerTypeData(ICustomerTypeData customerTypeData);
    }

    public interface IProductInterestManagement
    {
        public Tuple<Transform, IStoreProductObjectData> TempStoreProductOfInterest { get; }
    }
}