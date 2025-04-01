using System;
using GamePlayManagement.LevelManagement.LevelObjectsManagement;
using UnityEngine;
using Utils;

namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.CustomerInterfaces
{
    public interface IBaseCustomer : IProductInterestManagement, IInitializeWithArg1<ICustomerInstanceData>, IBaseCharacterInScene
    {
        public ICustomerTypeData CustomerTypeData { get; }
        public Guid CustomerId { get; }
        public ICustomerPurchaseStealData GetCustomerStoreVisitData { get; }
        public bool IsCustomerStealing { get; }
        public GameObject CustomerGameObject { get; }
        public Guid MCurrentPoiId { get; }

        void SetTempProductOfInterest(Tuple<Transform, IStoreProductObjectData> chooseRandomProduct);
    }
}