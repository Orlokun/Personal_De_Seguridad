using System;
using GameDirection.GeneralLevelManager;

namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.CustomerInterfaces
{
    public interface IBaseCustomer
    {
        public ICustomerTypeData CustomerTypeData { get; }
        public Guid CustomerId { get; }
        public ICustomerPurchaseStealData GetCustomerStoreVisitData { get; }
        public bool IsCustomerStealing { get; }
        public void SetCustomerId(Guid newId);
        public void SetInitialMovementData(IStoreEntrancePosition entranceData);
        public void SetCustomerTypeData(ICustomerTypeData customerTypeData);
    }
}