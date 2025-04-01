using System;
using GameDirection.GeneralLevelManager.ShopPositions;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.CustomerInterfaces;

namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management
{
    public class CustomerInstanceData : ICustomerInstanceData
    {
        private ICustomersInSceneManager _mCustomerManager;
        public CustomerInstanceData(ICustomersInSceneManager mCustomerManager, Guid customerId, IStoreEntrancePosition entrancePositions, ICustomerTypeData customerTypeData)
        {
            CustomerId = customerId;
            EntrancePositions = entrancePositions;
            CustomerTypeData = customerTypeData;
            _mCustomerManager = mCustomerManager;
        }

        public Guid CustomerId { get; set; }
        public IStoreEntrancePosition  EntrancePositions { get; set; }
        public ICustomerTypeData CustomerTypeData { get; set; }
        public ICustomersInSceneManager CustomerManager => _mCustomerManager;
    }
}