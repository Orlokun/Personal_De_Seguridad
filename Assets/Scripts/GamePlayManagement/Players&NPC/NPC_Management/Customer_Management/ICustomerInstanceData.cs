using System;
using GameDirection.GeneralLevelManager.ShopPositions;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.CustomerInterfaces;

namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management
{
    public interface ICustomerInstanceData
    {
        public ICustomersInSceneManager CustomerManager { get; }
        public Guid CustomerId { get; }
        public IStoreEntrancePosition  EntrancePositions { get; }
        public ICustomerTypeData CustomerTypeData { get; }
    }
}