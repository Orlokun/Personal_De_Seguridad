using System;
using GameDirection.GeneralLevelManager.ShopPositions;
using GamePlayManagement.LevelManagement.LevelObjectsManagement;
using UnityEngine;
using Utils;

namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.CustomerInterfaces
{
    public interface IBaseCustomer : IProductInterestManagement, IInitializeWithArg1<ICustomerInstanceData>
    {
        public ICustomerTypeData CustomerTypeData { get; }
        public Guid CustomerId { get; }
        public ICustomerPurchaseStealData GetCustomerStoreVisitData { get; }
        public bool IsCustomerStealing { get; }
        public GameObject CustomerGameObject { get; }
    }

    public interface IProductInterestManagement
    {
        public Tuple<Transform, IStoreProductObjectData> TempStoreProductOfInterest { get; }
    }
}